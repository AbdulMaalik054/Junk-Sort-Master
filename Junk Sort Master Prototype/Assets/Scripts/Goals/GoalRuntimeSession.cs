using UnityEngine;

public static class GoalRuntimeSession
{
    public static void ResetAll()
    {
        GoalSystem.Reset();
        PlayerMistakeTracker.Reset();
        GoalHintEvaluator.ResetSession();

        Debug.Log("[GoalSession] Runtime goal session reset");
    }
}
