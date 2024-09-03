using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    public float speed = 2f; // Velocità di movimento del personaggio
    public Transform groundCheck; // Punto da cui parte il raycast per controllare il terreno
    public float groundCheckDistance = 1f; // Distanza del raycast per rilevare il terreno
    public Transform wallCheck; // Punto da cui parte il raycast per controllare i muri
    public float wallCheckDistance = 0.5f; // Distanza del raycast per rilevare i muri
    public LayerMask groundLayer; // Layer che rappresenta il terreno e i muri

    private bool movingRight = true; // Direzione iniziale del personaggio

    void Update()
    {
        // Muove il personaggio
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Controlla se c'è terreno davanti al personaggio
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        // Controlla se c'è un muro davanti al personaggio
        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, Vector2.right * (movingRight ? 1 : -1), wallCheckDistance, groundLayer);

        // Se non c'è più terreno o c'è un muro, inverte la direzione
        if (groundInfo.collider == false || wallInfo.collider == true)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        // Disegna il raycast per il controllo del terreno
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);

        // Disegna il raycast per il controllo del muro
        Gizmos.color = Color.blue;
        Vector3 direction = movingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + direction * wallCheckDistance);
    }
}
