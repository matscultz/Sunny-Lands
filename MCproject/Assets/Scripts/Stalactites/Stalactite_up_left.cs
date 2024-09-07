using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite_up_left : MonoBehaviour
{
    public LayerMask playerLayer;
    public int damage = 3;
    public Animator animator;
    public float groundedAnimation = 0.5f;
    public float shakeAnimation = 0.5f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        

    }


    private void Fall()
    {
        animator.SetBool("isFall", true);
    }

   private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered with: " + collision.gameObject.name);
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isDetected", true);
            StartCoroutine(FallAfterShakeAnimation());
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Avvia l'animazione di danno e infliggi danno al player
            animator.SetTrigger("Hit");
            Player_Health playerHealth = collision.gameObject.GetComponent<Player_Health>();

            // Se lo script PlayerHealth esiste, infligge il danno
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // Avvia solo l'animazione della caduta
            animator.SetTrigger("Hit");
        }

        StartCoroutine(DisableAfterGroundedAnimation());

    }


    IEnumerator DisableAfterGroundedAnimation()
    {
        // Aspetta la durata dell'animazione di morte
        yield return new WaitForSeconds(groundedAnimation);

        // Disabilita il GameObject dopo l'animazione
        gameObject.SetActive(false);
    }
    IEnumerator FallAfterShakeAnimation()
    {
        // Aspetta la durata dell'animazione di shake
        yield return new WaitForSeconds(shakeAnimation);

        Fall();
        rb.isKinematic = false;


    }
}
