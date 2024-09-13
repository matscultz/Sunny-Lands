using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Gold : MonoBehaviour
{
    public int coinValue = 50;  // Il valore della moneta

    // Quando il player entra nel trigger della moneta
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Controlla se l'oggetto che è entrato nel trigger è il player
        if (collider.CompareTag("Player"))
        {
            // Raccogli la moneta (qui puoi aumentare il punteggio o fare altro)
            CollectCoin(collider);
        }
    }

    // Funzione per gestire la raccolta della moneta
    void CollectCoin(Collider2D collider)
    {

        collider.gameObject.GetComponent<Player_Precius>().AddCoin(coinValue);
        SoundManager.Instance.PlaySound3D("Pickup", transform.position);
        // Distruggi la moneta una volta raccolta
        Destroy(gameObject);
    }
}
