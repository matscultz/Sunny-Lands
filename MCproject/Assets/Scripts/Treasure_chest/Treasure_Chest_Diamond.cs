using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure_Chest_Diamond : MonoBehaviour, IOpenChest
{
    [System.Serializable]
    public class CoinType
    {
        public GameObject coinPrefab;  // Prefab della moneta
        public int amount;             // Numero di monete di questo tipo da spawnare
    }
    public Transform coinSpawnPoint;  // Il punto da cui spawnano le monete
    public GameObject specialCoinPrefab;  // Prefab della moneta speciale
    public Vector3 specialCoinOffset = new Vector3(0, 0.7f, 0);  // Offset per la posizione della moneta speciale
    public float delay;
    public List<CoinType> coinTypes;   // Lista dei tipi di monete e quantità
    public DiamondUIManager diamondUIManager;
    private Player_Precius player;
    private Animator animator;
    private bool isOpen = false;
    private Transform playerTransform;
    private GameObject playerFind;


    // Start is called before the first frame update
    void Start()
    {
        playerFind = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        playerTransform = playerFind.transform;
        player = playerFind.GetComponent<Player_Precius>();
        diamondUIManager = GameObject.FindGameObjectWithTag("DiamondUIManager").GetComponent<DiamondUIManager>();
    }


    public void OpenChest()
    {
        if (isOpen) return;  // Evita di aprire la chest più volte
        isOpen = true;
        // Genera monete
        SpawnCoin();
        animator.SetBool("isOpen", true);
        // Spawna la moneta speciale sopra al player
        player.PlaySpecialAnimation();
        StartCoroutine(SpawnSpecialCoinWithDelay(delay));

        
    }

    void SpawnCoin()
    {
        foreach (CoinType coinType in coinTypes)
        {
            for (int i = 0; i < coinType.amount; i++)
            {
                // Calcola la posizione di spawn attorno al giocatore
                GameObject coinInstance = Instantiate(coinType.coinPrefab, coinSpawnPoint.position, Quaternion.identity);
                Rigidbody2D rb = coinInstance.GetComponent<Rigidbody2D>();
                float forceX = Random.Range(-2f, 2f);
                float forceY = Random.Range(2f, 5f);
                rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
            }
        }
        
        // Impulso verso l'alto e casuale in direzione orizzontale
        
    }


    void CollectSpecialCoin(GameObject specialCoin)
    {
        SoundManager.Instance.PlaySound3D("Diamond", transform.position);
        diamondUIManager.AddDiamond();

    }

    IEnumerator SpawnSpecialCoinWithDelay(float delay)
    {
        // Aspetta per il ritardo specificato
        yield return new WaitForSeconds(delay);

        // Instanzia la moneta speciale sopra al player con un offset
        Vector3 spawnPosition = playerTransform.position + specialCoinOffset;
        GameObject specialCoin = Instantiate(specialCoinPrefab, spawnPosition, Quaternion.identity);

        // Imposta il playerTransform e l'offset per la moneta speciale
        Follow_Player specialCoinScript = specialCoin.GetComponent<Follow_Player>();
        if (specialCoinScript != null)
        {
            specialCoinScript.Initialize(playerTransform);
        }

        // Raccogli la moneta immediatamente e avvia l'animazione del player
        CollectSpecialCoin(specialCoin);
    }
}