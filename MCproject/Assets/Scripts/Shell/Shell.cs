using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float bounceForce = 5f;
    public int damageAmount = 10;  // Danno inflitto al player
    public Vector2 biteOffset;      // Offset dal nemico per il punto di morso
    public float biteRadius = 0.5f; // Raggio di attacco del morso
    public float fireRate = 2f;
    private float fireCooldown;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        fireCooldown -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Controlla se l'oggetto che collide è il Player
        if (collision.gameObject.tag == "Player")
        {
            // Controlla tutti i punti di contatto
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Se la normale del contatto è verso l'alto (cioè il Player è sopra)
                if (contact.normal == Vector2.down)
                {
                    // Applica una forza verso l'alto al Player per simulare il rimbalzo
                    Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        BouncePlayer(playerRb);
                    }
                }
            }
        }
    }

    private void BouncePlayer(Rigidbody2D playerRb)
    {
        SoundManager.Instance.PlaySound2D("Bounce Barrel");
        animator.SetTrigger("isBounce");
        // Applica la forza verso l'alto per simulare il rimbalzo
        playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (fireCooldown <= 0f)
            {
                animator.SetTrigger("isAttack");
                DealDamage(collision);
                fireCooldown = fireRate;
            }
        }
    }

    public void DealDamage(Collider2D player)
    {
        // Calcola il punto di morso in base alla posizione del nemico e l'offset
        Vector2 bitePoint = (Vector2)transform.position + biteOffset;

        // Controlla se il player è sufficientemente vicino
        if (player != null && Vector2.Distance(bitePoint, player.transform.position) <= biteRadius)
        {
            Player_Health playerHealth = player.transform.GetComponent<Player_Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    // Mostra il punto di morso nella scena con un gizmo
    private void OnDrawGizmosSelected()
    {
        // Imposta il colore del gizmo
        Gizmos.color = Color.red;

        // Calcola il punto di morso in base alla posizione del nemico e l'offset
        Vector2 bitePoint = (Vector2)transform.position + biteOffset;

        // Disegna un piccolo cerchio rosso che rappresenta il punto di morso
        Gizmos.DrawWireSphere(bitePoint, biteRadius);
    }
}
