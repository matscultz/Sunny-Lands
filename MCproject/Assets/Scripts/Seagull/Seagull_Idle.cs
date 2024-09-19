using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull_Idle : MonoBehaviour
{
    public string[] animationTriggers; // Lista dei nomi dei trigger per le animazioni
    public float minDelay = 1f; // Delay minimo tra le animazioni
    public float maxDelay = 3f; // Delay massimo tra le animazioni
    private Animator animator; // Riferimento all'Animator

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayRandomAnimations());
    }

    IEnumerator PlayRandomAnimations()
    {
        while (true)
        {
            // Scegli un'animazione a caso
            int randomIndex = Random.Range(0, animationTriggers.Length);
            string randomTrigger = animationTriggers[randomIndex];

            // Attiva il trigger per avviare l'animazione
            animator.SetTrigger(randomTrigger);

            // Attendi per un intervallo di tempo casuale tra minDelay e maxDelay
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }
}
