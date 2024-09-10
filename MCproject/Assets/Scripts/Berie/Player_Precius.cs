using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Precius : MonoBehaviour
{
    public static Player_Precius instance;
    public int coin = 0;
    public Text scoreText;
    public Button interactButton;
    private bool nearChest = false;
    private Treasure_Chest_Default chest;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        interactButton.onClick.AddListener(Interact);
    }

    // Update is called once per frame
    void Update()
    {
        if (nearChest && Input.GetKeyDown(KeyCode.L))
        {
            chest.OpenChest();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Chest"))
        {
            nearChest = true;
            chest = collider.GetComponent<Treasure_Chest_Default>();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Chest"))
        {
            nearChest = false;
            chest = null;
        }
    }
    public void AddCoin(int value)
    {
        coin += value;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Coin: " + coin;
    }

    public void PlaySpecialAnimation()
    {
        animator.SetTrigger("SpecialTreasure");
    }

    void Interact()
    {
        if (nearChest)
        {
            chest.OpenChest();
        }
    } 
}
