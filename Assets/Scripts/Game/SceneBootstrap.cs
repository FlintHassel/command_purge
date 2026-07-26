using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

/// <summary>
/// Dipasang di scene House. Saat play, scene Computer di-load secara additive
/// sehingga Canvas terminal tersedia di runtime (dirender oleh ScreenCamera ke TerminalRT).
/// ScreenCamera di Computer scene merender Canvas WorldSpace ke TerminalRT.
/// Quad 'Screen' di House scene menggunakan TerminalRT sebagai texture.
/// </summary>
public class SceneBootstrap : MonoBehaviour
{
    public static SceneBootstrap Instance { get; private set; }

    private GameObject computerEventSystem;
    private Camera screenCamera;
    private Canvas computerCanvas;
    private RenderTexture originalRT;
    private float originalDepth;

    [Header("Additive Scene Settings")]
    [Tooltip("Nama scene yang akan di-load additive. Harus ada di Build Settings.")]
    [SerializeField] private string computerSceneName = "Computer";

    [Header("Dependencies")]
    [Tooltip("PlayerStateManager di-instantiate otomatis jika kosong.")]
    [SerializeField] private PlayerStateManager playerStateManagerPrefab;

    private void Awake()
    {
        Instance = this;
        EnsurePlayerStateManager();
    }

    private void Start()
    {
        if (!IsSceneLoaded(computerSceneName))
        {
            StartCoroutine(LoadComputerScene());
        }
        else
        {
            Debug.Log("[SceneBootstrap] Computer scene sudah ter-load.");
            DisableComputerSceneConflicts();
        }
    }

    private IEnumerator LoadComputerScene()
    {
        Debug.Log("[SceneBootstrap] Loading Computer scene additively...");
        AsyncOperation op = SceneManager.LoadSceneAsync(computerSceneName, LoadSceneMode.Additive);
        op.allowSceneActivation = true;

        while (!op.isDone)
            yield return null;

        Debug.Log("[SceneBootstrap] Computer scene loaded!");
        DisableComputerSceneConflicts();
    }

    private void DisableComputerSceneConflicts()
    {
        Scene computerScene = SceneManager.GetSceneByName(computerSceneName);
        if (!computerScene.isLoaded) return;

        GameObject[] rootObjects = computerScene.GetRootGameObjects();
        if (rootObjects.Length > 0)
        {
            // Find duplicate EventSystem and disable it
            foreach (GameObject go in rootObjects)
            {
                if (go.name == "EventSystem")
                {
                    computerEventSystem = go;
                    go.SetActive(false);
                }
                if (go.name == "Directional Light")
                {
                    go.SetActive(false);
                }
            }

            // Temukan Canvas
            computerCanvas = rootObjects
                .SelectMany(root => root.GetComponentsInChildren<Canvas>(true))
                .FirstOrDefault();

            // Temukan ScreenCamera
            screenCamera = rootObjects
                .SelectMany(root => root.GetComponentsInChildren<Camera>(true))
                .FirstOrDefault(c => c.name == "ScreenCamera" || c.targetTexture != null);

            if (screenCamera != null)
            {
                originalRT = screenCamera.targetTexture;
                originalDepth = screenCamera.depth;

                // Jika tidak memakai RenderTexture, berarti ini langsung merender ke layar.
                // Kita sembunyikan awalnya agar tidak bocor menutupi layar House.
                if (originalRT == null)
                {
                    screenCamera.enabled = false;
                    if (computerCanvas != null) computerCanvas.enabled = false;
                }
            }
            else
            {
                // Jika tidak ada kamera sama sekali tapi ada Canvas Overlay
                if (computerCanvas != null) computerCanvas.enabled = false;
            }
        }
    }

    public void SetComputerSceneActive(bool isActive)
    {
        if (screenCamera != null)
        {
            if (originalRT == null)
            {
                screenCamera.enabled = isActive;
            }
            else
            {
                screenCamera.targetTexture = isActive ? null : originalRT;
            }
            screenCamera.depth = isActive ? 99 : originalDepth;
        }

        if (computerCanvas != null && originalRT == null)
        {
            computerCanvas.enabled = isActive;
        }

        if (computerEventSystem != null) computerEventSystem.SetActive(isActive);
    }

    private void EnsurePlayerStateManager()
    {
        if (PlayerStateManager.Instance == null)
        {
            if (playerStateManagerPrefab != null)
            {
                Instantiate(playerStateManagerPrefab);
            }
            else
            {
                // Buat PlayerStateManager baru secara dinamis
                GameObject psm = new GameObject("PlayerStateManager");
                psm.AddComponent<PlayerStateManager>();
                Debug.Log("[SceneBootstrap] PlayerStateManager dibuat secara dinamis.");
            }
        }
    }

    private bool IsSceneLoaded(string sceneName)
    {
        Scene s = SceneManager.GetSceneByName(sceneName);
        return s.IsValid() && s.isLoaded;
    }
}
