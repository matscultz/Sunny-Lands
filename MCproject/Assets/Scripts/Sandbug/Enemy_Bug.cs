using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public Animator animator;
    public float deadAnimation = 1f;
    public int maxHealth = 1;
    private int currentHealth;
    private Rigidbody2D rb;
    private Patrolling patrolling;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>(); // Ottieni il Rigidbody2D
        Patrolling movementScript = GetComponent<Patrolling>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (patrolling != null)
        {
            animator.SetBool("isRun", true);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Se il nemico subisce danno e la salute scende a 0 o meno, muore
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died :(");
        animator.SetBool("isDead", true); // Attiva l'animazione di morte
        patrolling = GetComponent<Patrolling>();
        if (patrolling != null)
        {
            patrolling.enabled = false;
        }
        // Disabilita il collider per evitare ulteriori interazioni
        GetComponent<Collider2D>().enabled = false;

        // Blocca il movimento del personaggio
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        // Disabilita questo script per evitare ulteriori aggiornamenti
        this.enabled = false;

        // Avvia la coroutine per disabilitare il GameObject dopo l'animazione di morte
        StartCoroutine(DisableAfterDeathAnimation());
    }

    IEnumerator DisableAfterDeathAnimation()
    {
        // Aspetta la durata dell'animazione di morte
        yield return new WaitForSeconds(deadAnimation);

        // Disabilita il GameObject dopo l'animazione
        gameObject.SetActive(false);
    }
}
