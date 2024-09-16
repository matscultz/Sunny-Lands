using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    public int health;
    public float deadAnimation = 1f;
    public Animator animator;
    public string hitAnimationName = "Hit"; // Nome del trigger dell'animazione di attacco
    public Cinemachine.CinemachineVirtualCamera cinemachineCamera;
    public bool isImmune = false; // Stato dell'immunita'
    public float immuneDuration = 1f; // Durata dell'immunità
    public Text healText;
    private static int maxHealth = 100;
    private float immuneTemp;

    // Aggiungi il riferimento allo ScreenFade
    public ScreenFade screenFade;  // Associa il riferimento allo script ScreenFade

    private void Start()
    {
        health = maxHealth;
        healText.text = "" + health;
        immuneTemp = immuneDuration;
    }

    public void TakeDamage(int damage)
    {
        if (!isImmune)
        {
            animator.SetTrigger(hitAnimationName); // Trigger animazione colpo subito
            health -= damage;
            if (health < 0)
                health = 0;
            UpdateHealthUI();
            if (health <= 0)
            {
                Die();
            }
            StartCoroutine(ActivateImmunity());
        }
    }

    private void Die()
    {
        animator.SetTrigger("Death");
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();

        // Loop through all the colliders and disable them
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = false;
        }
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        SoundManager.Instance.PlaySound3D("Dead", transform.position);
        // Avvia il processo di fade e respawn
        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {

        // Aspetta la durata dell'animazione di morte
        yield return new WaitForSeconds(deadAnimation);

        // Fai il fade-in (schermo nero)
        yield return StartCoroutine(screenFade.FadeIn());

        // Disabilita il player temporaneamente
        //gameObject.SetActive(false);
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();

        // Loop through all the colliders and disable them
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = true;
        }
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        // Notifica il GameManager che il player deve rinascere
        animator.Play("character_berie_idle");
        GameManager.Instance.RespawnPlayer();
        // Riabilita il player e resetta la sua salute
        //gameObject.SetActive(true);
        ResetHealth();

        // Fai il fade-out (schermo torna normale)
        yield return StartCoroutine(screenFade.FadeOut());
    }

    IEnumerator ActivateImmunity()
    {
        isImmune = true;

        // Aspetta fino al termine dell'immunità
        yield return new WaitForSeconds(immuneDuration);

        // Disattiva l'immunità
        isImmune = false;
    }

    public void Heal(int amount)
    {
        health += amount;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healText.text = "" + health;
    }

    public void ResetHealth()
    {
        health = 50;
        UpdateHealthUI();
        isImmune = false;
        immuneDuration = immuneTemp;
    }
}
