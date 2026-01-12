using UnityEngine;

public class LevelGoalBootstrapper : MonoBehaviour
{
    private void Start()
    {
        
        SeedGoals();
    }

    private void SeedGoals()
    {
        GoalSystem.RegisterGoals(new[]
        {
            new GoalRuntimeData
            {
                Id = 1,
                IsPrimary = true,
                DisplayName = "Sort 10 Items",
                CurrentValue = 0,
                TargetValue = 10
            },
            new GoalRuntimeData
            {
                Id = 2,
                IsPrimary = false,
                DisplayName = "No Mistakes",
                CurrentValue = 1,
                TargetValue = 1,
                RewardType = SecondaryRewardType.ScoreBonus,
                BonusScore = 500
            },
            new GoalRuntimeData
            {
                Id = 3,
                IsPrimary = false,
                DisplayName = "Trigger One Ability",
                CurrentValue = 1,
                TargetValue = 1,
                RewardType = SecondaryRewardType.ScoreBonus,
                BonusScore = 500
            }
        });
    var result = GoalSummaryEvaluator.Evaluate(LevelEndReason.TimeExpired);

    Debug.Log($"SUCCESS: {result.LevelSucceeded}");
    Debug.Log($"PRIMARY COMPLETE: {result.PrimaryGoal.IsCompleted}");
    Debug.Log($"SECONDARY COUNT: {result.SecondaryGoals.Count}");
    }
}
