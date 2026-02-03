using UnityEngine;

public class RisingLava : MonoBehaviour
{
    public float riseSpeed = 2f;

    void Update()
    {
        transform.Translate(Vector2.up * riseSpeed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Platform"))
        {
            Destroy(other.gameObject);
        }
    }
}


