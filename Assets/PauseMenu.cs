using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    bool isPaused = false;
    InputAction pauseAction;
    void Awake()
    {
        if (pausePanel == null)
        {
            var direct = transform.Find("PausePanel");
            if (direct != null)
                pausePanel = direct.gameObject;
        }

        if (pausePanel == null)
        {
            var found = GameObject.Find("PausePanel");
            if (found != null)
                pausePanel = found;
        }

        if (pausePanel == null)
        {
            Debug.LogError("PauseMenu: pausePanel is not assigned. Assign it in the inspector.", this);
            enabled = false;
            return;
        }
    }

    void Start()
    {
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
        if (pausePanel == null)
            return;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        if (pausePanel == null)
            return;
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
