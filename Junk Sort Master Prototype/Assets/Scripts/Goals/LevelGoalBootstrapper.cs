using System.Collections.Generic;
using UnityEngine;

public class LevelGoalBootstrapper : MonoBehaviour
{
    [Header("Databases")]
    [SerializeField] private LevelGoalDatabase goalDatabase;
    [SerializeField] private GoalHintDatabase hintDatabase;

    private void Awake()
    {
        GoalRuntimeSession.ResetAll();

        // Assign hint database if provided
        if (hintDatabase != null)
        {
            GoalHintDatabase.SetInstance(hintDatabase);
        }

        if (GoalHintDatabase.Instance == null)
        {
            Debug.LogError(
                "[LevelGoalBootstrapper] GoalHintDatabase missing. " +
                "Assign in Inspector or place in Resources/Goals."
            );
        }
    }

    private void Start()
    {
        SeedGoals();
        LogInitialGoalState();
    }

    private void SeedGoals()
    {
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
                CurrentValue = def.IsConstraintGoal ? def.TargetValue : 0,
                TargetValue = def.TargetValue,
                BonusScore = def.BonusScore,
                RewardType = def.RewardType
            });

        }

        GoalSystem.RegisterGoals(runtimeGoals);
    }

    private void LogInitialGoalState()
    {
        Debug.Log("[GoalSystem] Goals initialized:");

        foreach (var goal in GoalSystem.ActiveGoals)
        {
            Debug.Log(
                $"- {goal.DisplayName} | " +
                $"Primary: {goal.IsPrimary} | " +
                $"Progress: {goal.GetProgressText()}"
            );
        }
    }
}
