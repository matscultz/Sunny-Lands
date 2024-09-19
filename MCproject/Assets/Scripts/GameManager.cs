using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int totalLevels;
    private int levelIndex;
    private Vector3 lastCheckpointPosition;
    private Player_Health playerHealth;
    private Player_Precius playerScore;
    private int deathCount;
    private int diamondCount;
    private int coinCount;
    private float elapsedTime;
    private bool levelComplete = false;
    private bool[] levelsUnlocked;

    public int DeathCount { get => deathCount; set => deathCount = value; }
    public int DiamondCount { get => diamondCount; set => diamondCount = value; }
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
    public int CoinCount { get => coinCount; set => coinCount = value; }
    public int LevelIndex { get => levelIndex; set => levelIndex = value; }

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
        totalLevels = 6;
        levelsUnlocked = SaveManager.LoadGame(totalLevels);
        deathCount = 0;
        DiamondCount = 0;
        // Trova gli script del Player
        playerHealth = FindObjectOfType<Player_Health>();
        playerScore = FindObjectOfType<Player_Precius>();

    }
    void Update()
    {
        if (!levelComplete)
        {
            // Aumenta il timer del livello finché non è completato
            ElapsedTime += Time.deltaTime;
        }
    }

    // Imposta la posizione del checkpoint
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    // Ripristina il player alla posizione del checkpoint
    public void RespawnPlayer()
    {
        deathCount++;
        // Imposta la posizione del player al checkpoint
        playerHealth.transform.position = lastCheckpointPosition;

        // Ripristina la salute e il punteggio del player
        playerHealth.ResetHealth();
        //playerScore.ResetScore(); // Solo se necessario ripristinare il punteggio
    }


    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelsUnlocked.Length)
        {
            // Segna il livello corrente come completato
            levelsUnlocked[levelIndex] = true;

            // Sblocca anche il livello successivo, se esiste
            if (levelIndex + 1 < levelsUnlocked.Length)
            {
                levelsUnlocked[levelIndex + 1] = true;
            }

            // Salva lo stato aggiornato dei livelli
            SaveManager.SaveGame(levelsUnlocked);
        }
    }
}
