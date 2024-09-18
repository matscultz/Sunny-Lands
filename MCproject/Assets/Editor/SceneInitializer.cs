using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class SceneInitializer
{
    static SceneInitializer()
    {
        // Aggiungi un listener per l'evento di apertura della scena
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        // Verifica se la scena è vuota
        if (IsSceneEmpty(scene))
        {
            // Aggiungi i prefab desiderati
            AddPrefabsToScene();
        }
    }

    private static bool IsSceneEmpty(Scene scene)
    {
        // Controlla se la scena contiene almeno un GameObject
        return scene.GetRootGameObjects().Length <= 1;
    }

    private static void AddPrefabsToScene()
    {
        // Percorsi dei prefab che desideri aggiungere
        string[] prefabPaths = new string[]
        {
            "Assets/Prefab/Berie.prefab",
            "Assets/Prefab/AudioManager.prefab",
            "Assets/Prefab/GameManager.prefab",
            "Assets/Prefab/UI/DiamondUIManager.prefab",
            "Assets/Prefab/UI/Blackscreen.prefab",
            "Assets/Prefab/UI/UIControlButton/UI_Control_Buttons.prefab",
            "Assets/Prefab/UI/UILevelSummary/UILevelSummary.prefab",
            "Assets/Prefab/UI/UIPlay/UIPlay.prefab"
            // Aggiungi altri prefab se necessario
        };

        foreach (string prefabPath in prefabPaths)
        {
            // Carica il prefab
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                // Istanzia il prefab nella scena
                GameObject instance = Object.Instantiate(prefab);

                // Rinomina l'istanza del prefab
                instance.name = GenerateUniqueName(prefab.name);

                // Posiziona l'istanza (opzionale)
                instance.transform.position = Vector3.zero;
            }
            else
            {
                Debug.LogError($"Prefab non trovato: {prefabPath}");
            }
        }
    }

    private static string GenerateUniqueName(string baseName)
    {
        // Aggiunge un numero unico al nome del prefab per evitare duplicati
        int index = 1;
        string newName = baseName;
        while (GameObject.Find(newName) != null)
        {
            newName = $"{baseName}_{index}";
            index++;
        }
        return newName;
    }
}
