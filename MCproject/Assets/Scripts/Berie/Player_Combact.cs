using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combact : MonoBehaviour
{
    public Animator animator;
    public Transform attackPointA;
    public Transform attackPointB;
    public LayerMask enemyLayers;
    public int attackDamage = 1;
    [SerializeField] public float areaWidth = 1f;
    private Rigidbody2D _rigidbody;
    private float attackCooldown; // Tempo di cooldown tra gli attacchi
    private float lastAttackTime = -Mathf.Infinity; // Tempo dell'ultimo attacco
    public string attackAnimationName = "Attack"; // Nome del trigger dell'animazione di attacco

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        // Calcola la durata dell'animazione di attacco
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            attackCooldown = clipInfo[0].clip.length;
        }
        else
        {
            Debug.LogWarning("Animazione di attacco non trovata.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TryAttack();
        }


    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time; // Aggiorna il tempo dell'ultimo attacco
        }
    }

    void Attack()
    {
        animator.SetTrigger(attackAnimationName); // Trigger animazione attacco
 
        Vector2 min = new Vector2(Mathf.Min(attackPointA.position.x, attackPointB.position.x), Mathf.Min(attackPointA.position.y, attackPointB.position.y));
        Vector2 max = new Vector2(Mathf.Max(attackPointA.position.x, attackPointB.position.x), Mathf.Max(attackPointA.position.y, attackPointB.position.y));

        Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(min, max, enemyLayers);
        
        
        foreach (Collider2D enemy in hitEnemies)
        {
            string name = enemy.name;
            Debug.Log("Nome nemico" + enemy.name);
            var damageable = enemy.GetComponent<IDamageable>();
            
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }

            if (enemy.gameObject.name.Equals("trap_bomb"))
            {
                enemy.gameObject.GetComponent<Trap_Bomb>().StarExplosion();
            }

            if (enemy.gameObject.name.StartsWith("Barrel"))
            {
                switch (name)
                {
                    case "Barrel_heavy":
                        enemy.gameObject.GetComponent<Barrel_Heavy>().HitDestroy();
                        break;
                    case "Barrel_light":
                        enemy.gameObject.GetComponent<Barrel_Light>().HitDestroy();
                        break;
                }
                
            }

        }

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

