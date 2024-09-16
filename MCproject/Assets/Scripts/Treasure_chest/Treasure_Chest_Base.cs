using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure_Chest_Base : MonoBehaviour, IOpenChest
{

    public GameObject coinPrefab;  // Prefab della moneta
    public int numCoins = 10;  // Numero di monete da spawnare
    private Animator animator;
    private bool isOpen = false;
    private Transform coinSpawnPoint;  // Il punto da cui spawnano le monete
    // Start is called before the first frame update

    void Start()
    {
        coinSpawnPoint = transform.GetChild(0);
        animator = GetComponent<Animator>();
    }


    public void OpenChest()
    {
        if (isOpen) return;  // Evita di aprire la chest più volte
        isOpen = true;
        SoundManager.Instance.PlaySound3D("Chest", transform.position);

        // Genera monete
        for (int i = 0; i < numCoins; i++)
        {
            SpawnCoin();
        }

        animator.SetBool("isOpen", true);
    }

    void SpawnCoin()
    {
        GameObject coin = Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
        // Impulso verso l'alto e casuale in direzione orizzontale
        float forceX = Random.Range(-2f, 2f);
        float forceY = Random.Range(2f, 5f);
        rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
    }

}
