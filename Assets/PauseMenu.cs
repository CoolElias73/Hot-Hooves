using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    bool isPaused = false;
    InputAction pauseAction;
    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void OnEnable()
    {
        if (pauseAction == null)
        {
            pauseAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
        }
        pauseAction.Enable();
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.Disable();
        }
    }

    void Update()
    {
        if (pauseAction != null && pauseAction.WasPressedThisFrame())
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
