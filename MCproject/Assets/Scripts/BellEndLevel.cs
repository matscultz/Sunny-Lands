using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BellEndLevel : MonoBehaviour
{
    public Text coinText;
    public Text diamondText;
    public Text timeText;
    public Text deathText;
    private Button levelSelectionButton;
    public float animationDuration = 2f;
    private GameObject levelSummaryUI;  // Il Canvas della schermata di riepilogo
    private GameObject uiPlay;
    private GameObject controlButtons;
    private void Start()
    {
        levelSummaryUI = GameObject.Find("UILevelSummary");
        // Disattiva la UI di riepilogo all'inizio
        levelSummaryUI.SetActive(false);
        coinText = levelSummaryUI.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        diamondText = levelSummaryUI.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        timeText = levelSummaryUI.transform.GetChild(1).GetChild(2).GetComponent<Text>();
        deathText = levelSummaryUI.transform.GetChild(1).GetChild(3).GetComponent<Text>();
        levelSelectionButton = levelSummaryUI.transform.GetChild(1).GetChild(4).GetComponent<Button>();

        uiPlay = GameObject.Find("UIPlay");
        controlButtons = GameObject.Find("UI_Control_Buttons");
        // Aggiungi un listener al pulsante per tornare alla selezione dei livelli
        levelSelectionButton.onClick.AddListener(GoToLevelSelection);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound3D("LevelCompleted", transform.position);
            // Quando il player arriva al traguardo, completa il livello e mostra la schermata di riepilogo
            GameManager.Instance.CompleteLevel();
            uiPlay.SetActive(false);
            controlButtons.SetActive(false);
            ShowLevelSummary();
            Time.timeScale = 0;
        }
    }

    void ShowLevelSummary()
    {
        // Attiva la UI di riepilogo
        levelSummaryUI.SetActive(true);

        // Aggiorna i testi con le statistiche
        coinText.text = "Coins collected: " + GameManager.Instance.MonetePrese.ToString();
        diamondText.text = "Diamonds collected: " + GameManager.Instance.DiamantiPresi.ToString()+"/3";
        timeText.text = "Time taken: " + GameManager.Instance.TempoImpiegato.ToString("F2") + "s";
        deathText.text = "Deaths number: " + GameManager.Instance.NumeroMorti.ToString();
    }

    void GoToLevelSelection()
    {
        // Cambia scena per andare alla selezione dei livelli
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectionScene");
    }
}
