using UnityEngine;
using System.Collections;

public class GameJuiceEffectScript : MonoBehaviour
{
    [Header("Shake Settings")]
    public bool shakeHorizontal = true;
    public bool shakeVertical = true;
    public float shakeIntensity = 0.2f;
    public float shakeDuration = 0.5f;
    public int shakeFrequency = 20;

    [Header("Scale / Stretch Settings")]
    public bool scaleEffect = true;
    public Vector3 scaleMultiplier = new Vector3(1.2f, 1.2f, 1f);
    public float scaleDuration = 0.2f;

    private Vector3 originalPosition;
    private Vector3 originalScale;

    void Awake()
    {
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Call this to trigger the full “juice” effect
    /// </summary>
    public void TriggerJuice()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeAndScale());
    }

    private IEnumerator ShakeAndScale()
    {
        if (scaleEffect)
        {
            Vector3 targetScale = Vector3.Scale(originalScale, scaleMultiplier);
            float timer = 0f;
            while (timer < scaleDuration)
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / scaleDuration);
                yield return null;
            }
            transform.localScale = originalScale;
        }

        float elapsed = 0f;
        float interval = 1f / shakeFrequency;
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            Vector3 offset = Vector3.zero;
            if (shakeHorizontal) offset.x = Random.Range(-shakeIntensity, shakeIntensity);
            if (shakeVertical) offset.y = Random.Range(-shakeIntensity, shakeIntensity);

            transform.localPosition = originalPosition + offset;
            yield return new WaitForSeconds(interval);
        }

        transform.localPosition = originalPosition;
        transform.localScale = originalScale;
    }

    /// <summary>
    /// Optional helper: just shake without scaling
    /// </summary>
    public void TriggerShakeOnly()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeOnly());
    }

    private IEnumerator ShakeOnly()
    {
        float elapsed = 0f;
        float interval = 1f / shakeFrequency;
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            Vector3 offset = Vector3.zero;
            if (shakeHorizontal) offset.x = Random.Range(-shakeIntensity, shakeIntensity);
            if (shakeVertical) offset.y = Random.Range(-shakeIntensity, shakeIntensity);

            transform.localPosition = originalPosition + offset;
            yield return new WaitForSeconds(interval);
        }

        transform.localPosition = originalPosition;
    }
}
