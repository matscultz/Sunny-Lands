using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 lastCheckpointPosition;
    private Player_Health playerHealth;
    private Player_Precius playerScore;
    private int numeroMorti;
    private int diamantiPresi;
    private int monetePrese;
    private float tempoImpiegato;
    private bool levelComplete = false;

    public int NumeroMorti { get => numeroMorti; set => numeroMorti = value; }
    public int DiamantiPresi { get => diamantiPresi; set => diamantiPresi = value; }
    public float TempoImpiegato { get => tempoImpiegato; set => tempoImpiegato = value; }
    public int MonetePrese { get => monetePrese; set => monetePrese = value; }

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

    void Update()
    {
        if (!levelComplete)
        {
            // Aumenta il timer del livello finché non è completato
            TempoImpiegato += Time.deltaTime;
        }
    }

    private void Start()
    {
        numeroMorti = 0;
        DiamantiPresi = 0;
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
        numeroMorti++;
        // Imposta la posizione del player al checkpoint
        playerHealth.transform.position = lastCheckpointPosition;

        // Ripristina la salute e il punteggio del player
        playerHealth.ResetHealth();
        //playerScore.ResetScore(); // Solo se necessario ripristinare il punteggio
    }


    public void CompleteLevel()
    {
        levelComplete = true;
    }
}
