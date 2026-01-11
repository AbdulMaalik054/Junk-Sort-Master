/// <summary>
/// Semantic reason why a level ended.
/// Used by evaluators, analytics, and UI flow.
/// </summary>
public enum LevelEndReason
{
    PrimaryGoalCompleted,
    TimeExpired,
    PlayerFailed,
    ManualExit
}
