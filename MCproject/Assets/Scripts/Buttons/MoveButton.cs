using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  // Namespace necessario per IPointerDownHandler e IPointerUpHandler

public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed = false;

    // Chiamato quando il pulsante viene premuto
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    // Chiamato quando il pulsante viene rilasciato
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
