using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreCounter;

    private float totalScore = 0;

    public void AddScore(float amount)
    {
        totalScore += amount;
        scoreCounter.text = "Score: " + totalScore;
    }
}
