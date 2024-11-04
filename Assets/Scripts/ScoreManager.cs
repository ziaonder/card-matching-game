using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;
    public float comboMultiplier = 1f;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnMatch += SetComboMultiplier;
    }

    private void OnDisable()
    {
        GameManager.OnMatch -= SetComboMultiplier;
    }

    private void Start()
    {
        scoreText.text = "Score \n" + score;
    }

    // These ints are redundant, but they are needed to match the signature of the delegate.
    // They are used elsewhere in CardController.
    private void SetComboMultiplier(int a, int b, bool isMatch)
    {
        if (isMatch)
        {
            score += (int)(100 * comboMultiplier);
            comboMultiplier += 0.5f;
            scoreText.text = "Score \n" + score;
        }
        else
        {
            comboMultiplier = 1f;
        }
    }
}
