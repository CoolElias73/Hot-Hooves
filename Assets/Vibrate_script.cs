using UnityEngine;
using System.Collections;

public class ImpactShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeStrength = 0.1f;

    Vector3 originalPos;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float t = 0f;

        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            Vector3 offset = Random.insideUnitSphere * shakeStrength;
            transform.localPosition = originalPos + new Vector3(offset.x, offset.y, 0);
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
