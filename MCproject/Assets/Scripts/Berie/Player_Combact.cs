using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Combact : MonoBehaviour
{
    public Animator animator;
    public Transform attackPointA;
    public Transform attackPointB;
    public LayerMask enemyLayers;
    public LayerMask barrelLayers;
    public LayerMask otherLayers;
    public int attackDamage = 1;
    [SerializeField] public float areaWidth = 1f;
    private Rigidbody2D _rigidbody;
    private float attackCooldown; // Tempo di cooldown tra gli attacchi
    private float lastAttackTime = -Mathf.Infinity; // Tempo dell'ultimo attacco
    public string attackAnimationName = "Attack"; // Nome del trigger dell'animazione di attacco
    public Button attackButton;
    void Start()
    {
        attackButton = GameObject.Find("Attack").GetComponent<Button>();
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
        attackButton.onClick.AddListener(TryAttack);
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
            SoundManager.Instance.PlaySound2D("NoHit");
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
        Collider2D[] hitBarrels = Physics2D.OverlapAreaAll(min, max, barrelLayers);
        Collider2D[] other = Physics2D.OverlapAreaAll(min, max, otherLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            string name = enemy.name;
            Debug.Log("Nome nemico" + enemy.name);
            var damageable = enemy.GetComponent<IDamageable>();
            
            if (damageable != null)
            {
                SoundManager.Instance.PlaySound2D("HitSomething");
                damageable.TakeDamage(attackDamage);
            }

            if (enemy.gameObject.name.StartsWith("trap_bomb"))
            {
                SoundManager.Instance.PlaySound2D("HitSomething");
                enemy.gameObject.GetComponent<Trap_Bomb>().StarExplosion();
            }

        }
        foreach(Collider2D barrel in hitBarrels)
        {
            if (barrel.gameObject.name.StartsWith("Barrel_heavy"))
                {
                SoundManager.Instance.PlaySound2D("HitSomething");
                }
                if (barrel.gameObject.name.StartsWith("Barrel_light"))
                {
                SoundManager.Instance.PlaySound2D("HitSomething");

                barrel.gameObject.GetComponent<Barrel_Light>().HitDestroy();
                }

        }

        foreach (Collider2D otr in other)
        {
            SoundManager.Instance.PlaySound2D("HitSomething");
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

