using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel_Light : MonoBehaviour
{
    public float bounceForce = 5f;       // La forza con cui il player rimbalza
    public float durataAnimazione = 0.5f;
    private Animator animator;            // Riferimento all'animator dell'oggetto
    private bool isDestroyed = false;     // Stato di distruzione

    void Start()
    {
        // Recupera l'animator dall'oggetto (se presente)
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Controlla se il player è entrato in collisione
        if (collision.gameObject.tag == "Player" && !isDestroyed)
        {
            // Applica una forza verso l'alto al player per simulare il rimbalzo
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                SoundManager.Instance.PlaySound2D("Bounce Barrel");
                Vector2 bounce = new Vector2(0, bounceForce);
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounce.y);
                animator.SetTrigger("Destroy");
            }
            SoundManager.Instance.PlaySound2D("BarrelSmash");
            // Distruggi l'oggetto dopo un breve ritardo per permettere all'animazione di completarsi
            Invoke("DestroyObject", durataAnimazione); // Cambia il ritardo in base alla durata dell'animazione
            isDestroyed = true;
           
        }
    }

    // Funzione per distruggere l'oggetto
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void HitDestroy()
    {
        SoundManager.Instance.PlaySound2D("BarrelSmash");
        animator.SetTrigger("Destroy");
        Invoke("DestroyObject", durataAnimazione);
        isDestroyed = true;
    }

}

