using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Octopus_projectile_sx : MonoBehaviour
{
    public float speed = 5f;
    public GameObject explosionPrefab;
    public int damage = 10;
    public float knockback = 5f;
    public LayerMask playerLayer;
    public LayerMask otherLayers;
    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Colpito l'oggetto con layer: {LayerMask.LayerToName(collision.gameObject.layer)}");
        int collisionLayer = collision.gameObject.layer;
        if ((playerLayer == (playerLayer | (1 << collisionLayer))))
        {
            // Applicare danno al giocatore
            Player_Health playerHealth = collision.gameObject.GetComponent<Player_Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                // Applicare knockback
                Vector2 knockbackDirection = collision.transform.position - transform.position;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection.normalized * knockback, ForceMode2D.Impulse);
            }
            Explode();
        }
        else if (otherLayers == (otherLayers | (1 << collisionLayer)))
        {
            // Esplosione senza danno
            Explode();
        }

    }
    void Explode()
    {
        // Mostra l'effetto dell'esplosione
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        // Distrugge il proiettile
        Destroy(gameObject);
        GetComponent<Collider2D>().enabled = false;
    }
}
