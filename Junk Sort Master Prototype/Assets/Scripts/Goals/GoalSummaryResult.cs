using System.Collections.Generic;

/// <summary>
/// Immutable result of evaluating goals at level end.
/// Passed to end-level UI and analytics systems.
/// </summary>
public struct GoalSummaryResult
{
    public bool LevelSucceeded;
    public LevelEndReason EndReason;

    public GoalRuntimeData PrimaryGoal;
    public IReadOnlyList<GoalRuntimeData> SecondaryGoals;

    // NEW
    public IReadOnlyList<SecondaryGoalRewardResult> SecondaryRewards;
    public int TotalBonusScore;
}
