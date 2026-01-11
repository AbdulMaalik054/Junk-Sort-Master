using System.Collections.Generic;
using System.Linq;

public static class SecondaryGoalRewardEvaluator
{
    public static IReadOnlyList<SecondaryGoalRewardResult> Evaluate(
        IReadOnlyList<GoalRuntimeData> goals
    )
    {
        var results = new List<SecondaryGoalRewardResult>();

        foreach (var goal in goals.Where(g => !g.IsPrimary))
        {
            bool granted = goal.IsCompleted;
            int score = 0;

            if (granted && goal.RewardType == SecondaryRewardType.ScoreBonus)
                score = goal.RewardValue;

            results.Add(new SecondaryGoalRewardResult
            {
                Goal = goal,
                Granted = granted,
                ScoreAwarded = score
            });
        }

        return results;
    }
}
