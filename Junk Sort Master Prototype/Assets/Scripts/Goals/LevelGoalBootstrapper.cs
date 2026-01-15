using System.Collections.Generic;
using UnityEngine;

public class LevelGoalBootstrapper : MonoBehaviour
{
    public static LevelGoalBootstrapper Instance; // Added Singleton

    [Header("Databases")]
    [SerializeField] private LevelGoalDatabase goalDatabase;
    [SerializeField] private GoalHintDatabase hintDatabase;

    private void Awake()
    {
        Instance = this; // Initialize Singleton

        if (hintDatabase != null)
            GoalHintDatabase.SetInstance(hintDatabase);
    }

    // This replaces the Start() logic
    public void InitializeGoalsForLevel()
    {
        Debug.Log("[GoalBootstrapper] Re-seeding goals for new session...");

        // 1. Reset previous session data
        GoalRuntimeSession.ResetAll();

        // 2. Re-seed from Database
        if (goalDatabase == null)
        {
            Debug.LogError("[LevelGoalBootstrapper] LevelGoalDatabase not assigned.");
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
                IsConstraintGoal = def.IsConstraintGoal,
                // Ensure constraint goals start at Target, others at 0
                CurrentValue = def.IsConstraintGoal ? def.TargetValue : 0,
                TargetValue = def.TargetValue,
                BonusScore = def.BonusScore,
                RewardType = def.RewardType
            });
        }

        // 3. Register with System
        GoalSystem.RegisterGoals(runtimeGoals);
        LogInitialGoalState();
    }

    private void LogInitialGoalState()
    {
        foreach (var goal in GoalSystem.ActiveGoals)
        {
            Debug.Log($"- {goal.DisplayName} initialized at {goal.GetProgressText()}");
        }
    }
}