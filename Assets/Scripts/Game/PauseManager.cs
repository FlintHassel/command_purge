using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Panel UI untuk Pause Menu")]
    [SerializeField] private GameObject pauseMenuUI;

    public bool isPaused { get; private set; } = false;

    private void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Jangan buka pause menu jika player sedang menggunakan komputer
        if (PlayerStateManager.Instance != null && PlayerStateManager.Instance.CurrentState == PlayerState.UsingComputer)
        {
            return;
        }

        // Jangan buka pause menu jika ada cutscene
        if (PlayerStateManager.Instance != null && PlayerStateManager.Instance.CurrentState == PlayerState.Cutscene)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
        // Kunci kursor kembali saat resume bermain
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Buka kursor agar bisa klik tombol di pause menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
