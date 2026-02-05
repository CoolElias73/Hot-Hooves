using UnityEngine;
using System;

public class RisingLava : MonoBehaviour
{
    public float riseSpeed = 4f;
    public float speedIncreaseEverySeconds = 20f;
    public float speedIncreaseAmount = 2f;
    public bool stopAtCameraTop = true;

    float _currentSpeed;
    float _nextIncreaseTime;

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

        if (Time.timeSinceLevelLoad >= _nextIncreaseTime)
        {
            _currentSpeed += speedIncreaseAmount;
            _nextIncreaseTime = Time.timeSinceLevelLoad + speedIncreaseEverySeconds;
        }
        transform.Translate(Vector2.up * _currentSpeed * Time.deltaTime);
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




