using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    public static event Action OnGemCollected;

    // Example of using particles
    //public ParticleSystem collectParticle;

    public void Collect()
    {
        OnGemCollected?.Invoke();
        SoundManager.Instance.PlaySound2D("Pickup");
        // This is where you can play sounds or spawn particles
        //Instantiate(collectParticle, transform.position, Quaternion.identity);

        Debug.Log("Coin collected!");
        Destroy(gameObject);
    }
}
