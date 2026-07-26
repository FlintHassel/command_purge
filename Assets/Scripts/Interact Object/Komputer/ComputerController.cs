tusing UnityEngine;
using System.Collections;

public class ComputerController : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Transform cameraSitPosition;
    [SerializeField] private FPSMovement playerMovement;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private GameObject crosshairUI;

    [Header("Cinemachine (Unity 6 Virtual Camera)")]
    [SerializeField] private GameObject cinemachineCameraObject;

    [Header("Settings")]
    [SerializeField] private float lerpSpeed = 0.6f;
    [SerializeField] private float lookSensitivity = 0.5f;
    [SerializeField] private float maxLookAngle = 15f;

    private bool isUsing = false;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private Coroutine cameraAnimCoroutine;
    private Transform originalParent;
    private Quaternion _computerBaseRotation;
    private float _computerLookYaw;

    [Header("Scene Transition Settings")]
    [Tooltip("Nama scene UI Komputer yang akan diload. (Harus ada di File -> Build Settings)")]
    [SerializeField] private string computerSceneName = "Ui Computer";
    private bool isComputerSceneLoaded = false;
    private GameObject houseEventSystem;

    private void Awake()
    {
        if (crosshairUI == null)
            crosshairUI = GameObject.Find("Crosshair");
    }

    public bool IsUsing => isUsing;

    public void Interact()
    {
        if (cameraAnimCoroutine != null) return;

        if (playerCameraTransform == null || cameraSitPosition == null || playerMovement == null)
        {
            Debug.LogError($"[{gameObject.name}] Tolong lengkapi slot variable di Inspector! Ada yang masih kosong.", this);
            return;
        }

        if (isUsing)
        {
            // Abaikan Interaksi 'E' jika sedang menggunakan komputer (keluar pakai ESC)
            return;
        }

        cameraAnimCoroutine = StartCoroutine(EnterComputerAnimation());
    }

    public string GetInteractText()
    {
        return isUsing ? "Press [ESC] to Quit" : "Press [E] to Use Computer";
    }

    void Update()
    {
        if (!isUsing) return;

        if (cameraAnimCoroutine == null && Input.GetKeyDown(KeyCode.Escape))
        {
            cameraAnimCoroutine = StartCoroutine(ExitComputerAnimation());
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        _computerLookYaw = Mathf.Clamp(_computerLookYaw + mouseX, -maxLookAngle, maxLookAngle);
        playerCameraTransform.rotation = _computerBaseRotation * Quaternion.Euler(0, _computerLookYaw, 0);
    }

    private IEnumerator EnterComputerAnimation()
    {
        playerMovement.enabled = false;

        originalParent = playerCameraTransform.parent;
        originalCameraPosition = playerCameraTransform.position;
        originalCameraRotation = playerCameraTransform.rotation;

        if (cinemachineCameraObject != null) cinemachineCameraObject.SetActive(false);

        playerCameraTransform.SetParent(null);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (crosshairUI != null) crosshairUI.SetActive(false);

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            playerCameraTransform.position = Vector3.Lerp(originalCameraPosition, cameraSitPosition.position, elapsed);
            playerCameraTransform.rotation = Quaternion.Slerp(originalCameraRotation, cameraSitPosition.rotation, elapsed);
            elapsed += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        playerCameraTransform.position = cameraSitPosition.position;
        playerCameraTransform.rotation = cameraSitPosition.rotation;

        _computerBaseRotation = playerCameraTransform.rotation;
        _computerLookYaw = 0f;

        // --- BUKA SCENE KOMPUTER DENGAN AMAN ---
        if (!isComputerSceneLoaded && !IsSceneLoaded(computerSceneName))
        {
            UnityEngine.AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(computerSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            if (op != null)
            {
                while (!op.isDone) yield return null;
                isComputerSceneLoaded = true;
            }
            else
            {
                Debug.LogError($"[ComputerController] GAGAL MEMUAT SCENE! Pastikan scene '{computerSceneName}' sudah ditambahkan ke File -> Build Settings!");
            }
        }
        else if (isComputerSceneLoaded)
        {
            SetComputerSceneObjectsActive(true);
        }

        // Jangan matikan houseEventSystem agar UI di scene Computer bisa tetap di-klik!

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isUsing = true;
        cameraAnimCoroutine = null;

        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.SetState(PlayerState.UsingComputer);
        }
    }

    private IEnumerator ExitComputerAnimation()
    {
        isUsing = false;

        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.SetState(PlayerState.Exploring);
        }

        // --- PROSES MATIKAN SCENE KOMPUTER ---
        SetComputerSceneObjectsActive(false);

        Vector3 startPos = playerCameraTransform.position;
        Quaternion startRot = playerCameraTransform.rotation;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            playerCameraTransform.position = Vector3.Lerp(startPos, originalCameraPosition, elapsed);
            playerCameraTransform.rotation = Quaternion.Slerp(startRot, originalCameraRotation, elapsed);
            elapsed += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        playerCameraTransform.position = originalCameraPosition;
        playerCameraTransform.rotation = originalCameraRotation;

        playerCameraTransform.SetParent(originalParent);

        if (cinemachineCameraObject != null) cinemachineCameraObject.SetActive(true);
        playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (crosshairUI != null) crosshairUI.SetActive(true);

        isUsing = false;
        cameraAnimCoroutine = null;
    }

    private bool IsSceneLoaded(string sceneName)
    {
        UnityEngine.SceneManagement.Scene s = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        return s.IsValid() && s.isLoaded;
    }

    private void SetComputerSceneObjectsActive(bool active)
    {
        UnityEngine.SceneManagement.Scene s = UnityEngine.SceneManagement.SceneManager.GetSceneByName(computerSceneName);
        if (s.IsValid() && s.isLoaded)
        {
            GameObject[] rootObjects = s.GetRootGameObjects();
            foreach (GameObject go in rootObjects)
            {
                go.SetActive(active);
            }
        }
    }
}
