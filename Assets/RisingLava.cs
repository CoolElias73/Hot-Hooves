using UnityEngine;
using System;

public class RisingLava : MonoBehaviour
{
    public float riseSpeed = 4f;
    public float speedIncreaseEverySeconds = 20f;
    public float speedIncreaseAmount = 2f;
    public bool stopAtCameraTop = true;

    [Header("In-View Slowdown")]
    public bool slowDownWhenInView = true;
    public float viewResetSpeed = 2.5f;
    public float viewResetStep = 0.25f;
    public float viewResetMaxSpeed = 6f;
    public float viewResetCooldown = 0.5f;
    public float viewResetTriggerOffset = 1.5f;

    float _currentSpeed;
    float _nextIncreaseTime;
    float _nextViewResetTime;
    bool _wasInView;

    public event Action<Transform, float> PlayerTouched;

    public float CurrentSpeed => _currentSpeed;

    void Start()
    {
        _currentSpeed = riseSpeed;
        _nextIncreaseTime = Time.timeSinceLevelLoad + speedIncreaseEverySeconds;
    }

    void Update()
    {
        if (stopAtCameraTop && IsAtOrAboveCameraTop())
            return;

        bool inView = IsInCameraView();
        if (slowDownWhenInView)
        {
            if (inView && !_wasInView && Time.time >= _nextViewResetTime)
            {
                _currentSpeed = viewResetSpeed;
                viewResetSpeed = Mathf.Min(viewResetMaxSpeed, viewResetSpeed + viewResetStep);
                _nextIncreaseTime = Time.timeSinceLevelLoad + speedIncreaseEverySeconds;
                _nextViewResetTime = Time.time + viewResetCooldown;
            }
        }

        if (Time.timeSinceLevelLoad >= _nextIncreaseTime)
        {
            _currentSpeed += speedIncreaseAmount;
            _nextIncreaseTime = Time.timeSinceLevelLoad + speedIncreaseEverySeconds;
        }
        transform.Translate(Vector2.up * _currentSpeed * Time.deltaTime);

        _wasInView = inView;
    }

    private bool IsAtOrAboveCameraTop()
    {
        var cam = Camera.main;
        if (cam == null)
            return false;

        float camTopY = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
        float lavaTopY = transform.position.y;
        var lavaRenderer = GetComponent<Renderer>();
        if (lavaRenderer != null)
            lavaTopY = lavaRenderer.bounds.max.y;

        return lavaTopY >= camTopY;
    }

    private bool IsInCameraView()
    {
        var cam = Camera.main;
        if (cam == null)
            return false;

        float camBottomY = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y + viewResetTriggerOffset;
        float lavaTopY = transform.position.y;
        var lavaRenderer = GetComponent<Renderer>();
        if (lavaRenderer != null)
            lavaTopY = lavaRenderer.bounds.max.y;

        return lavaTopY >= camBottomY;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag("Player"))
        {
            PlayerTouched?.Invoke(other.transform, _currentSpeed);
            Destroy(other.gameObject);
        }

       
        if (other.CompareTag("Platform"))
        {
            Destroy(other.gameObject);
        }
    }
}




