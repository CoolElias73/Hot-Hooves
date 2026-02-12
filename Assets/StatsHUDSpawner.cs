using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsHUDSpawner : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private Vector2 topRightPadding = new Vector2(20f, 20f);
    [SerializeField] private Vector2 bottomPadding = new Vector2(0f, 20f);
    [SerializeField] private Vector2 topRightSize = new Vector2(360f, 120f);
    [SerializeField] private Vector2 bottomSize = new Vector2(520f, 90f);
    [SerializeField] private float topRightFontSize = 24f;
    [SerializeField] private float bottomFontSize = 36f;
    [SerializeField] private float gameOverFontSize = 64f;
    [SerializeField] private float highscoreFontSize = 32f;
    [SerializeField] private float mainMenuFontSize = 75f;
    [SerializeField] private float gameOverOffscreenPadding = 80f;
    [SerializeField] private int canvasSortingOrder = 1000;
    [SerializeField] private Vector2 referenceResolution = new Vector2(1920f, 1080f);
    [SerializeField, Range(0f, 1f)] private float matchWidthOrHeight = 1f;

    [Header("Behavior")]
    [SerializeField] private bool spawnOnAwake = true;
    [SerializeField] private bool preventDuplicates = true;
    [SerializeField] private bool showLavaSpeed = true;

    void Awake()
    {
        if (spawnOnAwake)
            Spawn();
    }

    public void Spawn()
    {
        if (preventDuplicates && FindFirstObjectByType<EliasMStatsHUD>() != null)
            return;

        var canvasGo = new GameObject("StatsHUD_Canvas");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = canvasSortingOrder;

        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = referenceResolution;
        scaler.matchWidthOrHeight = matchWidthOrHeight;
        canvasGo.AddComponent<GraphicRaycaster>();

        var textGo = new GameObject("StatsHUD_Text");
        textGo.transform.SetParent(canvasGo.transform, false);

        var rect = textGo.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.sizeDelta = topRightSize;
        rect.anchoredPosition = new Vector2(-topRightPadding.x, -topRightPadding.y);

        var tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = topRightFontSize;
        tmp.alignment = TextAlignmentOptions.TopRight;
        tmp.textWrappingMode = TextWrappingModes.NoWrap;
        tmp.text = "Height: 0 m\nJump force gained: 0";
        tmp.color = Color.white;
        tmp.enabled = true;

        var lavaTextGo = new GameObject("StatsHUD_LavaText");
        lavaTextGo.transform.SetParent(canvasGo.transform, false);

        var lavaRect = lavaTextGo.AddComponent<RectTransform>();
        lavaRect.anchorMin = new Vector2(0.5f, 0f);
        lavaRect.anchorMax = new Vector2(0.5f, 0f);
        lavaRect.pivot = new Vector2(0.5f, 0f);
        lavaRect.sizeDelta = bottomSize;
        lavaRect.anchoredPosition = new Vector2(bottomPadding.x, bottomPadding.y);

        var lavaTmp = lavaTextGo.AddComponent<TextMeshProUGUI>();
        lavaTmp.fontSize = bottomFontSize;
        lavaTmp.alignment = TextAlignmentOptions.Bottom;
        lavaTmp.textWrappingMode = TextWrappingModes.NoWrap;
        lavaTmp.text = string.Empty;
        lavaTmp.enabled = false;

        var gameOverGo = new GameObject("StatsHUD_GameOverText");
        gameOverGo.transform.SetParent(canvasGo.transform, false);

        var gameOverRect = gameOverGo.AddComponent<RectTransform>();
        gameOverRect.anchorMin = new Vector2(0.5f, 0.5f);
        gameOverRect.anchorMax = new Vector2(0.5f, 0.5f);
        gameOverRect.pivot = new Vector2(0.5f, 0.5f);
        gameOverRect.sizeDelta = new Vector2(700f, 120f);
        gameOverRect.anchoredPosition = Vector2.zero;

        var gameOverTmp = gameOverGo.AddComponent<TextMeshProUGUI>();
        gameOverTmp.fontSize = gameOverFontSize;
        gameOverTmp.alignment = TextAlignmentOptions.Center;
        gameOverTmp.textWrappingMode = TextWrappingModes.NoWrap;
        gameOverTmp.text = "Game Over";
        gameOverTmp.enabled = false;

        var highscoreGo = new GameObject("StatsHUD_HighscoreText");
        highscoreGo.transform.SetParent(canvasGo.transform, false);

        var highscoreRect = highscoreGo.AddComponent<RectTransform>();
        highscoreRect.anchorMin = new Vector2(0.5f, 0.5f);
        highscoreRect.anchorMax = new Vector2(0.5f, 0.5f);
        highscoreRect.pivot = new Vector2(0.5f, 0.5f);
        highscoreRect.sizeDelta = new Vector2(520f, 80f);
        highscoreRect.anchoredPosition = new Vector2(0f, -70f);

        var highscoreTmp = highscoreGo.AddComponent<TextMeshProUGUI>();
        highscoreTmp.fontSize = highscoreFontSize;
        highscoreTmp.alignment = TextAlignmentOptions.Center;
        highscoreTmp.textWrappingMode = TextWrappingModes.NoWrap;
        highscoreTmp.text = "Highscore: 0 m";
        highscoreTmp.enabled = false;

        var buttonGo = new GameObject("StatsHUD_MainMenuButton");
        buttonGo.transform.SetParent(canvasGo.transform, false);

        var buttonRect = buttonGo.AddComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.sizeDelta = new Vector2(400f, 75f);
        buttonRect.anchoredPosition = new Vector2(0f, -140f);

        var buttonImage = buttonGo.AddComponent<Image>();
        buttonImage.color = Color.white;
        buttonImage.sprite = null;
        buttonImage.type = Image.Type.Simple;

        var button = buttonGo.AddComponent<Button>();
        button.transition = Selectable.Transition.ColorTint;
        button.colors = new ColorBlock
        {
            normalColor = new Color(1f, 1f, 1f, 1f),
            highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f),
            pressedColor = new Color(0.78431374f, 0.78431374f, 0.78431374f, 1f),
            selectedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f),
            disabledColor = new Color(0.78431374f, 0.78431374f, 0.78431374f, 0.5019608f),
            colorMultiplier = 1f,
            fadeDuration = 0.1f
        };
        button.targetGraphic = buttonImage;

        var buttonTextGo = new GameObject("Text");
        buttonTextGo.transform.SetParent(buttonGo.transform, false);

        var buttonTextRect = buttonTextGo.AddComponent<RectTransform>();
        buttonTextRect.anchorMin = Vector2.zero;
        buttonTextRect.anchorMax = Vector2.one;
        buttonTextRect.offsetMin = Vector2.zero;
        buttonTextRect.offsetMax = Vector2.zero;

        var buttonText = buttonTextGo.AddComponent<TextMeshProUGUI>();
        buttonText.fontSize = mainMenuFontSize;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.textWrappingMode = TextWrappingModes.NoWrap;
        buttonText.text = "Main Menu";
        buttonText.color = new Color(0.19607843f, 0.19607843f, 0.19607843f, 1f);

        var menuFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        if (menuFont != null)
            buttonText.font = menuFont;

        var retryButtonGo = new GameObject("StatsHUD_RetryButton");
        retryButtonGo.transform.SetParent(canvasGo.transform, false);

        var retryRect = retryButtonGo.AddComponent<RectTransform>();
        retryRect.anchorMin = new Vector2(0.5f, 0.5f);
        retryRect.anchorMax = new Vector2(0.5f, 0.5f);
        retryRect.pivot = new Vector2(0.5f, 0.5f);
        retryRect.sizeDelta = new Vector2(400f, 75f);
        retryRect.anchoredPosition = new Vector2(0f, -225f);

        var retryImage = retryButtonGo.AddComponent<Image>();
        retryImage.color = Color.white;
        retryImage.sprite = null;
        retryImage.type = Image.Type.Simple;

        var retryButton = retryButtonGo.AddComponent<Button>();
        retryButton.transition = Selectable.Transition.ColorTint;
        retryButton.colors = new ColorBlock
        {
            normalColor = new Color(1f, 1f, 1f, 1f),
            highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f),
            pressedColor = new Color(0.78431374f, 0.78431374f, 0.78431374f, 1f),
            selectedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f),
            disabledColor = new Color(0.78431374f, 0.78431374f, 0.78431374f, 0.5019608f),
            colorMultiplier = 1f,
            fadeDuration = 0.1f
        };
        retryButton.targetGraphic = retryImage;

        var retryTextGo = new GameObject("Text");
        retryTextGo.transform.SetParent(retryButtonGo.transform, false);

        var retryTextRect = retryTextGo.AddComponent<RectTransform>();
        retryTextRect.anchorMin = Vector2.zero;
        retryTextRect.anchorMax = Vector2.one;
        retryTextRect.offsetMin = Vector2.zero;
        retryTextRect.offsetMax = Vector2.zero;

        var retryText = retryTextGo.AddComponent<TextMeshProUGUI>();
        retryText.fontSize = mainMenuFontSize;
        retryText.alignment = TextAlignmentOptions.Center;
        retryText.textWrappingMode = TextWrappingModes.NoWrap;
        retryText.text = "Retry";
        retryText.color = new Color(0.19607843f, 0.19607843f, 0.19607843f, 1f);
        if (menuFont != null)
            retryText.font = menuFont;

        var gameOverSlide = gameOverGo.AddComponent<GameOverTextSlide>();
        gameOverSlide.Initialize(FindFirstObjectByType<RisingLava>(), gameOverOffscreenPadding, highscoreTmp, button, retryButton);

        var hud = textGo.AddComponent<EliasMStatsHUD>();
        hud.Initialize(tmp, FindFirstObjectByType<Movements>(), FindFirstObjectByType<RisingLava>());
        hud.InitializeLavaText(lavaTmp);
        hud.SetShowLavaSpeed(showLavaSpeed);
    }
}
