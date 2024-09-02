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

    private CinemachineFramingTransposer framingTransposer;
    private Rigidbody2D _rigidbody;
    private float scale;
    private bool rightFace = true;
    private bool onGround = true;
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
         Debug.Log("AnimatorController assigned: " + animator.runtimeAnimatorController != null);*/
        Debug.Log("framingTransposer esiste: " + framingTransposer != null);
        Debug.Log("virtualCamera esiste: " + virtualCamera != null);

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
        if (Input.GetButtonDown("Jump") && onGround)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJump", true);
        }

        // Animazione di caduta
        if (_rigidbody.velocity.y < 0)
        {
            animator.SetBool("isFall", true);
            animator.SetBool("isJump", false);
        }
        else if (_rigidbody.velocity.y == 0 && onGround)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            animator.SetBool("onGround", true);
            animator.SetBool("isJump", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
            animator.SetBool("onGround", false);
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
}
