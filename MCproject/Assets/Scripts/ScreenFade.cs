using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Riferimento al CanvasGroup
    public float fadeDuration = 1.0f; // Durata del fade in/fade out

    // Coroutine per fare il fade-in
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
    }

    // Coroutine per fare il fade-out
    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
    }
}
