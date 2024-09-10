using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AlignWithTilemap : MonoBehaviour
{
    public Tilemap tilemap; // Riferimento alla Tilemap

    void Start()
    {
        // Prendi la posizione dell'oggetto (che è lo stesso a cui è collegato lo script)
        Vector3 worldPosition = transform.position;

        // Converti la posizione del mondo in coordinate della Tilemap
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // Converti la posizione della cella di nuovo in coordinate del mondo
        Vector3 alignedPosition = tilemap.GetCellCenterWorld(cellPosition);

        // Posiziona l'oggetto perfettamente al centro della tile
        transform.position = alignedPosition;
    }
}
