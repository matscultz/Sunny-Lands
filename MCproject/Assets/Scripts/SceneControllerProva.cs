using UnityEngine;
using System.Collections.Generic;

public class SceneControllerProva : MonoBehaviour
{
    public GameObject objectToEnable; // Oggetto specifico da abilitare
    public List<GameObject> objectsToExclude; // Lista di oggetti da escludere dalla disabilitazione

    void Start()
    {
        // Disabilita tutti gli oggetti nella scena tranne quelli nella lista di esclusione
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (!objectsToExclude.Contains(obj))
            {
                obj.SetActive(false);
            }
        }

        // Abilita solo l'oggetto specifico
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }
}
