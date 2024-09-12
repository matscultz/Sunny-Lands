using System.Collections;
using System.Collections.Generic;
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
    private void Start()
    {
        health = maxHealth;
        healText.text = ""+health;
        immuneTemp = immuneDuration;
    }

    private void Update()
    {
        
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
        immuneDuration = 1.5f;
        /* GameObject placeholder = new GameObject("Placeholder");
         placeholder.transform.position = transform.position;

         // Imposta la camera per seguire l'oggetto vuoto
         cinemachineCamera.Follow = placeholder.transform;
         // Avvia la coroutine per disabilitare il GameObject dopo l'animazione di morte*/
        StartCoroutine(DisableAfterDeathAnimation());
    }

    IEnumerator DisableAfterDeathAnimation()
    {
        // Aspetta la durata dell'animazione di morte
        yield return new WaitForSeconds(deadAnimation);

        // Disabilita il player temporaneamente
        gameObject.SetActive(false);

        // Notifica il GameManager che il player deve rinascere
        GameManager.Instance.RespawnPlayer();

        // Riabilita il player
        gameObject.SetActive(true);
    }

    IEnumerator ActivateImmunity()
    {
        isImmune = true;

        // Aspetta fino al termine dell'animazione
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
        health = maxHealth;
        UpdateHealthUI();
        isImmune = false;
        immuneDuration = immuneTemp;
    }
}
