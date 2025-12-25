using UnityEngine;

public class SortingLaneTracker : MonoBehaviour
{
    [Header("Lane Tracking")]
    public int penaltyCount = 0;
    public int streakCount = 0;

    [SerializeField] private int streakThreshold = 5;

    public void RegisterCorrect()
    {
        streakCount++;

        CheckBonus();
        Debug.Log($"{name} | Correct | Streak: {streakCount}, Penalty: {penaltyCount}");
    }

    public void RegisterWrong()
    {
        penaltyCount++;
        streakCount = 0;

        Debug.Log($"{name} | Wrong | Penalty: {penaltyCount}");
    }

    public void RegisterOverflow()
    {
        RegisterWrong();
        Debug.Log($"{name} | Overflow");
    }

    private void CheckBonus()
    {
        if (streakCount < streakThreshold) return;

        int bonusCount = streakCount / streakThreshold;

        for (int i = 0; i < bonusCount; i++)
        {
            ApplyBonus();
        }

        streakCount = streakCount % streakThreshold;
    }

    private void ApplyBonus()
    {
        if (penaltyCount > 0)
        {
            penaltyCount--;
            Debug.Log($"{name} | Bonus Triggered! Penalty Removed");
        }
        else
        {
            Debug.Log($"{name} | Bonus Triggered! No penalty to remove");
        }
    }
}
