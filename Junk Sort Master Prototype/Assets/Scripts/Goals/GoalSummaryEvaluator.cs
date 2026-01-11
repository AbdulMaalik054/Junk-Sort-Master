using System.Linq;


public static class GoalSummaryEvaluator
{
    public static GoalSummaryResult Evaluate(LevelEndReason endReason)
    {
        var allGoals = GoalSystem.ActiveGoals;

        var primaryGoal = allGoals.FirstOrDefault(g => g.IsPrimary);
        var secondaryGoals = allGoals.Where(g => !g.IsPrimary).ToList();

        bool levelSucceeded = EvaluateSuccess(primaryGoal, endReason);
        
        var rewards = SecondaryGoalRewardEvaluator.Evaluate(secondaryGoals);
        int totalBonus = rewards.Sum(r => r.ScoreAwarded);

        return new GoalSummaryResult
        {
            LevelSucceeded = levelSucceeded,
            EndReason = endReason,
            PrimaryGoal = primaryGoal,
            SecondaryGoals = secondaryGoals,

            SecondaryRewards = rewards,
            TotalBonusScore = totalBonus
        };
    }

    private static bool EvaluateSuccess(
        GoalRuntimeData primaryGoal,
        LevelEndReason endReason
    )
    {
        if (primaryGoal == null)
            return false;

        switch (endReason)
        {
            case LevelEndReason.PrimaryGoalCompleted:
            case LevelEndReason.TimeExpired:
                return primaryGoal.IsCompleted;

            case LevelEndReason.PlayerFailed:
            case LevelEndReason.ManualExit:
            default:
                return false;
        }
    }
}
