using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EliasMStatsHUD : MonoBehaviour
{
    [SerializeField] private Movements player;
    [SerializeField] private RisingLava lava;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text lavaText;
    [SerializeField] private Vector2 padding = new Vector2(20f, 20f);
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] private bool heightFromStart = true;

    private Rigidbody2D _playerRb;
    private float _startY;
    private float _nextUpdate;

    void Awake()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        ResolveReferences();
    }

    void Update()
    {
        if (Time.time < _nextUpdate)
            return;

        _nextUpdate = Time.time + Mathf.Max(0.02f, updateInterval);

        if (text == null)
            return;

        if (player == null || _playerRb == null)
            ResolveReferences();

        float heightValue = player != null ? player.transform.position.y : 0f;
        if (heightFromStart)
            heightValue -= _startY;

        float jumpForceValue = player != null ? player.jumpForceAdded : 0f;

        text.text =
            $"Height: {heightValue:0.0} m\n" +
            $"Jump force gained: {jumpForceValue:0.0}";

        UpdateLavaText();
    }

    private void ResolveReferences()
    {
        if (player == null)
            player = FindObjectOfType<Movements>();
        if (lava == null)
            lava = FindObjectOfType<RisingLava>();

        if (player != null)
        {
            _playerRb = player.GetComponent<Rigidbody2D>();
            _startY = player.transform.position.y;
        }
    }

    public void Initialize(TMP_Text tmpText, Movements playerRef, RisingLava lavaRef, Vector2 paddingValue)
    {
        text = tmpText;
        player = playerRef;
        lava = lavaRef;
        padding = paddingValue;
        ResolveReferences();
    }

    public void InitializeLavaText(TMP_Text tmpText)
    {
        lavaText = tmpText;
        if (lavaText != null)
            lavaText.text = string.Empty;
    }

    private void UpdateLavaText()
    {
        if (lavaText == null || lava == null)
            return;

        var cam = Camera.main;
        if (cam == null)
        {
            lavaText.enabled = false;
            return;
        }

        float bottomY = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
        float lavaTopY = lava.transform.position.y;
        var lavaRenderer = lava.GetComponent<Renderer>();
        if (lavaRenderer != null)
            lavaTopY = lavaRenderer.bounds.max.y;

        float distanceFromBottom = bottomY - lavaTopY;

        if (distanceFromBottom <= 0f)
        {
            lavaText.enabled = false;
            return;
        }

        lavaText.enabled = true;
        lavaText.text = $"Lava distance: {distanceFromBottom:0.0} m";
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AutoCreateInEliasM()
    {
        if (SceneManager.GetActiveScene().name != "EliasM")
            return;

        if (FindObjectOfType<EliasMStatsHUD>() != null)
            return;

        var canvasGo = new GameObject("EliasM_StatsCanvas");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        canvasGo.AddComponent<CanvasScaler>();
        canvasGo.AddComponent<GraphicRaycaster>();

        var textGo = new GameObject("EliasM_StatsText");
        textGo.transform.SetParent(canvasGo.transform, false);

        var rect = textGo.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.sizeDelta = new Vector2(360f, 120f);
        rect.anchoredPosition = new Vector2(-20f, -20f);

        var tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = 24f;
        tmp.alignment = TextAlignmentOptions.TopRight;
        tmp.enableWordWrapping = false;
        tmp.text = "Height: 0\nJump force gained: 0";

        var lavaTextGo = new GameObject("EliasM_LavaText");
        lavaTextGo.transform.SetParent(canvasGo.transform, false);

        var lavaRect = lavaTextGo.AddComponent<RectTransform>();
        lavaRect.anchorMin = new Vector2(0.5f, 0f);
        lavaRect.anchorMax = new Vector2(0.5f, 0f);
        lavaRect.pivot = new Vector2(0.5f, 0f);
        lavaRect.sizeDelta = new Vector2(520f, 90f);
        lavaRect.anchoredPosition = new Vector2(0f, 20f);

        var lavaTmp = lavaTextGo.AddComponent<TextMeshProUGUI>();
        lavaTmp.fontSize = 36f;
        lavaTmp.alignment = TextAlignmentOptions.Bottom;
        lavaTmp.enableWordWrapping = false;
        lavaTmp.text = string.Empty;
        lavaTmp.enabled = false;

        var hud = textGo.AddComponent<EliasMStatsHUD>();
        hud.Initialize(tmp, FindObjectOfType<Movements>(), FindObjectOfType<RisingLava>(), new Vector2(20f, 20f));
        hud.InitializeLavaText(lavaTmp);
    }
}
