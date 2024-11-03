using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataLoader : MonoBehaviour
{
    [HideInInspector] public CardData cardData;

    private void Start()
    {
        StartCoroutine(SetImage());
    }

    private IEnumerator SetImage()
    {
        while (cardData.cardImage == null)
        {
            yield return null;
        }

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cardData.cardImage;
    }
}
