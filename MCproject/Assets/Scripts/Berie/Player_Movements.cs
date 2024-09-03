using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public Transform groundCheck; // Punto da cui parte il raycast per controllare il terreno
    public float groundCheckDistance = 1f; // Distanza del raycast per rilevare il terreno
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    private CinemachineFramingTransposer framingTransposer;
    private Rigidbody2D _rigidbody;
    private float scale;
    private bool rightFace = true;
    private bool onGround;

    private BoxCollider2D boxCollider2D;

    void Start()
    {
        animator = GetComponent<Animator>();
        //animator.gameObject.SetActive(true);
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
        /*Debug.Log(animator.runtimeAnimatorController.name);
         Debug.Log("attivo?: " + animator.isActiveAndEnabled);
         Debug.Log(animator.gameObject.activeSelf);
         Debug.Log("AnimatorController assigned: " + animator.runtimeAnimatorController != null);
        Debug.Log("framingTransposer esiste: " + framingTransposer != null);
        Debug.Log("virtualCamera esiste: " + virtualCamera != null);*/
        // Input di movimento
        var movement_x = Input.GetAxisRaw("Horizontal");
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
            jumpTime+=1;
            onGround = false; // Impedisce ulteriori reset del conteggio dei salti finché non si tocca terra
            

        }

        // Animazione di caduta
        if (_rigidbody.velocity.y < 0)
        {
            animator.SetBool("isFall", true);
            animator.SetBool("isJump", false);
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, enemyLayer);
            //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // Ottieni il componente EnemyController e chiama TakeDamage
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(50); // Puoi specificare il valore del danno
                }

                // Rimbalza dopo aver colpito il nemico
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce / 2);
            }
        }
        else if (_rigidbody.velocity.y == 0 || onGround)
        {
            animator.SetBool("isFall", false);
        }

    }

    void FixedUpdate()
    {
        // Movimento del personaggio
        var movement_x = Input.GetAxisRaw("Horizontal");
        _rigidbody.velocity = new Vector2(movement_x * movVel, _rigidbody.velocity.y);
    }

    /* private void OnCollisionEnter2D(Collision2D collision)
     {
         Debug.Log("Collision detected with: " + collision.gameObject.name);
         Collider2D otherCollider = collision.collider;
         if (collision.gameObject.CompareTag("Ground"))
         {

             onGround = true;
             animator.SetBool("onGround", true);
             animator.SetBool("isJump", false);
             jumpTime = 0;
         }
     }*/


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

        // Esegui il Raycast sotto il personaggio
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        //Debug.Log("Raycast groud: " + groundInfo.collider.gameObject.name);

        // Se il Raycast colpisce qualcosa, significa che siamo a terra
        return groundInfo.collider != null;
    }

    void OnDrawGizmos()
    {
        // Disegna il raycast per il controllo del terreno
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
    }
}
