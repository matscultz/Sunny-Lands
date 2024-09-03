using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Octopus_projectile : MonoBehaviour
{
    public float speed = 5f;
    public GameObject explosionPrefab;
    public int damage = 10;

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Applicare danno al giocatore
            Player_Health playerHealth = collision.gameObject.GetComponent<Player_Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                // Applicare knockback
                Vector2 knockbackDirection = collision.transform.position - transform.position;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection.normalized * 5f, ForceMode2D.Impulse);
            }
        }

        // Creare esplosione
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject); // Distruggere il proiettile
    }
}
