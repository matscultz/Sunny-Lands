using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Precius : MonoBehaviour
{
    public static Player_Precius instance;
    public int coin = 0;
    private Text scoreText;
    private Button interactButton;
    private DiamondUIManager diamondUIManager;
    private bool nearChest = false;
    private Animator animator;
    private IOpenChest open;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("CoinText").GetComponent<Text>();
        interactButton = GameObject.Find("Interact").GetComponent<Button>();
        diamondUIManager = GameObject.Find("DiamondUIManager").GetComponent<DiamondUIManager>();
        animator = GetComponent<Animator>();
        interactButton.onClick.AddListener(Interact);
    }

    // Update is called once per frame
    void Update()
    {
        if (nearChest && Input.GetKeyDown(KeyCode.L))
        {
            open.OpenChest();
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Chest"))
        {
            nearChest = true;

            open = collider.gameObject.GetComponent<IOpenChest>();
        }

        if (collider.CompareTag("Diamond"))
        {
            SoundManager.Instance.PlaySound2D("Diamond");
            diamondUIManager.AddDiamond();
            PlaySpecialAnimation();
            Destroy(collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Chest"))
        {
            nearChest = false;
            open = null;
        }
    }
    public void AddCoin(int value)
    {
        coin += value;
        UpdateScoreUI();
        GameManager.Instance.CoinCount+=value;
    }

    void UpdateScoreUI()
    {
        scoreText.text = "" + coin;
    }

    public void PlaySpecialAnimation()
    {
        animator.SetTrigger("SpecialTreasure");
    }

    void Interact()
    {
        if (nearChest)
        {
            open.OpenChest();
        }
    }

    // Ripristina il punteggio al momento del respawn (se necessario)
    public void ResetScore()
    {
        // Decidi se vuoi resettare il punteggio a 0 o mantenere il valore corrente
        // score = 0;  // Se desideri azzerare il punteggio
    }
}
