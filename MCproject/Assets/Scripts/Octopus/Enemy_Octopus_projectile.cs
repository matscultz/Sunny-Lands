using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Octopus_projectile : MonoBehaviour
{
    public float speed = 5f;
    public GameObject explosionPrefab;
    public int damage = 10;
    public float knockback = 5f;
    public LayerMask playerLayer;
    public LayerMask otherLayers;
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        Debug.Log("Collision layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        int collisionLayer = collision.gameObject.layer;
        // Verifica il layer dell'oggetto con cui il proiettile collidere
        if ((playerLayer == (playerLayer | (1 << collisionLayer))))
        {
            // Danno al player
            collision.gameObject.GetComponent<Player_Health>().TakeDamage(damage);
            //Knockback per il colpo
            Vector2 knockbackDirection = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection.normalized * knockback, ForceMode2D.Impulse);

            // Esegui esplosione
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject); // Distruggi il proiettile
        }
        else if (otherLayers == (otherLayers | (1 << collisionLayer)))
        {
            // Esplosione senza danno
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject); // Distruggi il proiettile
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
