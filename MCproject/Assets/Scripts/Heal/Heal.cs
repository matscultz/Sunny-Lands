using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public int heal_amount=15;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound2D("Heal");
            collision.gameObject.GetComponent<Player_Health>().Heal(heal_amount);
            Destroy(gameObject);
        }
    }
}
