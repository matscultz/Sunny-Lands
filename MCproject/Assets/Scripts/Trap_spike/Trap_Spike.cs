using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Spike : MonoBehaviour
{
    public int damage = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Puoi accedere a uno script sul player per infliggere il danno
            Player_Health playerHealth = collision.GetComponent<Player_Health>();

            // Se lo script PlayerHealth esiste, infligge il danno
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
