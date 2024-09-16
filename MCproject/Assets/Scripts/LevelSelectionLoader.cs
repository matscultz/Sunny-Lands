using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionLoader : MonoBehaviour
{
    public void LevelSelectionScene()
    {
        SceneManager.LoadScene("LevelSelection_Try");
    }
}
