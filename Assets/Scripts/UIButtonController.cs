using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class UIButtonController : MonoBehaviour
{
    private string saveFilePath;
    public void OnQuitButtonClicked()
    {
        if(SceneManager.GetActiveScene().name == "Main" && !GameManager.Instance.IsGameOver)
        {
            GameData gameData = new GameData();
            gameData.score = ScoreManager.Instance.score;
            gameData.time = (int)GameManager.Instance.gameTime;
            gameData.totalCardsLeft = GameManager.Instance.totalCards;
            gameData.comboMultiplier = ScoreManager.Instance.comboMultiplier;
            gameData.cardInfos = new List<CardInfo>();
            
            for(int i = 0; i < GameManager.Instance.Cards.transform.childCount; i++)
            {
                CardInfo cardInfo = new CardInfo();
                cardInfo.xPos = GameManager.Instance.Cards.transform.GetChild(i).position.x;
                cardInfo.yPos = GameManager.Instance.Cards.transform.GetChild(i).position.y;
                cardInfo.SO_Name = GameManager.Instance.Cards.transform.GetChild(i).
                    GetComponent<CardController>().cardData.cardName;
                gameData.cardInfos.Add(cardInfo);
            }

            SaveSystem saveSystem = FindObjectOfType<SaveSystem>();
            saveSystem.SaveGame(gameData);
        }
        
        Application.Quit();
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnContinueButtonClicked()
    {
        saveFilePath = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(saveFilePath))
        {
            // Here we only check if there is a save file, and load it.
            // Data will later be gathered in Main scene.
            SceneManager.LoadScene("Main");
        }
        else
        {
            // Couldn't have time to implement UI for it.
            Debug.LogWarning("Save file not found at " + saveFilePath);
        }
    }
}
