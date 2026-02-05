using UnityEngine;

public class RisingLava : MonoBehaviour
{
    public float riseSpeed = 4f;
    public float speedIncreaseEverySeconds = 20f;
    public float speedIncreaseAmount = 2f;

    float _currentSpeed;
    float _nextIncreaseTime;

    void Start()
    {
        _currentSpeed = riseSpeed;
        _nextIncreaseTime = Time.timeSinceLevelLoad + speedIncreaseEverySeconds;
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad >= _nextIncreaseTime)
        {
            _currentSpeed += speedIncreaseAmount;
            _nextIncreaseTime = Time.timeSinceLevelLoad + speedIncreaseEverySeconds;
        }
        transform.Translate(Vector2.up * _currentSpeed * Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }

       
        if (other.CompareTag("Platform"))
        {
            Destroy(other.gameObject);
        }
    }
}




