using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Bomb : MonoBehaviour
{
    public int damage = 3;
    public Animator animator;
    public float impactAnimation = 1f;
    public float allert1Animation = 1f;
    public float allert2Animation = 1f;
    public float allert3Animation = 1f;
    public float explosionAnimation = 1f;
    public float bounceForce = 5f;
    private CircleCollider2D explosionCollider;
    private BoxCollider2D boxCollider2D;
    private bool isHit;
    private bool isExplode;
    private Rigidbody2D rb;
    private bool hasBounced;  // Nuovo flag per il rimbalzo

    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        explosionCollider = GetComponent<CircleCollider2D>();
        explosionCollider.enabled = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        isExplode = false;
        hasBounced = false;  // Inizialmente il rimbalzo non è avvenuto
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            // Verifica se il giocatore sta cadendo sulla bomba (collisione dall'alto)
            if (collision.contacts[0].point.y > transform.position.y && !hasBounced && !isExplode)
            {
                BouncePlayer(playerRb);  // Applica il rimbalzo
                hasBounced = true;  // Segna che il rimbalzo è avvenuto
            }
        }
    }

    private void BouncePlayer(Rigidbody2D playerRb)
    {
        // Applica la forza verso l'alto per simulare il rimbalzo
        playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExplode)
        {
            if (collision.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Player_Health>().TakeDamage(damage);
            }
        }
    }

    private IEnumerator PlayExplosionSequence()
    {
        // Gestisce tutte le transizioni delle animazioni
        yield return new WaitForSeconds(impactAnimation);
        animator.SetBool("Allert1", true);

        yield return new WaitForSeconds(allert1Animation);
        animator.SetBool("Allert2", true);

        yield return new WaitForSeconds(allert2Animation);
        animator.SetBool("Allert3", true);

        yield return new WaitForSeconds(allert3Animation);
        animator.SetBool("isExplode", true);
        isExplode = true;
        rb.gravityScale = 0f;
        explosionCollider.enabled = true;
        boxCollider2D.enabled = false;
        

        yield return new WaitForSeconds(explosionAnimation);
        // Disabilita il GameObject dopo l'animazione
         gameObject.SetActive(false);
       // Destroy(gameObject);
    }

    public void StarExplosion()
    {
        isHit = true;
        animator.SetBool("isHit", true);
        StartCoroutine(PlayExplosionSequence());
    }
    public bool GetIsHit()
    {
        return isHit;
    }
}
