using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int score, time, totalCardsLeft;
    public float comboMultiplier;
    public List<CardInfo> cardInfos;
}

[System.Serializable]
public class CardInfo
{
    public float xPos, yPos;
    public string SO_Name;
}
