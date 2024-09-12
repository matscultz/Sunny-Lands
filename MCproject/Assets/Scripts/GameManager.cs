using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 lastCheckpointPosition;
    private Player_Health playerHealth;
    private Player_Precius playerScore;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Trova gli script del Player
        playerHealth = FindObjectOfType<Player_Health>();
        playerScore = FindObjectOfType<Player_Precius>();
    }

    // Imposta la posizione del checkpoint
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    // Ripristina il player alla posizione del checkpoint
    public void RespawnPlayer()
    {
        // Imposta la posizione del player al checkpoint
        playerHealth.transform.position = lastCheckpointPosition;

        // Ripristina la salute e il punteggio del player
        playerHealth.ResetHealth();
        //playerScore.ResetScore(); // Solo se necessario ripristinare il punteggio
    }
}
