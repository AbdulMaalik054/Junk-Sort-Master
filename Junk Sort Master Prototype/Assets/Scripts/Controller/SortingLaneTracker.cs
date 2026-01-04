using UnityEngine;

public class SortingLaneTracker : MonoBehaviour
{
    [Header("Lane Index")]
    [Tooltip("Assigned automatically by ConveyorController or inspector")]
    public int physicalLaneIndex = -1;   // 0–4 (used by Breakdown, Bulbs, Conveyor)
    public int playableLaneIndex = -1;   // 0–3 (used by Abilities, Junk, Registry)


    [Header("Lane Tracking")]
    public int penaltyCount = 0;
    public int bonusCount = 0;
    public int streakCount = 0;

    [SerializeField] public int streakThreshold = 5;

    public event System.Action OnLaneUpdated;

    private BreakdownManager breakdown => BreakdownManager.Instance;

    private void OnEnable()
    {
        BreakdownManager.Instance.OnLaneReset += ResetLaneState;
    }

    private void OnDisable()
    {
        if (BreakdownManager.Instance != null)
            BreakdownManager.Instance.OnLaneReset -= ResetLaneState;
    }

    private void ResetLaneState(int laneIndex)
    {
        if (this.physicalLaneIndex != laneIndex) return;

        penaltyCount = 0;
        bonusCount = 0;

        OnLaneUpdated?.Invoke(); // Ensure bulbs refresh visuals as well
    }

    public void RegisterCorrect()
    {
        // Immediate penalty cancellation
        if (penaltyCount > 0)
        {
            penaltyCount--;

            if (physicalLaneIndex >= 0)
                breakdown.RemovePenalty(physicalLaneIndex, 1);

            streakCount = 0; // optional but recommended: break streak on recovery

            Debug.Log(
                $"LaneTracker {gameObject.name} cancelled penalty immediately. Remaining={penaltyCount}, laneIndex={physicalLaneIndex}"
            );
        }
        else
        {
            // Build streak only when lane is clean
            streakCount++;

            if (streakCount >= streakThreshold)
            {
                bonusCount++;
                streakCount = 0;

                Debug.Log(
                    $"LaneTracker {gameObject.name} earned bonus. Total bonuses={bonusCount}, laneIndex={physicalLaneIndex}"
                );
            }
        }

        OnLaneUpdated?.Invoke();
    }



    public void RegisterWrong()
    {
        penaltyCount++;
        streakCount = 0;

        if (physicalLaneIndex >= 0)
            breakdown.AddPenalty(physicalLaneIndex);

        OnLaneUpdated?.Invoke();
    }

    public void RegisterOverflow()
    {
        RegisterWrong();
    }

    public void ApplyStreakBonus()
    {
        int bonusCycles = streakCount / streakThreshold;
        streakCount %= streakThreshold;

        for (int i = 0; i < bonusCycles; i++)
        {
            if (penaltyCount > 0)
            {
                penaltyCount--;
                
            }
            else
            {
                bonusCount++;
            }
        }

        OnLaneUpdated?.Invoke();
    }

    public void ResetBonus()
    {
        bonusCount = 0;
        OnLaneUpdated?.Invoke();
    }
}
