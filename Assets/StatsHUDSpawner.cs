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
    [SerializeField] private int canvasSortingOrder = 1000;

    [Header("Behavior")]
    [SerializeField] private bool spawnOnAwake = true;
    [SerializeField] private bool preventDuplicates = true;

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
        canvas.sortingOrder = canvasSortingOrder;

        canvasGo.AddComponent<CanvasScaler>();
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

        var hud = textGo.AddComponent<EliasMStatsHUD>();
        hud.Initialize(tmp, FindFirstObjectByType<Movements>(), FindFirstObjectByType<RisingLava>());
        hud.InitializeLavaText(lavaTmp);
    }
}
