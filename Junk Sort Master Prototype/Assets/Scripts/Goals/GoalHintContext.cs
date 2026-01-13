using System.Collections.Generic;

public struct GoalHintContext
{
    public bool PrimaryGoalFailed;
    public HashSet<int> FailedSecondaryGoalIds;
    public int MistakeCount;
}
