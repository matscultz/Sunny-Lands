using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Octopus : MonoBehaviour, IDamageable
{
    public float detectionRadius = 5f;
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float fireRate = 2f;
    public Animator animator;
    public Transform attackPointA;
    public Transform attackPointB;
    public LayerMask enemyLayers;
    [SerializeField] public float areaWidth = 1f;
    public float deadAnimation = 1f;
    public int maxHealth = 1;

    private int currentHealth;
    private Rigidbody2D rb;
    private float fireCooldown;
    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            Vector2 min = new Vector2(Mathf.Min(attackPointA.position.x, attackPointB.position.x), Mathf.Min(attackPointA.position.y, attackPointB.position.y));
            Vector2 max = new Vector2(Mathf.Max(attackPointA.position.x, attackPointB.position.x), Mathf.Max(attackPointA.position.y, attackPointB.position.y));

            Collider2D[] playersInRange = Physics2D.OverlapAreaAll(min, max, enemyLayers);
            foreach (var player in playersInRange)
            {
                if (player.CompareTag("Player"))
                {
                    animator.SetBool("isAttack", true);
                    FireProjectile();
                    fireCooldown = fireRate;
                    break;
                }
            }
        }
        else
        {
            animator.SetBool("isAttack", false);
        }
    }

    private void FireProjectile()
    {
        
        Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        // Disegna un rettangolo per rappresentare l'area dell'attacco
        Vector2 center = (attackPointA.position + attackPointB.position) / 2;
        Vector2 size = new Vector2(areaWidth, Mathf.Abs(attackPointB.position.y - attackPointA.position.y));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    public void UpdateAreaWidth(float newWidth)
    {
        areaWidth = newWidth;
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
        
        // Disabilita il collider per evitare ulteriori interazioni
        GetComponent<Collider2D>().enabled = false;

        

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

