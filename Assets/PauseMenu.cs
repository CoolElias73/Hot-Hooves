using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";
    private bool isPaused;
    private static Texture2D buttonTex;
    private static GUIStyle buttonStyle;

    void Update()
    {
        if (IsEscapePressedThisFrame())
            TogglePause();
    }

    void OnGUI()
    {
        if (!isPaused)
            return;

        EnsureStyles();
        float width = 240f;
        float height = 55f;
        float x = (Screen.width - width) * 0.5f;
        float y = (Screen.height - (height * 3f + 20f)) * 0.5f;

        if (GUI.Button(new Rect(x, y, width, height), "Resume", buttonStyle))
            TogglePause();

        if (GUI.Button(new Rect(x, y + height + 10f, width, height), "Main Menu", buttonStyle))
            LoadMainMenu();

        if (GUI.Button(new Rect(x, y + (height + 10f) * 2f, width, height), "Quit", buttonStyle))
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    bool IsEscapePressedThisFrame()
    {
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
    }

    void EnsureStyles()
    {
        if (buttonTex == null)
        {
            buttonTex = new Texture2D(1, 1);
            buttonTex.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.6f));
            buttonTex.Apply();
        }

        if (buttonStyle == null)
        {
            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.background = buttonTex;
            buttonStyle.hover.background = buttonTex;
            buttonStyle.active.background = buttonTex;
            buttonStyle.focused.background = buttonTex;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.hover.textColor = Color.white;
            buttonStyle.active.textColor = Color.white;
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.fontSize = 20;
        }
    }
}
