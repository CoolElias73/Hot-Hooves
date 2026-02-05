using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverTextSlide : MonoBehaviour
{
    [SerializeField] private RisingLava lava;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private float offscreenPadding = 80f;
    [SerializeField] private float minDuration = 0.1f;
    [SerializeField] private float postSlideDelay = 2f;
    [SerializeField] private string highscoreKey = "HighscoreHeight";

    private RectTransform _rect;
    private Canvas _canvas;
    private Coroutine _slideRoutine;
    private float _playerStartY;
    private bool _hasStartY;

    void Awake()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        if (text != null)
            text.enabled = false;
    }

    void OnEnable()
    {
        if (lava == null)
            lava = FindFirstObjectByType<RisingLava>();
        if (lava != null)
            lava.PlayerTouched += OnPlayerTouched;

        if (!_hasStartY)
            CacheStartHeight();
    }

    void OnDisable()
    {
        if (lava != null)
            lava.PlayerTouched -= OnPlayerTouched;
    }

    private void OnPlayerTouched(Transform player, float lavaSpeed)
    {
        if (_rect == null || text == null)
            return;

        var hud = FindFirstObjectByType<EliasMStatsHUD>();
        if (hud != null)
            hud.HideMainStats();

        float duration = CalculateDuration(lavaSpeed);
        if (_slideRoutine != null)
            StopCoroutine(_slideRoutine);

        UpdateHighscore(player);
        text.enabled = true;
        _slideRoutine = StartCoroutine(SlideToCenter(duration));
    }

    private float CalculateDuration(float lavaSpeed)
    {
        var cam = Camera.main;
        if (cam == null || lava == null)
            return minDuration;

        float lavaTopY = lava.transform.position.y;
        var lavaRenderer = lava.GetComponent<Renderer>();
        if (lavaRenderer != null)
            lavaTopY = lavaRenderer.bounds.max.y;

        float camTopY = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
        float distance = camTopY - lavaTopY;
        float speed = Mathf.Max(0.01f, lavaSpeed);
        float duration = distance / speed;
        return Mathf.Max(minDuration, duration);
    }

    private System.Collections.IEnumerator SlideToCenter(float duration)
    {
        float canvasHeight = _canvas != null ? _canvas.pixelRect.height : Screen.height;
        Vector2 start = new Vector2(0f, -(canvasHeight * 0.5f) - offscreenPadding);
        Vector2 end = Vector2.zero;

        _rect.anchoredPosition = start;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            _rect.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }
        _rect.anchoredPosition = end;

        if (postSlideDelay > 0f)
            yield return new WaitForSeconds(postSlideDelay);

        if (highscoreText != null)
            highscoreText.enabled = true;
        if (mainMenuButton != null)
            mainMenuButton.gameObject.SetActive(true);
        if (retryButton != null)
            retryButton.gameObject.SetActive(true);

        _slideRoutine = null;
    }

    private void CacheStartHeight()
    {
        var player = FindFirstObjectByType<Movements>();
        if (player != null)
        {
            _playerStartY = player.transform.position.y;
            _hasStartY = true;
        }
    }

    private void UpdateHighscore(Transform player)
    {
        float endHeight = player != null ? player.position.y : 0f;
        if (_hasStartY)
            endHeight -= _playerStartY;

        float best = PlayerPrefs.GetFloat(highscoreKey, endHeight);
        if (endHeight > best)
        {
            best = endHeight;
            PlayerPrefs.SetFloat(highscoreKey, best);
        }

        if (highscoreText != null)
        {
            highscoreText.text = $"Highscore: {best:0.0} m";
            highscoreText.enabled = false;
        }
    }

    private void OnMainMenuClicked()
    {
        SceneManager.LoadScene(0);
    }

    private void OnRetryClicked()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Initialize(RisingLava lavaRef, float offscreenPaddingValue, TMP_Text highscoreTextRef, Button mainMenuButtonRef, Button retryButtonRef)
    {
        lava = lavaRef;
        offscreenPadding = offscreenPaddingValue;
        highscoreText = highscoreTextRef;
        mainMenuButton = mainMenuButtonRef;
        retryButton = retryButtonRef;
        if (text != null)
            text.enabled = false;
        if (highscoreText != null)
            highscoreText.enabled = false;
        if (mainMenuButton != null)
            mainMenuButton.gameObject.SetActive(false);
        if (retryButton != null)
            retryButton.gameObject.SetActive(false);

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }
        if (retryButton != null)
        {
            retryButton.onClick.RemoveListener(OnRetryClicked);
            retryButton.onClick.AddListener(OnRetryClicked);
        }
    }
}
