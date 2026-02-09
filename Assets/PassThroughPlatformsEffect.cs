using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatformsEffect : MonoBehaviour
{
    [Tooltip("How long the effect lasts after activation (seconds).")]
    public float duration = 6f;

    [Tooltip("Default duration used when activation duration is <= 0 (seconds).")]
    public float defaultDuration = 6f;

    [Tooltip("How close the player can be to the platform underside before ignoring collisions.")]
    public float enterBuffer = 0.05f;

    [Tooltip("How far the player must be above/below to re-enable collisions.")]
    public float exitBuffer = 0.05f;

    [Tooltip("Tags that should be treated as platforms.")]
    public string[] platformTags = { "Platform", "DestructiblePlatform" };

    [Tooltip("Layers that should be treated as platforms (defaults to Ground + Default if unset).")]
    public LayerMask platformLayers;

    private readonly HashSet<Collider2D> _ignored = new HashSet<Collider2D>();
    private Collider2D _playerCollider;
    private Rigidbody2D _rb;
    private float _endTime;
    private bool _active;

    public bool IsActive => _active;

    public float TimeRemaining
    {
        get
        {
            if (!_active)
                return 0f;
            return Mathf.Max(0f, _endTime - Time.time);
        }
    }

    void Awake()
    {
        _playerCollider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        if (platformLayers == 0)
            platformLayers = LayerMask.GetMask("Ground", "Default");
    }

    public void Activate(float newDuration)
    {
        if (newDuration <= 0f)
            newDuration = defaultDuration;

        if (newDuration <= 0f)
            return;

        duration = newDuration;
        _endTime = Time.time + duration;
        _active = true;
        enabled = true;
    }

    void FixedUpdate()
    {
        if (!_active)
            return;

        if (Time.time >= _endTime)
        {
            Deactivate();
            return;
        }

        RefreshIgnoredCollisions();
        IgnorePlatformsAboveWhenRising();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryIgnorePlatformCollision(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        TryIgnorePlatformCollision(collision);
    }

    private void TryIgnorePlatformCollision(Collision2D collision)
    {
        if (!_active || _rb == null || _playerCollider == null)
            return;

        if (!IsPlatform(collision.collider))
            return;

        if (_rb.linearVelocity.y <= 0f)
            return;

        bool platformAbove = _playerCollider.bounds.center.y <= collision.collider.bounds.center.y;
        bool hitFromBelow = false;
        ContactPoint2D[] contacts = collision.contacts;
        for (int i = 0; i < contacts.Length; i++)
        {
            if (contacts[i].normal.y < -0.1f)
            {
                hitFromBelow = true;
                break;
            }
        }

        if (platformAbove || hitFromBelow)
        {
            Physics2D.IgnoreCollision(_playerCollider, collision.collider, true);
            _ignored.Add(collision.collider);
        }
    }

    private void RefreshIgnoredCollisions()
    {
        if (_ignored.Count == 0 || _playerCollider == null)
            return;

        List<Collider2D> toRemove = null;
        foreach (var col in _ignored)
        {
            if (col == null)
            {
                if (toRemove == null) toRemove = new List<Collider2D>();
                toRemove.Add(col);
                continue;
            }

            if (IsPlayerClearlyAbove(col) || IsPlayerClearlyBelow(col))
            {
                Physics2D.IgnoreCollision(_playerCollider, col, false);
                if (toRemove == null) toRemove = new List<Collider2D>();
                toRemove.Add(col);
            }
        }

        if (toRemove == null)
            return;

        foreach (var col in toRemove)
            _ignored.Remove(col);
    }

    private bool IsPlayerClearlyAbove(Collider2D col)
    {
        return _playerCollider.bounds.min.y > col.bounds.max.y + exitBuffer;
    }

    private bool IsPlayerClearlyBelow(Collider2D col)
    {
        return _playerCollider.bounds.max.y < col.bounds.min.y - exitBuffer;
    }

    private bool IsPlatform(Collider2D col)
    {
        if (col == null)
            return false;

        int layerMask = 1 << col.gameObject.layer;
        if ((platformLayers.value & layerMask) != 0)
            return true;

        if (col.GetComponent<IceSurface>() != null)
            return true;

        if (platformTags == null || platformTags.Length == 0)
            return false;

        string tagValue = col.tag;
        for (int i = 0; i < platformTags.Length; i++)
        {
            if (tagValue == platformTags[i])
                return true;
        }
        return false;
    }

    private void IgnorePlatformsAboveWhenRising()
    {
        if (_rb == null || _playerCollider == null)
            return;

        if (_rb.linearVelocity.y <= 0f)
            return;

        Bounds bounds = _playerCollider.bounds;
        float boxHeight = Mathf.Max(enterBuffer * 2f, 0.02f);
        Vector2 center = new Vector2(bounds.center.x, bounds.max.y + enterBuffer);
        Vector2 size = new Vector2(bounds.size.x * 0.9f, boxHeight);

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, platformLayers.value);
        if (hits == null || hits.Length == 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D col = hits[i];
            if (col == null || col == _playerCollider)
                continue;

            if (!IsPlatform(col))
                continue;

            float platformBottom = col.bounds.min.y;
            if (bounds.max.y <= platformBottom + enterBuffer || bounds.center.y <= col.bounds.center.y)
            {
                Physics2D.IgnoreCollision(_playerCollider, col, true);
                _ignored.Add(col);
            }
        }
    }

    private void Deactivate()
    {
        _active = false;

        if (_playerCollider != null)
        {
            foreach (var col in _ignored)
            {
                if (col != null)
                    Physics2D.IgnoreCollision(_playerCollider, col, false);
            }
        }

        _ignored.Clear();
        enabled = false;
    }

    void OnDisable()
    {
        if (_active)
            Deactivate();
    }
}
