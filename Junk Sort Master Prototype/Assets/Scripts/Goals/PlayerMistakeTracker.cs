using System.Linq;
using UnityEngine;

public static class PlayerMistakeTracker
{
    private const int NoMistakeGoalId = 11; // database-authored ID

    public static int TotalMistakes { get; private set; }

    public static void Reset()
    {
        TotalMistakes = 0;
    }

    public static void RegisterMistake()
    {
        TotalMistakes++;

        Debug.Log($"[Mistake] Registered. Total mistakes: {TotalMistakes}");

        InvalidateNoMistakeGoal();
    }

    private static void InvalidateNoMistakeGoal()
    {
        var goal = GoalSystem.ActiveGoals
            .FirstOrDefault(g => g.Id == NoMistakeGoalId);

        if (goal == null)
        {
            Debug.Log("[MistakeGoal] Goal not found");
            return;
        }

        Debug.Log($"[MistakeGoal] Found goal. CurrentValue={goal.CurrentValue}, Target={goal.TargetValue}, IsCompleted={goal.IsCompleted}");

        if (!goal.IsCompleted)
        {
            Debug.Log("[MistakeGoal] Goal already failed — skipping invalidation");
            return;
        }

        GoalSystem.ReportProgress(goal.Id, 0);
        Debug.Log("[GoalSystem] No-Mistake goal invalidated");
    }

}
