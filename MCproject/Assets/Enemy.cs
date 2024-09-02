using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public float deadAnimation = 1f;
    public int maxHealth = 1;
    private int currentHealth;
    private bool isDead = false;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>(); // Ottieni il Rigidbody2D
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        //animator.SetTrigger("Hurt");
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Assicurati che Die() venga eseguito solo una volta

        Debug.Log("Enemy died :(");
        animator.SetBool("isDead", true);
        isDead = true; // Imposta isDead su true per prevenire ulteriori riproduzioni
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        StartCoroutine(DisableAfterDeathAnimation());
    }

    IEnumerator DisableAfterDeathAnimation()
    {
        // Supponiamo che l'animazione di morte duri 1 secondo. Modifica il valore se necessario.
        yield return new WaitForSeconds(deadAnimation);
        // Disabilita il GameObject dopo la durata dell'animazione
        gameObject.SetActive(false);
    }
}
