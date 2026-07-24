using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("Referensi Objek")]
    public Transform playerCamera;
    public Transform pickupSlot;
    public LayerMask pickableLayer;

    [Header("Pengaturan Jarak")]
    public float hitRange = 3f;

    [Header("UI Interaction Prompt")]
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TextMeshProUGUI interactionText;

    [Header("Pengaturan Visual Kertas Di Tangan")]
    [SerializeField] private float heldScale = 0.055f;
    [SerializeField] private Vector3 heldPositionOffset = new Vector3(-0.023f, -0.02f, 0.027f);
    [SerializeField] private Vector3 heldRotationOffset = new Vector3(-10f, 10f, 5f);

    private GameObject inHandItem;
    private Highlight currentHighlight;
    private PickableItem currentPickable;

    void Awake()
    {
        if (playerCamera == null)
        {
            Camera cam = FindFirstObjectByType<Camera>();
            if (cam != null) playerCamera = cam.transform;
        }
        if (pickupSlot == null && playerCamera != null)
            pickupSlot = playerCamera.Find("PickupSlot");
        if (pickableLayer == 0)
            pickableLayer = LayerMask.GetMask("Pickable");

        // Auto-find UI references jika tidak di-assign di Inspector
        if (interactionPanel == null)
        {
            GameObject panel = GameObject.Find("InteractionUIPanel");
            if (panel != null)
            {
                interactionPanel = panel;
                if (interactionText == null)
                    interactionText = panel.GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }

    void Update()
    {
        HandleRaycast();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inHandItem == null)
            {
                if (currentHighlight != null)
                    TryPickUp(currentHighlight.gameObject);
                else if (currentPickable != null)
                    TryPickUp(currentPickable.gameObject);
            }
        }
    }

    private void HandleRaycast()
    {
        if (currentHighlight != null)
        {
            currentHighlight.ToggleHighlight(false);
            currentHighlight = null;
        }
        currentPickable = null;

        if (inHandItem != null)
        {
            // Jangan sentuh UI apapun — biar InteractionManager tetap bisa nampilin prompt ladder/computer
            return;
        }

        if (playerCamera == null) return;

        // 1. Raycast layer Pickable (layer 8) untuk object Highlight
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, hitRange, pickableLayer, QueryTriggerInteraction.Collide))
        {
            Highlight highlight = hit.collider.GetComponent<Highlight>();
            if (highlight != null)
            {
                highlight.ToggleHighlight(true);
                currentHighlight = highlight;
                ShowInteractionUI("Press [E] to pick up the paper");
                return;
            }

            PickableItem pickable = hit.collider.GetComponent<PickableItem>();
            if (pickable != null && pickable.enabled)
            {
                currentPickable = pickable;
                string prompt = pickable.GetInteractText();
                if (!string.IsNullOrEmpty(prompt))
                    ShowInteractionUI(prompt);
                else
                    HideInteractionUI();
                return;
            }
        }

        // 2. Juga raycast layer Interactable (layer 6) — tapi hanya untuk PickableItem (kertas)
        LayerMask interactableLayer = 1 << 6;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit2, hitRange, interactableLayer, QueryTriggerInteraction.Collide))
        {
            PickableItem pickable = hit2.collider.GetComponent<PickableItem>();
            if (pickable != null && pickable.enabled)
            {
                currentPickable = pickable;
                string prompt = pickable.GetInteractText();
                if (!string.IsNullOrEmpty(prompt))
                    ShowInteractionUI(prompt);
                else
                    HideInteractionUI();
                return;
            }
        }

        // Tidak ada yang terdeteksi
        HideInteractionUI();
    }

    private void ShowInteractionUI(string text)
    {
        if (interactionPanel != null) interactionPanel.SetActive(true);
        if (interactionText != null) interactionText.text = text;
    }

    private void HideInteractionUI()
    {
        if (interactionPanel != null && interactionPanel.activeSelf)
            interactionPanel.SetActive(false);
    }

    private void TryPickUp(GameObject item)
    {
        // Dukung baik PaperItem maupun PickableItem
        if (item.GetComponent<PaperItem>() != null || item.GetComponent<PickableItem>() != null)
        {
            inHandItem = item;

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            item.transform.SetParent(pickupSlot, false);

            item.transform.localPosition = heldPositionOffset;
            item.transform.localRotation = Quaternion.Euler(heldRotationOffset);
            item.transform.localScale = new Vector3(heldScale, heldScale, heldScale);

            // Daftarkan ke PlayerInventory supaya PipeSlot bisa deteksi
            PickableItem pickable = item.GetComponent<PickableItem>();
            if (pickable != null && PlayerInventory.Instance != null)
            {
                PlayerInventory.Instance.heldItem = pickable;
            }

            // Matikan collider supaya tidak trigger event aneh
            Collider col = item.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            // Apply generated paper texture + material anti-clipping
            // MODIFIKASI material asli (URP Lit printer), bukan ganti shader!
            Renderer[] renderers = item.GetComponentsInChildren<Renderer>();
            Texture2D paperTex = PaperTextureGenerator.Generate(256, 256);
            foreach (Renderer rend in renderers)
            {
                Material mat = rend.material;
                mat.mainTexture = paperTex;
                mat.color = Color.white;
                mat.SetFloat("_Surface", 0f); // Opaque
                mat.SetColor("_EmissionColor", Color.white * 0.3f); // subtle glow biar selalu terang
                mat.EnableKeyword("_EMISSION");

                // ZTest Always + ZWrite ON + Cull Off
                mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                mat.SetInt("_ZWrite", 1);
                mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                mat.renderQueue = 2000;

                rend.material = mat;

                // Expand mesh bounds supaya renderer tidak di-frustum cull
                MeshFilter mf = rend.GetComponent<MeshFilter>();
                if (mf != null && mf.sharedMesh != null)
                {
                    mf.sharedMesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100);
                }
            }

            HideInteractionUI();

            if (currentHighlight != null)
            {
                currentHighlight.ToggleHighlight(false);
                currentHighlight = null;
            }
            currentPickable = null;
        }
    }
}
