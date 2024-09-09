using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int score = 0;

    void Awake()
    {
        // Assicurati che ci sia solo un'istanza di GameManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Metodo per aggiungere punti
    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Punteggio: " + score);
    }
}
