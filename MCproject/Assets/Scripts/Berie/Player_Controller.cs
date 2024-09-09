using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public static Player_Controller instance;  // Singleton per accedere al player

    private Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
    }

    // Gioca l'animazione speciale
    public void PlaySpecialAnimation()
    {
        animator.SetTrigger("SpecialTreasure");
    }
}
