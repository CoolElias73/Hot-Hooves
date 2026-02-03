using UnityEngine;

public class BackgroundMusicTime : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float timeBeforeSpeedUp = 30f;
    [SerializeField] private float pitchIncreaseAmount = 0.05f;
    [SerializeField] private float increaseEverySeconds = 10f;
    [SerializeField] private float speedUpDuration = 1f;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
            StartCoroutine(SpeedUpAfterDelay());
    }

    private System.Collections.IEnumerator SpeedUpAfterDelay()
    {
        if (timeBeforeSpeedUp > 0f)
            yield return new WaitForSeconds(timeBeforeSpeedUp);

        while (true)
        {
            float startPitch = audioSource.pitch;
            float targetPitch = startPitch + pitchIncreaseAmount;

            if (speedUpDuration <= 0f)
            {
                audioSource.pitch = targetPitch;
            }
            else
            {
                float elapsed = 0f;
                while (elapsed < speedUpDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / speedUpDuration);
                    audioSource.pitch = Mathf.Lerp(startPitch, targetPitch, t);
                    yield return null;
                }

                audioSource.pitch = targetPitch;
            }

            if (increaseEverySeconds > 0f)
                yield return new WaitForSeconds(increaseEverySeconds);
            else
                yield return null;
        }
    }
}
