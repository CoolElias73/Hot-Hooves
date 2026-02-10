using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("Spawn")]
    [Min(0f)] public float spawnWeight = 1f;

    public float speedIncrease = 1f;
    public float jumpIncrease = 2f;

    public AudioClip pickupSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Movements player = other.GetComponent<Movements>();
            if (player != null)
            {
                player.IncreaseStats(speedIncrease, jumpIncrease);
            }

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, 3f);
            }

            Destroy(gameObject);
        }
    }
}

