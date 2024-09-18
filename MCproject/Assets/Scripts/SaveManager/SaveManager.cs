using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string saveFilePath = Application.persistentDataPath + "/savefile.json";

    public static void SaveGame(bool[] levelsUnlocked)
    {
        SaveData data = new SaveData();
        data.levelsUnlocked = levelsUnlocked;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    public static bool[] LoadGame(int numberOfLevels)
    {
        bool[] levelsUnlocked = new bool[numberOfLevels];

        // Inizializza il primo livello come sbloccato
        if (numberOfLevels > 0)
        {
            levelsUnlocked[0] = true;
        }

        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data.levelsUnlocked.Length == numberOfLevels)
            {
                // Copia i dati esistenti se presenti
                for (int i = 0; i < numberOfLevels; i++)
                {
                    levelsUnlocked[i] = data.levelsUnlocked[i];
                }
            }
        }

        return levelsUnlocked;
    }
}
