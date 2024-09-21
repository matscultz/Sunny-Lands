using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint;  // Il punto di partenza della piattaforma
    public Transform endPoint;    // Il punto di arrivo della piattaforma
    public float speed = 2.0f;    // Velocità del movimento

    private Vector3 targetPosition;  // Posizione di destinazione

    void Start()
    {
        // Iniziamo il movimento verso il punto di partenza
        targetPosition = endPoint.position;
    }

    void Update()
    {
        // Muoviamo la piattaforma verso la posizione target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Quando la piattaforma raggiunge la destinazione, cambiamo il target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (targetPosition == endPoint.position)
            {
                targetPosition = startPoint.position;  // Se è all'endPoint, torniamo al startPoint
            }
            else
            {
                targetPosition = endPoint.position;    // Altrimenti andiamo verso l'endPoint
            }
        }
    }
}
