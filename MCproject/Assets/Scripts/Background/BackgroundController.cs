using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPosX, startPosY, lengthX, heightY;
    public GameObject cam;
    public float parallaxEffectX; // Effetto parallax per asse X
    public float parallaxEffectY; // Effetto parallax per asse Y

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x; // Larghezza del background sull'asse X
        heightY = GetComponent<SpriteRenderer>().bounds.size.y; // Altezza del background sull'asse Y (per eventuale scrolling infinito su Y)
    }

    void FixedUpdate()
    {
        // Calcola la distanza da percorrere in base al movimento della fotocamera sull'asse X
        float distanceX = cam.transform.position.x * parallaxEffectX;
        float movementX = cam.transform.position.x * (1 - parallaxEffectX);

        // Calcola la distanza da percorrere in base al movimento della fotocamera sull'asse Y
        float distanceY = cam.transform.position.y * parallaxEffectY;
        float movementY = cam.transform.position.y * (1 - parallaxEffectY);

        // Debugging per verificare il movimento della telecamera
        Debug.Log("Camera Y Position: " + cam.transform.position.y);
        Debug.Log("Parallax Effect Y: " + parallaxEffectY);
        Debug.Log("Calculated distanceY: " + distanceY);

        // Aggiorna la posizione del background basata sui calcoli di parallax sugli assi X e Y
        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

        // Gestisci lo scrolling infinito per l'asse X
        if (movementX > startPosX + lengthX)
        {
            startPosX += lengthX;
        }
        else if (movementX < startPosX - lengthX)
        {
            startPosX -= lengthX;
        }

        // Eventuale gestione scrolling infinito per l'asse Y (non sempre necessario)
        // if (movementY > startPosY + heightY) {
        //     startPosY += heightY;
        // } else if (movementY < startPosY - heightY) {
        //     startPosY -= heightY;
        // }
    }
}
