using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    private Transform player;
    public Vector3 offset = new Vector3(0, 1f, 0);  // Offset per la posizione della moneta speciale
    public float followDuration = 0.3f; // Durata per cui l'oggetto seguirà il giocatore
    private float followTime;

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        followTime = followDuration;
    }

    void Update()
    {
        if (player != null && followTime > 0)
        {
            // Segui il giocatore mantenendo un offset verticale
            transform.position = new Vector3(player.position.x, player.position.y + offset.y, transform.position.z);

            // Riduce il tempo di durata
            followTime -= Time.deltaTime;

            // Se il tempo è scaduto, distrugge l'oggetto
            if (followTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    public void SetOffset(Vector3 offset)
    {
        this.offset = offset;
    }
}
