using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool showOptions;
    private static Texture2D buttonTex;
    private static GUIStyle buttonStyle;

    void OnGUI()
    {
        EnsureStyles();
        float width = 220f;
        float height = 50f;
        float x = (Screen.width - width) * 0.5f;
        float y = (Screen.height - (height * 3f + 20f)) * 0.5f;

        if (!showOptions)
        {
            if (GUI.Button(new Rect(x, y, width, height), "Play", buttonStyle))
                LoadGame();

            if (GUI.Button(new Rect(x, y + height + 10f, width, height), "Options", buttonStyle))
                showOptions = true;

            if (GUI.Button(new Rect(x, y + (height + 10f) * 2f, width, height), "Quit", buttonStyle))
                QuitGame();
        }
        else
        {
            if (GUI.Button(new Rect(x, y + height + 10f, width, height), "Back", buttonStyle))
                showOptions = false;
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
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
}

