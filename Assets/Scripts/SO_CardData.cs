using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "NewCardData", menuName = "Card Game/Card Data")]
public class SO_CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public Material material;
}
