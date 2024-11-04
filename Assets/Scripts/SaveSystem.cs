using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        // Define the save path for JSON files
        saveFilePath = Application.persistentDataPath + "/savefile.json";
    }

    public void SaveGame(GameData gameData)
    {
        string jsonData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, jsonData);
    }

    public GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

            return gameData;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + saveFilePath);
            return null;
        }
    }
}
