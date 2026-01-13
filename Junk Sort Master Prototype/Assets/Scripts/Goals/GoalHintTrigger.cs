using System;
using System.Collections.Generic;
[Serializable]
public struct GoalHintTrigger
{
    public GoalHintTriggerType TriggerType;

    // Optional qualifiers
    public int GoalId;              // -1 = any goal
    public int Threshold;           // Used for mistakes / counts
    public bool OnlyOnce;           // Show once per session

    public bool Matches(GoalHintContext context)
    {
        switch (TriggerType)
        {
            case GoalHintTriggerType.PrimaryGoalNotCompleted:
                return context.PrimaryGoalFailed;

            case GoalHintTriggerType.SecondaryGoalFailed:
                return GoalId < 0 ||
                       context.FailedSecondaryGoalIds.Contains(GoalId);

            case GoalHintTriggerType.PlayerRepeatedMistake:
                return context.MistakeCount >= Threshold;

            default:
                return false;
        }
    }
}
