using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Script : MonoBehaviour
{
    public void LevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }

}
