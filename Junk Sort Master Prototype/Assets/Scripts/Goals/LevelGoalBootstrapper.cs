using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGoalBootstrapper : MonoBehaviour
{
    [Header("Optional: Assign Database Manually")]
    [SerializeField] private GoalHintDatabase hintDatabase;
    [Header("Assign Goal Database ")]
    [SerializeField] private LevelGoalDatabase goalDatabase;

    private void Awake()
    {
        // Assign singleton if manually provided
        if (hintDatabase != null)
        {
            GoalHintDatabase.SetInstance(hintDatabase);
        }

        // Ensure database exists
        if (GoalHintDatabase.Instance == null)
        {
            Debug.LogError("[LevelGoalBootstrapper] GoalHintDatabase not found! Please assign in Inspector or place in Resources/Goals.");
        }

        // Reset session state for all hints
        GoalHintEvaluator.ResetSession();
    }

    private void Start()
    {
        SeedGoals();

        // Subscribe to runtime updates
        GoalSystem.OnGoalProgressUpdated += OnGoalProgressUpdated;

        // Initial evaluation logging
        LogCurrentGoalStatus();
        // TEMP TEST: simulate progress
        Debug.Log("[GoalSystem] Simulating progress update...");
        GoalSystem.ReportProgress(
            GoalSystem.ActiveGoals.First().Id,
            1
        );
    }

    private void OnDestroy()
    {
        GoalSystem.OnGoalProgressUpdated -= OnGoalProgressUpdated;
    }

    private void SeedGoals()
    {
        if (goalDatabase == null)
        {
            Debug.LogWarning("No LevelGoalDatabase assigned, seeding sample goals manually.");
            //SeedManualGoals();
            return;
        }

        var runtimeGoals = new List<GoalRuntimeData>();
        foreach (var def in goalDatabase.Goals)
        {
            runtimeGoals.Add(new GoalRuntimeData
            {
                Id = def.Id,
                DisplayName = def.DisplayName,
                IsPrimary = def.IsPrimary,
                CurrentValue = 0,
                TargetValue = def.TargetValue,
                BonusScore = def.BonusScore,
                RewardType = def.RewardType
            });
        }

        GoalSystem.RegisterGoals(runtimeGoals);
        var result = GoalSummaryEvaluator.Evaluate(LevelEndReason.TimeExpired);

    Debug.Log($"SUCCESS: {result.LevelSucceeded}");
    Debug.Log($"PRIMARY COMPLETE: {result.PrimaryGoal.IsCompleted}");
    Debug.Log($"SECONDARY COUNT: {result.SecondaryGoals.Count}");
    }

    /// <summary>
    /// Called whenever a goal reports progress
    /// </summary>
    private void OnGoalProgressUpdated(GoalRuntimeData goal)
    {
        Debug.Log($"[GoalSystem] Goal Updated: {goal.DisplayName} - Progress: {goal.GetProgressText()}");

        // Optional: log level success dynamically
        bool allPrimaryComplete = GoalSystem.ActiveGoals
            .Where(g => g.IsPrimary)
            .All(g => g.IsCompleted);

        Debug.Log($"[GoalSystem] All primary goals complete? {allPrimaryComplete}");

        var secondaryCompleted = GoalSystem.ActiveGoals
            .Where(g => !g.IsPrimary)
            .Count(g => g.IsCompleted);

        Debug.Log($"[GoalSystem] Secondary goals completed: {secondaryCompleted}/{GoalSystem.ActiveGoals.Count(g => !g.IsPrimary)}");
    }

    /// <summary>
    /// Log current status of all goals at level start
    /// </summary>
    private void LogCurrentGoalStatus()
    {
        Debug.Log("[GoalSystem] Initial Goal Status:");

        foreach (var goal in GoalSystem.ActiveGoals)
        {
            Debug.Log($"- {goal.DisplayName} | Primary: {goal.IsPrimary} | Progress: {goal.GetProgressText()} | Target: {goal.TargetValue}");
        }
    }
}
