using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.IO;

public class FlexibleLayoutSystem : MonoBehaviour
{
    [SerializeField] private SO_CardData[] cards;
    private SO_CardData[] pairedCards;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardHolder;
    [SerializeField] private SO_CardData[] cardData;

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        if(File.Exists(Application.persistentDataPath + "/savefile.json"))
        {
            SetupSavedFile();
        }
        else
            SetCardLayout();
    }

    private void SetupSavedFile()
    {
        SaveSystem saveSystem = FindObjectOfType<SaveSystem>();
        GameData gameData = saveSystem.LoadGame();

        GameManager.Instance.gameTime = gameData.time;
        ScoreManager.Instance.score = gameData.score;
        GameManager.Instance.totalCards = gameData.totalCardsLeft;
        ScoreManager.Instance.comboMultiplier = gameData.comboMultiplier;
        foreach(CardInfo cardInfo in gameData.cardInfos)
        {
            GameObject cardInstance = Instantiate(cardPrefab, new Vector3(cardInfo.xPos, cardInfo.yPos), Quaternion.identity, cardHolder);
            
            foreach(SO_CardData card in cardData)
            {
                if (card.cardName == cardInfo.SO_Name)
                {
                    cardInstance.GetComponent<CardController>().cardData = card;
                    break;
                }
            }
        }
    }

    private void SetCardLayout()
    {
        float screenAspectRatio = (float)Screen.width / Screen.height;

        if (screenAspectRatio >= 1.7f) // Widescreen 
        {
            SetupGrid(5, 4);
        }
        else if (screenAspectRatio >= 1.5f) // Standard landscape (e.g., 16:9)
        {
            SetupGrid(4, 3);
        }
        else if (screenAspectRatio >= 1.3f) // Standard (e.g., 4:3)
        {
            SetupGrid(4, 4); 
        }
        else if (screenAspectRatio > 1.0f) // Square-ish landscape (e.g., 5:4)
        {
            SetupGrid(4, 4); 
        }
        else if (screenAspectRatio <= 1.0f && screenAspectRatio >= 0.5f) // Portrait (9:16 or similar)
        {
            SetupGrid(3, 4); 
        }
        else // Very tall aspect ratio
        {
            SetupGrid(2, 4); 
        }
    }

    private void ShuffleCardList(SO_CardData[] list)
    {
        Random random = new Random();

        for (int i = list.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            SO_CardData temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void SetupGrid(int column, int row)
    {
        pairedCards = new SO_CardData[row * column];
        GameManager.Instance.totalCards = row * column;

        ShuffleCardList(cards); // Shuffle card list to have a variety of cards

        // Pull half the number of cards in the grid capacity from the shuffled list.
        for (int i = 0; i < row * column / 2; i++)
        {
            pairedCards[i] = cards[i];
        }

        // Duplicate the cards to make pairs.
        for (int i = row * column / 2; i < row * column; i++)
        {
            pairedCards[i] = pairedCards[i - row * column / 2];
        }

        // Shuffle the paired cards.
        ShuffleCardList(pairedCards);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                float xOffset = (column % 2 == 0) ? 0.5f : 0f;
                float yOffset = (row % 2 == 0) ? -0.5f : 0f;

                Vector3 pos = new Vector3((j - column / 2 + xOffset) * 1.75f, (row / 2 - i + yOffset) * 2f);
                GameObject cardInstance = Instantiate(cardPrefab, pos, Quaternion.identity, cardHolder);
                cardInstance.GetComponent<CardController>().cardData = pairedCards[i * column + j];
                // This info will later be used for instantiating cards from a saved file.
                cardInstance.GetComponent<CardController>().SetPosition(i, j);
            }
        } 
    }
}
