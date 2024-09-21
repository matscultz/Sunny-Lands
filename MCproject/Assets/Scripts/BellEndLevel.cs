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
    public float animationDuration = 2f;
    public float delayBetweenFields = 0.5f; // Ritardo tra l'apparizione di ogni campo
    private Button levelSelectionButton;
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
        timeText = levelSummaryUI.transform.GetChild(1).GetChild(3).GetComponent<Text>();
        deathText = levelSummaryUI.transform.GetChild(1).GetChild(2).GetComponent<Text>();
        levelSelectionButton = levelSummaryUI.transform.GetChild(1).GetChild(4).GetComponent<Button>();

        uiPlay = GameObject.Find("UIPlay");
        controlButtons = GameObject.Find("UI_Control_Buttons");
        // Nasconde i testi inizialmente
        coinText.gameObject.SetActive(false);
        diamondText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        deathText.gameObject.SetActive(false);
        // Aggiungi un listener al pulsante per tornare alla selezione dei livelli
        levelSelectionButton.onClick.AddListener(GoToLevelSelection);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound3D("LevelCompleted", transform.position);
            // Quando il player arriva al traguardo, completa il livello e mostra la schermata di riepilogo
            GameManager.Instance.CompleteLevel(GameManager.Instance.LevelIndex);
            uiPlay.SetActive(false);
            controlButtons.SetActive(false);
            ShowLevelSummary();
           
        }
    }

    void ShowLevelSummary()
    {
        Time.timeScale = 0;
        // Attiva la UI di riepilogo
        levelSummaryUI.SetActive(true);

        // Avvia la coroutine per mostrare e animare i campi gradualmente
        StartCoroutine(ShowFieldsOneByOne());
    }

    IEnumerator ShowFieldsOneByOne()
    {
        // Monete
        coinText.gameObject.SetActive(true);  // Mostra il testo delle monete
        yield return StartCoroutine(AnimateCount(coinText, 0, GameManager.Instance.CoinCount, animationDuration));
        yield return new WaitForSecondsRealtime(delayBetweenFields);  // Usa WaitForSecondsRealtime

        // Diamanti
        diamondText.gameObject.SetActive(true);  // Mostra il testo dei diamanti
        yield return StartCoroutine(AnimateCount(diamondText, 0, GameManager.Instance.DiamondCount, animationDuration));
        yield return new WaitForSecondsRealtime(delayBetweenFields);  // Usa WaitForSecondsRealtime

        // Morti
        deathText.gameObject.SetActive(true);  // Mostra il testo delle morti
        yield return StartCoroutine(AnimateCount(deathText, 0, GameManager.Instance.DeathCount, animationDuration));

        // Tempo
        timeText.gameObject.SetActive(true);  // Mostra il testo del tempo
        yield return StartCoroutine(AnimateTime(timeText, 0, GameManager.Instance.ElapsedTime, animationDuration));
        yield return new WaitForSecondsRealtime(delayBetweenFields);  // Usa WaitForSecondsRealtime

    }

    IEnumerator AnimateCount(Text targetText, int startValue, int endValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Usa Time.unscaledDeltaTime per continuare l'animazione anche con timeScale a 0
            elapsedTime += Time.unscaledDeltaTime;
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, elapsedTime / duration));
            targetText.text = currentValue.ToString();
            yield return null;
        }

        targetText.text = endValue.ToString();
    }

    IEnumerator AnimateTime(Text targetText, float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // Interpola il tempo tra startValue ed endValue
            float currentTime = Mathf.Lerp(startValue, endValue, elapsedTime / duration);

            // Calcola i minuti e i secondi
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            // Aggiorna il testo in formato "MM:SS"
            targetText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return null;
        }

        // Assicurati che il valore finale sia visualizzato correttamente
        int finalMinutes = Mathf.FloorToInt(endValue / 60);
        int finalSeconds = Mathf.FloorToInt(endValue % 60);
        targetText.text = string.Format("{0:00}:{1:00}", finalMinutes, finalSeconds);
    }

    void GoToLevelSelection()
    {
        Time.timeScale = 1;
        // Cambia scena per andare alla selezione dei livelli
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelection");
    }
}
