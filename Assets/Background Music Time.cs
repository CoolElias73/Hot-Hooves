using UnityEngine;

public class BackgroundMusicTime : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private RisingLava lava;
    [SerializeField] private bool syncToLavaSpeed = true;
    [SerializeField] private float minLavaSpeed = 2f;
    [SerializeField] private float maxLavaSpeed = 12f;
    [SerializeField] private float minPitch = 0.85f;
    [SerializeField] private float maxPitch = 1.25f;
    [SerializeField] private float pitchLerpSpeed = 4f;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (lava == null)
            lava = FindFirstObjectByType<RisingLava>();
    }

    void Update()
    {
        if (!syncToLavaSpeed || audioSource == null || lava == null)
            return;

        float speed = lava.CurrentSpeed;
        float t = Mathf.InverseLerp(minLavaSpeed, maxLavaSpeed, speed);
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, t);
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, pitchLerpSpeed * Time.deltaTime);
    }
}
