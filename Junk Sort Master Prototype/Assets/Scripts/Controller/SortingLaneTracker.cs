using UnityEngine;

public class SortingLaneTracker : MonoBehaviour
{
    [Header("Lane Tracking")]
    public int penaltyCount = 0;
    public int bonusCount = 0; // NEW — visible green indicators
    public int streakCount = 0;

    [SerializeField] public int streakThreshold = 5;

    public event System.Action OnLaneUpdated;

    public void RegisterCorrect()
    {
        streakCount++;

        Debug.Log($"Correct | Streak: {streakCount} | Penalty: {penaltyCount} | Bonus: {bonusCount}");

        if (streakCount >= streakThreshold)
            ApplyStreakBonus();

        OnLaneUpdated?.Invoke();
    }

    public void RegisterWrong()
    {
        penaltyCount++;
        streakCount = 0;

        Debug.Log($"Wrong | Streak reset | Penalty: {penaltyCount} | Bonus: {bonusCount}");

        OnLaneUpdated?.Invoke();
    }

    public void RegisterOverflow()
    {
        RegisterWrong();
        Debug.Log($"{name} | Overflow");
    }

    private void ApplyStreakBonus()
    {
        int bonusCycles = streakCount / streakThreshold;
        streakCount %= streakThreshold; // keep leftover streak

        for (int i = 0; i < bonusCycles; i++)
        {
            if (penaltyCount > 0)
            {
                penaltyCount--;
                Debug.Log($"{name} | Penalty removed by bonus!");
            }
            else
            {
                bonusCount++;
                Debug.Log($"{name} | Bonus bulb earned! BonusCount: {bonusCount}");
            }
        }
    }
}
