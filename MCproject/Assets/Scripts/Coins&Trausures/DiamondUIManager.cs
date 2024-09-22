using UnityEngine;
using UnityEngine.UI;

public class DiamondUIManager : MonoBehaviour
{
    public Image[] diamondSlots; // Array degli slot (le immagini nere di diamanti)
    public Sprite diamondIcon; // L'immagine del diamante che appare quando raccolto

    private int collectedDiamonds = 0;
    private void Start()
    {
        for(int i=0; i<3; i++)
        {
            diamondSlots[i] = GameObject.Find("diamond"+i+1).GetComponent<Image>();
        }
    }
    // Metodo per aggiungere un diamante
    public void AddDiamond()
    {
        if (collectedDiamonds < diamondSlots.Length)
        {
            GameManager.Instance.DiamondCount++;
            diamondSlots[collectedDiamonds].sprite = diamondIcon; // Sostituisci lo slot nero con l'icona del diamante
            diamondSlots[collectedDiamonds].color = Color.white;
            collectedDiamonds++;
        }
    }

}
