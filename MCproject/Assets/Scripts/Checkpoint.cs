using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActive = false;  // Flag per sapere se il checkpoint è già stato attivato
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActive)
        {
            // Attiva il checkpoint solo se non è già attivo
            isActive = true;

            // Comunica al GameManager di aggiornare il checkpoint
            GameManager.Instance.SetCheckpoint(transform.position);
            animator.SetTrigger("Activated");
            // Puoi anche aggiungere un feedback visivo o sonoro per indicare che il checkpoint è attivo
            // ad esempio cambiando il colore o mostrando un'animazione.
        }
    }
}
