using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Octopus_explosionBullet : MonoBehaviour
{
    public float explosionDuration = 1f; // Durata dell'animazione di esplosione

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, explosionDuration);
    }
}
