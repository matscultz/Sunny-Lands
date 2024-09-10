using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCam; // Riferimento alla tua Cinemachine Virtual Camera
    public float screenXRight = 0.34f; // Valore di Screen X per la direzione destra
    public float screenXLeft = -0.34f; // Valore di Screen X per la direzione sinistra
    public bool isFacingRight; // Direzione iniziale del player

    private void Start()
    {
        isFacingRight = true;
    }

    private void Update()
    {
        // Rileva la direzione del giocatore (supponendo che si ribalti l'asse X per il flip)
        if (Input.GetAxisRaw("Horizontal") > 0 && !isFacingRight)
        {
            FlipDirection(true); // Destra
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && isFacingRight)
        {
            FlipDirection(false); // Sinistra
        }
    }

    void FlipDirection(bool facingRight)
    {
        isFacingRight = facingRight;

        // Modifica il valore di Screen X della Cinemachine
        CinemachineFramingTransposer transposer = cinemachineCam.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (isFacingRight)
        {
            transposer.m_ScreenX = screenXRight;
        }
        else
        {
            transposer.m_ScreenX = screenXLeft;
        }
    }
}
