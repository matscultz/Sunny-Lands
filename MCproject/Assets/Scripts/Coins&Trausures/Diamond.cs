using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public Transform playerTransform;  // Riferimento al player
    public Vector3 offset;  // Offset della posizione della moneta rispetto al player

    private void Update()
    {
        // Aggiorna la posizione della moneta per farla seguire il player
        transform.position = playerTransform.position + offset;
    }
}
