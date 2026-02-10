using UnityEngine;

public class PassThroughPlatformsPowerup : MonoBehaviour
{
    [Header("Spawn")]
    [Min(0f)] public float spawnWeight = 1f;

    [Min(0f)]
    public float effectDuration = 6f;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        var effect = other.GetComponent<PassThroughPlatformsEffect>();
        if (effect == null)
            effect = other.gameObject.AddComponent<PassThroughPlatformsEffect>();

        effect.Activate(effectDuration);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 3f);

        Destroy(gameObject);
    }
}
