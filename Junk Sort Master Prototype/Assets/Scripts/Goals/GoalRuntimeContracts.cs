using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Minimal runtime contract for goal data.
/// This is intentionally lightweight and UI-facing only.
/// Evaluation logic lives elsewhere.
/// </summary>
[Serializable]
public class GoalRuntimeData
{
    public int Id;
    public string DisplayName;
    public bool IsPrimary;
    public bool IsConstraintGoal;
    public int CurrentValue;
    public int TargetValue;
    public int BonusScore;
    

    public bool IsCompleted => CurrentValue >= TargetValue;

    // NEW (optional, ignored for primary goals)
    public SecondaryRewardType RewardType;
    public int RewardValue;
    public string GetProgressText()
    {
        return IsCompleted
            ? "Completed"
            : $"{CurrentValue}/{TargetValue}";
    }
}

/// <summary>
/// Minimal goal system facade exposed to UI presenters.
/// This acts as the single push-based source of truth for goal progress.
/// </summary>
public static class GoalSystem
{
    public static IReadOnlyList<GoalRuntimeData> ActiveGoals => _activeGoals;

    public static event Action<GoalRuntimeData> OnGoalProgressUpdated;

    private static readonly List<GoalRuntimeData> _activeGoals = new();

    public static void RegisterGoals(IEnumerable<GoalRuntimeData> goals)
    {
        _activeGoals.Clear();
        _activeGoals.AddRange(goals);

        foreach (var goal in _activeGoals)
        {
            OnGoalProgressUpdated?.Invoke(goal);
        }
    }

    public static void Reset()
    {
        _activeGoals.Clear();
        // OnGoalProgressUpdated = null; // REMOVE OR COMMENT THIS OUT
        // Clearing the list is enough. Setting the event to null 
        // kills all UI listeners that are still active in the scene.
    }

    public static void ReportProgress(int goalId, int newValue)
    {
        for (int i = 0; i < _activeGoals.Count; i++)
        {
            var goal = _activeGoals[i];
            if (goal.Id != goalId)
                continue;

            goal.CurrentValue = Mathf.Clamp(newValue, 0, goal.TargetValue);
            OnGoalProgressUpdated?.Invoke(goal);
            return;
        }
    }
}
