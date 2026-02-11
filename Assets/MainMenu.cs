using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;

    public void PlayGame()
    {
        var optionsMenu = FindFirstObjectByType<OptionsMenu>();
        if (optionsMenu == null)
        {
            var allOptionsMenus = Resources.FindObjectsOfTypeAll<OptionsMenu>();
            for (int i = 0; i < allOptionsMenus.Length; i++)
            {
                if (allOptionsMenus[i] != null && allOptionsMenus[i].gameObject.scene.IsValid())
                {
                    optionsMenu = allOptionsMenus[i];
                    break;
                }
            }
        }

        if (optionsMenu != null)
            optionsMenu.ApplyCurrentSettings();
        else
            PlayerPrefs.Save();

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }
}
