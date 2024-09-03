using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Octopus : MonoBehaviour
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
        animator.SetBool("isAttack", true);
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
}
