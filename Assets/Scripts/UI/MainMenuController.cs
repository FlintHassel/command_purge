using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Settings")]
    public string gameSceneName = "House";

    private void Start()
    {
        // Make sure cursor is visible and unlocked in the main menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Ensure time scale is normal (in case we quit from a paused state)
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        Debug.Log("Starting game... Loading scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
