using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool showOptions;
    private static Texture2D buttonTex;
    private static GUIStyle buttonStyle;

    void OnGUI()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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

    public GameObject optionsPanel;

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public GameObject optionsPanel;

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }
}

