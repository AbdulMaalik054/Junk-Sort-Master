using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreCounter;
    public TextMeshProUGUI comboCounter;

    private float totalScore = 0;
    private int currentStreak = 0;
    private int currentMultiplier = 0;

    private DifficultySettings difficulty => GameManager.Instance.ActiveDifficulty;

    public void AddCorrectScore()
    {
        float amount = difficulty.baseScore * currentMultiplier;
        
        totalScore += amount;

        UpdateScoreUI();
        Debug.Log($"Score Added: {amount} x{currentMultiplier} = {amount}");
    }

    public void AddWrongPenalty()
    {
        totalScore += difficulty.wrongPenalty;

        Debug.Log($"Wrong Penalty: {difficulty.wrongPenalty}");

        UpdateScoreUI();
    }

    public void AddOverflowPenalty()
    {
        totalScore += difficulty.overflowPenalty;

        Debug.Log($"Overflow Penalty: {difficulty.overflowPenalty}");

        UpdateScoreUI();
    }
    public void IncreaseStreak()
    {
        currentStreak++;
        UpdateMultiplier();
        Debug.Log("Streak " + currentStreak);
    }

    public void ResetStreak()
    {
        currentStreak = 0;
        UpdateMultiplier();
        Debug.Log("Streak Reset");
    }


    private void UpdateMultiplier()
    {
        if (currentStreak >= 8) currentMultiplier = 4;
        else if (currentStreak >= 5) currentMultiplier = 3;
        else if (currentStreak >= 3) currentMultiplier = 2;
        else currentMultiplier = 1;

        Debug.Log($"Streak: {currentStreak} | Multiplier x{currentMultiplier}");
        comboCounter.text = "Combo: " + currentMultiplier;


    }

    public int GetStreak() => currentStreak; // Another way of writing "return current streak"
    

    public int GetMultiplier() => currentMultiplier;
    

    public void UpdateScoreUI()
    {
    
        scoreCounter.text = "Score: " + totalScore;
    }

}
