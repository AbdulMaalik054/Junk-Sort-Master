using UnityEngine;

public class SortingLaneTracker : MonoBehaviour
{
    [Header("Lane Index")]
    [Tooltip("Assigned automatically by ConveyorController or inspector")]
    public int physicalLaneIndex = -1;   // 0–4 (used by Breakdown, Bulbs, Conveyor)
    public int playableLaneIndex = -1;   // 0–3 (used by Abilities, Junk, Registry)


    [Header("Lane Tracking")]
    
    public int bonusCount = 0;
    public int streakCount = 0;

    [SerializeField] public int streakThreshold = 5;

    public event System.Action OnLaneUpdated;

    private BreakdownManager breakdown => BreakdownManager.Instance;

    private void OnEnable()
    {
        if (BreakdownManager.Instance != null)
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

        bonusCount = 0;
        streakCount = 0;

        OnLaneUpdated?.Invoke();
    }


    public void RegisterCorrect()
    {
        if (physicalLaneIndex < 0)
            return;

        int penalties = breakdown.GetPenaltyCount(physicalLaneIndex);

        // Immediate penalty cancellation
        if (penalties > 0)
        {
            breakdown.RemovePenalty(physicalLaneIndex, 1);
            streakCount = 0;

            
        }
        else
        {
            // Build streak only when lane is clean
            streakCount++;

            if (streakCount >= streakThreshold)
            {
                bonusCount++;
                streakCount = 0;

                
                
            }
        }

        OnLaneUpdated?.Invoke();
    }




    public void RegisterWrong()
    {
        
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
        if (physicalLaneIndex < 0)
            return;

        int bonusCycles = streakCount / streakThreshold;
        streakCount %= streakThreshold;

        for (int i = 0; i < bonusCycles; i++)
        {
            int penalties = breakdown.GetPenaltyCount(physicalLaneIndex);

            if (penalties > 0)
            {
                breakdown.RemovePenalty(physicalLaneIndex, 1);
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
