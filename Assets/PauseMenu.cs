using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";
    private bool isPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    void OnGUI()
    {
        if (!isPaused)
            return;

        float width = 240f;
        float height = 55f;
        float x = (Screen.width - width) * 0.5f;
        float y = (Screen.height - (height * 3f + 20f)) * 0.5f;

        GUI.Box(new Rect(x - 20f, y - 30f, width + 40f, height * 3f + 80f), "Paused");

        if (GUI.Button(new Rect(x, y, width, height), "Resume"))
            TogglePause();

        if (GUI.Button(new Rect(x, y + height + 10f, width, height), "Main Menu"))
            LoadMainMenu();

        if (GUI.Button(new Rect(x, y + (height + 10f) * 2f, width, height), "Quit"))
            QuitGame();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
