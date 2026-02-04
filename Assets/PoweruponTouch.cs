using UnityEngine;

public class Powerup : MonoBehaviour
{
    public float speedIncrease = 1f;
    public float jumpIncrease = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SimpleMovement player = other.GetComponent<SimpleMovement>();
            if (player != null)
            {
                player.IncreaseStats(speedIncrease, jumpIncrease);
            }

            Destroy(gameObject);
        }
    }
}

