using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{
    public Transform target;    // Riferimento al personaggio da seguire
    public float smoothSpeed = 0.125f;  // Velocità di movimento della camera
    public Vector3 offset;      // Offset della camera rispetto al personaggio

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;  // Posizione desiderata della camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);  // Interpolazione per movimento fluido
        transform.position = smoothedPosition;  // Aggiorna la posizione della camera
    }
}
