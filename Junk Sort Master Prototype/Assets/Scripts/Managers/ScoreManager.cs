using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // ------------------------------
    // SCORE + STREAK
    // ------------------------------
    public int Score { get; private set; }
    private int currentStreak = 0;
    private int currentMultiplier = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ----------------------------------------------------
    // PUBLIC SCORE API
    // ----------------------------------------------------
    public void ResetScore()
    {
        Score = 0;
        currentStreak = 0;
        currentMultiplier = 1;

        UIManager.Instance.UpdateScoreUI(Score);
        UIManager.Instance.UpdateComboUI(currentMultiplier);
    }

    /// <summary>
    /// Adds correct score using multiplier (combo system)
    /// </summary>
    public void AddCorrectScore(int baseAmount)
    {
        int amount = baseAmount * currentMultiplier;

        Score += amount;
        UIManager.Instance.UpdateScoreUI(Score);

        IncreaseStreak();

        Debug.Log($"Correct Score: {baseAmount} x{currentMultiplier} = {amount}");
    }

    /// <summary>
    /// Wrong sorting penalty
    /// </summary>
    public void AddWrongPenalty(int penaltyAmount)
    {
        Score -= penaltyAmount;
        if (Score < 0) Score = 0;

        ResetStreak();

        UIManager.Instance.UpdateScoreUI(Score);
        Debug.Log($"Wrong Penalty: -{penaltyAmount}");
    }

    /// <summary>
    /// Overflow penalty (trash piled up)
    /// </summary>
    public void AddOverflowPenalty(int penaltyAmount)
    {
        Score -= penaltyAmount;
        if (Score < 0) Score = 0;

        ResetStreak();

        UIManager.Instance.UpdateScoreUI(Score);
        Debug.Log($"Overflow Penalty: -{penaltyAmount}");
    }

    // ----------------------------------------------------
    // STREAK + MULTIPLIER SYSTEM
    // ----------------------------------------------------
    private void IncreaseStreak()
    {
        currentStreak++;
        UpdateMultiplier();

        Debug.Log($"Streak Increased: {currentStreak}");
    }

    private void ResetStreak()
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

        // Call UI updates
        UIManager.Instance.UpdateComboUI(currentMultiplier);

        Debug.Log($"Multiplier Updated: x{currentMultiplier}");
    }

    // Public accessors (optional)
    public int GetStreak() => currentStreak;
    public int GetMultiplier() => currentMultiplier;
}
