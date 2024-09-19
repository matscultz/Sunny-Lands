using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionScene : MonoBehaviour
{
    public Button[] buttons;
    private bool[] levelsUnlocked;

    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            buttons[i] = gameObject.transform.Find("level" + (i + 1)).GetComponent<Button>();
        }
    }

    private void Start()
    {
        int totalLevels = buttons.Length;
        levelsUnlocked = SaveManager.LoadGame(totalLevels);
        UpdateLevelButtons();
    }

    private void UpdateLevelButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            // Abilita il pulsante solo se il livello è sbloccato
            buttons[i].interactable = levelsUnlocked[i];
        }
    }

    public void OpenLevel(int levelId)
    {
        // Assicurati che il livelloId sia valido
        if (levelId > 0 && levelId <= buttons.Length)
        {
            GameManager.Instance.LevelIndex = levelId-1;
            string levelName = "Level" + levelId;
            SceneManager.LoadScene(levelName);
            MusicManager.Instance.PlayMusic("Level");
        }
        else
        {
            Debug.LogWarning("Invalid levelId: " + levelId);
        }
    }

    public void Home()
    {
        SceneManager.LoadScene("mainpage");
    }
}
