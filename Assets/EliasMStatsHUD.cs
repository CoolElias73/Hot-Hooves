using UnityEngine;
using TMPro;

public class EliasMStatsHUD : MonoBehaviour
{
    [SerializeField] private Movements player;
    [SerializeField] private RisingLava lava;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text lavaText;
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] private bool heightFromStart = true;

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

        if (player == null)
            ResolveReferences();

        float heightValue = player != null ? player.transform.position.y : 0f;
        if (heightFromStart)
            heightValue = Mathf.Max(0f, heightValue - _startY);

        float jumpForceValue = player != null ? player.jumpForceAdded : 0f;

        text.text =
            $"Height: {heightValue:0.0} m\n" +
            $"Jump force gained: {jumpForceValue:0.0}";

        UpdateLavaText();
    }

    private void ResolveReferences()
    {
        if (player == null)
            player = FindFirstObjectByType<Movements>();
        if (lava == null)
            lava = FindFirstObjectByType<RisingLava>();

        if (player != null)
        {
            _startY = player.transform.position.y;
        }
    }

    public void Initialize(TMP_Text tmpText, Movements playerRef, RisingLava lavaRef)
    {
        text = tmpText;
        player = playerRef;
        lava = lavaRef;
        ResolveReferences();
    }

    public void InitializeLavaText(TMP_Text tmpText)
    {
        lavaText = tmpText;
        if (lavaText != null)
            lavaText.text = string.Empty;
    }

    public void HideMainStats()
    {
        if (text != null)
            text.enabled = false;
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

}
