using UnityEngine;

public class RisingLava : MonoBehaviour
{
    public float riseSpeed = 2f;
    public float speedUpAfterSeconds = 30f;
    public float boostedRiseSpeed = 4f;

    void Update()
    {
        float currentSpeed = Time.timeSinceLevelLoad >= speedUpAfterSeconds
            ? boostedRiseSpeed
            : riseSpeed;

        transform.Translate(Vector2.up * currentSpeed * Time.deltaTime);
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




