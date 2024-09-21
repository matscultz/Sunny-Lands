using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public TextMesh textMesh;  // Riferimento al TextMeshPro sul checkpoint
    public float displayDuration = 2.0f;  // Tempo in cui la scritta rimane visibile
    public float fadeDuration = 0.5f;     // Durata del fade in/fade out

    private bool isActive = false;  // Stato del checkpoint

    private void Start()
    {
        // Assicurati che la scritta sia invisibile all'inizio
        if (textMesh != null)
        {
            SetTextAlpha(0);  // Imposta l'alpha del testo a 0 per renderlo invisibile
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se il giocatore è entrato nel checkpoint
        if (other.CompareTag("Player") && !isActive)
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        isActive = true;
        GameManager.Instance.SetCheckpoint(transform.position);
        // Mostra il testo con il fade in
        if (textMesh != null)
        {
            SoundManager.Instance.PlaySound2D("Checkpoint");
            StartCoroutine(FadeTextIn());

            // Attendi il tempo della durata del display, poi inizia il fade out
            Invoke("StartFadeOut", displayDuration);
        }
    }

    private void StartFadeOut()
    {
        // Nascondi il testo con il fade out
        StartCoroutine(FadeTextOut());
    }

    // Coroutine per gestire il fade in del testo
    private IEnumerator FadeTextIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            SetTextAlpha(Mathf.Lerp(0, 1, elapsedTime / fadeDuration));  // Aumenta gradualmente l'alpha
            yield return null;
        }
        SetTextAlpha(1);  // Assicurati che l'alpha sia pienamente visibile alla fine
    }

    // Coroutine per gestire il fade out del testo
    private IEnumerator FadeTextOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            SetTextAlpha(Mathf.Lerp(1, 0, elapsedTime / fadeDuration));  // Riduci gradualmente l'alpha
            yield return null;
        }
        SetTextAlpha(0);  // Assicurati che l'alpha sia completamente trasparente alla fine
    }

    // Metodo per impostare l'alpha del testo
    private void SetTextAlpha(float alpha)
    {
        if (textMesh != null)
        {
            Color color = textMesh.color;
            color.a = alpha;
            textMesh.color = color;
        }
    }
}
