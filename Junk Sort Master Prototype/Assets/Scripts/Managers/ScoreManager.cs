using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score { get; private set; }
    private int currentStreak = 0;
    private int currentMultiplier = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ------------------------------------------------
    // RESET
    // ------------------------------------------------
    public void ResetScore()
    {
        Score = 0;
        currentStreak = 0;
        currentMultiplier = 1;

        UIManager.Instance.UpdateScoreUI(Score);
        UIManager.Instance.UpdateComboUI(currentMultiplier);
    }

    // ------------------------------------------------
    // CORRECT SCORE
    // ------------------------------------------------
    public void AddCorrectScore(int baseAmount, Vector3 worldPos)
    {
        int amount = baseAmount * currentMultiplier;

        Score += amount;
        UIManager.Instance.UpdateScoreUI(Score);

        // Floating score
        FloatingTextPool.Instance.SpawnFloatingText("+" + amount, Color.green, worldPos);
        

        IncreaseStreak(worldPos);
    }

    // ------------------------------------------------
    // WRONG SORTING
    // ------------------------------------------------
    public void AddWrongPenalty(int penaltyAmount, Vector3 worldPos)
    {
        Score -= penaltyAmount;
        if (Score < 0) Score = 0;

        UIManager.Instance.UpdateScoreUI(Score);

        // Floating red penalty
        FloatingTextPool.Instance.SpawnFloatingText("-" + penaltyAmount, Color.red, worldPos);

        
        ResetStreak(worldPos);
    }

    public void AddOverflowPenalty(int penaltyAmount, Vector3 worldPos)
    {
        Score -= penaltyAmount;
        if (Score < 0) Score = 0;

        UIManager.Instance.UpdateScoreUI(Score);

        // Floating red penalty
        FloatingTextPool.Instance.SpawnFloatingText("-" + penaltyAmount, Color.red, worldPos);
        
        ResetStreak(worldPos);
    }

    // ------------------------------------------------
    // STREAK / MULTIPLIER
    // ------------------------------------------------
    private void IncreaseStreak(Vector3 worldPos)
    {
        currentStreak++;
        UpdateMultiplier(worldPos);
    }

    private void ResetStreak(Vector3 worldPos)
    {
        currentStreak = 0;
        UpdateMultiplier(worldPos);
        Vector3 offset = new Vector3(0 , 1 , 0);

        FloatingTextPool.Instance.SpawnFloatingText("Combo Broken", Color.red, worldPos + offset);
    }

    // ------------------------------------------------
    // BONUS / EXTERNAL SCORE (Goals, Rewards, etc.)
    // ------------------------------------------------
    public void AddExternalScore(int amount)
    {
        if (amount <= 0)
            return;

        Score += amount;
        UIManager.Instance.UpdateScoreUI(Score);
    }


    private void UpdateMultiplier(Vector3 worldPos)
    {
        int previousMultiplier = currentMultiplier;

        if (currentStreak >= 8) currentMultiplier = 4;
        else if (currentStreak >= 5) currentMultiplier = 3;
        else if (currentStreak >= 3) currentMultiplier = 2;
        else currentMultiplier = 1;

        UIManager.Instance.UpdateComboUI(currentMultiplier);

        // Combo popup ONLY IF multiplier increased
        if (currentMultiplier > 1 && currentMultiplier != previousMultiplier)
        {
            FloatingTextPool.Instance.SpawnFloatingText("Combo x" + currentMultiplier, Color.yellow, worldPos);
        }
    }

    // Accessors
    public int GetStreak() => currentStreak;
    public int GetMultiplier() => currentMultiplier;
}
