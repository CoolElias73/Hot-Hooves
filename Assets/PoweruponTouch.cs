using UnityEngine;

public class Powerup : MonoBehaviour
{
    public float speedIncrease = 1f;
    public float jumpIncrease = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Movements player = other.GetComponent<Movements>();
            if (player != null)
            {
                player.IncreaseStats(speedIncrease, jumpIncrease);
            }

            Destroy(gameObject);
        }
    }
}

