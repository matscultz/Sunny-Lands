using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player_Movements : MonoBehaviour
{
    public float movVel = 1f;
    public float jumpForce = 1f;
    public GameObject myPrefab;
    public Animator animator;   // Riferimento all'animator
    public CinemachineVirtualCamera virtualCamera;  // Riferimento alla Camera Virtuale di Cinemachine
    public int jumpTime = 0;
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f); // Dimensioni dell'area di controllo
    public Transform upCheck;
    public Vector2 upCheckSize = new Vector2(0.5f, 0.1f); // Dimensioni dell'area di controllo
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public float knockbackForce = 5f;
    public int attackDamage = 0;

    private CinemachineFramingTransposer framingTransposer;
    private Rigidbody2D _rigidbody;
    private float scale;
    private bool rightFace = true;
    private bool onGround;

    private BoxCollider2D boxCollider2D;

    void Start()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        scale = transform.localScale.x;
        if (virtualCamera != null)
        {
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    void Update()
    {
       // Debug.Log(_rigidbody.velocity.y);
        // Input di movimento
        var movement_x = Input.GetAxisRaw("Horizontal");
        FixedUpdate();
        // Animazione di corsa
        animator.SetFloat("Speed", Mathf.Abs(movement_x));

        // Flip dello sprite
        if (movement_x < 0 && rightFace)
        {
            Flip();
        }
        else if (movement_x > 0 && !rightFace)
        {
            Flip();
        }

        // Animazione di salto
        onGround = IsGrounded();
        animator.SetBool("onGround", onGround);
        if (IsGrounded())
        {
            jumpTime = 0;
        }
        if (Input.GetButtonDown("Jump") && (onGround || jumpTime < 1))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
            animator.SetBool("isJump", true);
            jumpTime += 1;
            onGround = false; // Impedisce ulteriori reset del conteggio dei salti finché non si tocca terra
        }

        // Animazione di caduta
        if (_rigidbody.velocity.y < 0)
        {
            animator.SetBool("isFall", true);
            animator.SetBool("isJump", false);
           // LayerMask trapLayer = LayerMask.GetMask("Traps");
            // Controlla se il personaggio sta colpendo un nemico
            Collider2D hit = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, enemyLayer);
            Collider2D hitBomb = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, LayerMask.GetMask("Traps"));
            /*Debug.Log("E' la bomba: "+ hitBomb.gameObject.name.Equals("trap_bomb"));
            Debug.Log("Il nome e': " + hitBomb.gameObject.name);
            Debug.Log("Il layer e':" +hitBomb.gameObject.layer);*/
            if (hit != null && hit.CompareTag("Enemy"))
            {
                // Ottieni il componente EnemyController e chiama TakeDamage
                var damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);

                    // Rimbalza dopo aver colpito il nemico
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce / 2);
                    Debug.Log("Collided with: " + hit.gameObject.name + " on layer: " + LayerMask.LayerToName(hit.gameObject.layer));
                }
            }
            else if (_rigidbody.velocity.y == 0 || onGround)
            {
                animator.SetBool("isFall", false);
            }
            try
            {
                if (hitBomb.gameObject.name.Equals("trap_bomb"))
                {
                    hitBomb.gameObject.GetComponent<Trap_Bomb>().StarExplosion();

                }
            }
            catch { }


        }

        if (_rigidbody.velocity.y > 0)
        {

            // Controlla se il personaggio sta colpendo un nemico
            Collider2D hit = Physics2D.OverlapBox(upCheck.position, upCheckSize, 0f, enemyLayer);
            if (hit != null && hit.CompareTag("Enemy"))
            {
                // Ottieni il componente EnemyController e chiama TakeDamage
                var damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);

                    // Rimbalza dopo aver colpito il nemico
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -knockbackForce);
                    Debug.Log("Collided with: " + hit.gameObject.name + " on layer: " + LayerMask.LayerToName(hit.gameObject.layer));
                }
            }
        }

        void FixedUpdate()
        {
            // Movimento del personaggio
            var movement_x = Input.GetAxisRaw("Horizontal");
            _rigidbody.velocity = new Vector2(movement_x * movVel, _rigidbody.velocity.y);
        }

        void Flip()
        {
            rightFace = !rightFace;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
            framingTransposer.m_TrackedObjectOffset.x *= -1;
        }

        bool IsGrounded()
        {
            // Verifica se l'area di rilevamento colpisce il layer del terreno
            Collider2D groundInfo = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
            if (groundInfo == null)
            {
                Collider2D specialInfo = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, LayerMask.GetMask("Traps"));
                if (specialInfo != null && specialInfo.CompareTag("Bomb"))
                {
                    return true;
                }
            }

            return groundInfo != null;
        }
    }

    private void OnDrawGizmos()
    {
        // Disegna l'area di rilevamento nell'editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.DrawWireCube(upCheck.position, upCheckSize);
    }

    
}

