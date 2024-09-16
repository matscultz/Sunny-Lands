using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionScene : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            buttons[i] = gameObject.transform.Find("Level" + (i + 1)).GetComponent<Button>();
        }

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void OpenLevel(int levelId)
    {
        string levelName = "Level" + levelId;
        SceneManager.LoadScene(levelName);
        MusicManager.Instance.PlayMusic("Level");
    }

    public void Home()
    {
        SceneManager.LoadScene("mainpage");
    }
}
