using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagul_fly : MonoBehaviour
{
    public float speed = 2f; // Velocità del movimento
    public float height = 2f; // Altezza del movimento
    public float fallDelay = 0.5f; // Ritardo prima della caduta
    public float bounceForce = 5f;       // La forza con cui il player rimbalza
    private Animator animator;
    private Rigidbody2D rb2d;
    private Vector3 startingPosition;
    private bool isDead = false;
    void Start()
    {
        // Ottieni il Rigidbody2D
        rb2d = GetComponent<Rigidbody2D>();
        // Salva la posizione di partenza
        startingPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
        {
            // Calcola il movimento su e giù usando una sinusoide
            float newY = Mathf.Sin(Time.time * speed) * height;
            transform.position = new Vector3(startingPosition.x, startingPosition.y + newY, startingPosition.z);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Controlla se l'oggetto che collabora è il Player
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                SoundManager.Instance.PlaySound3D("Bounce Barrel", transform.position);
                Vector2 bounce = new Vector2(0, bounceForce);
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounce.y);
                animator.SetBool("isDead",true);
                isDead = true;
            }
            // Avvia il ritardo prima della caduta
            Invoke("Fall", fallDelay);
        }
    }

    void Fall()
    {
        // Imposta il Rigidbody2D come dinamico per farlo cadere
        rb2d.isKinematic = false;
        Invoke("DestroyObject", 1f );
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
