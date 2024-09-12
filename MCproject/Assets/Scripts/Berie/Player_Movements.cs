using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
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
    public MoveButton leftButton;
    public MoveButton rightButton;
    public Button jumpButton;

    private CinemachineFramingTransposer framingTransposer;
    private Rigidbody2D _rigidbody;
    private float scale;
    private bool rightFace = true;
    private bool onGround;


    void Start()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
        
        if (virtualCamera != null)
        {
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        jumpTime = 0;
        jumpButton.onClick.AddListener(Jump);
    }

    void Update()
    {
        float movement_x = 0f;
        if (leftButton.isPressed)
        {
            movement_x = -1f;
        }
        else if ((rightButton.isPressed))
        {
            movement_x = 1f;
        }
        else
        {
            movement_x = Input.GetAxisRaw("Horizontal"); // Controllo anche da tastiera
        }
        _rigidbody.velocity = new Vector2(movement_x * movVel, _rigidbody.velocity.y);
        animator.SetFloat("Speed", movement_x);
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
            animator.SetBool("isFall", false);
            animator.SetBool("isJump", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Animazione di caduta
        if (_rigidbody.velocity.y < 0)
        {
            animator.SetBool("isFall", true);
            animator.SetBool("isJump", false);
            // Controlla se il personaggio sta colpendo un nemico
            Collider2D hit = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, enemyLayer);
            Collider2D hitBomb = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, LayerMask.GetMask("Traps"));
           
            if (hit != null && hit.CompareTag("Enemy"))
            {
                // Ottieni il componente EnemyController e chiama TakeDamage
                var damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);

                    // Rimbalza dopo aver colpito il nemico
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce / 2);
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
            animator.SetBool("isJump", true);
            animator.SetBool("isFall", false);
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
                }
            }
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
                Collider2D chestInfo = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, LayerMask.GetMask("Interactive"));
                Collider2D specialInfo = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, LayerMask.GetMask("Traps"));
                if (specialInfo != null && specialInfo.CompareTag("Bomb"))
                {
                    return true;
                }
                if(chestInfo != null && chestInfo.CompareTag("Chest"))
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


    void Jump()
    {
        if (onGround || jumpTime < 1) // Permetti di saltare se è a terra o se ha fatto meno di 2 salti
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
            jumpTime++; // Incrementa il contatore dei salti
            onGround = false; // Disabilita ulteriori salti finché non torna a terra
            
        }
    }
}

