using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtonSound : MonoBehaviour
{
    public GameObject[] buttonObjects; // Gli oggetti UI con EventTriggers

    private void Start()
    {
        AssignButtonSounds();
    }

    private void AssignButtonSounds()
    {
        SoundManager audioManager = SoundManager.Instance;

        if (audioManager != null)
        {
            foreach (GameObject buttonObj in buttonObjects)
            {
                EventTrigger trigger = buttonObj.GetComponent<EventTrigger>();
                if (trigger != null)
                {
                    // Rimuovi eventuali vecchi eventi
                    trigger.triggers.Clear();

                    // Crea un nuovo Entry per il click event
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((eventData) => { audioManager.PlaySound2D("ButtonClick"); });

                    // Aggiungi l'evento al trigger
                    trigger.triggers.Add(entry);
                }
            }
        }
    }
}


