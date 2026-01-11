using System;
using UnityEngine;

/// <summary>
/// High-level UI states that drive visibility, layout, and copy
/// for all goal-related UI surfaces (pre-level, HUD, end screen, tutorials).
/// 
/// This enum is intentionally coarse-grained. Fine-grained behavior
/// should be derived from (GoalType, GoalStatus, GoalPhase) rather than
/// adding new UI states.
/// </summary>
public enum GoalUIState
{
    /// <summary>
    /// No goal UI is visible. Used during scene loads, transitions,
    /// or when gameplay systems are not yet initialized.
    /// </summary>
    Hidden = 0,

    /// <summary>
    /// Pre-level presentation state.
    /// Displays primary goal(s), optional secondary goals,
    /// rewards, and constraints before gameplay starts.
    /// </summary>
    PreLevel = 10,

    /// <summary>
    /// Active gameplay HUD state.
    /// Primary goals are emphasized; secondary goals are compact
    /// and optionally collapsible.
    /// </summary>
    InGame = 20,

    /// <summary>
    /// Temporary overlay state used to teach a goal concept
    /// or introduce a new goal mechanic mid-level.
    /// Gameplay may be paused or slowed depending on design.
    /// </summary>
    TutorialOverlay = 30,

    /// <summary>
    /// Gameplay is paused, but goal progress remains visible.
    /// Typically used when opening pause menus or modal dialogs.
    /// </summary>
    Paused = 40,

    /// <summary>
    /// Level successfully completed.
    /// Displays completion status for all goals, rewards earned,
    /// and progression affordances.
    /// </summary>
    LevelSuccess = 50,

    /// <summary>
    /// Level failed.
    /// Displays unmet primary goals, partial secondary progress,
    /// and retry options.
    /// </summary>
    LevelFailure = 60
}

/// <summary>
/// Utility extensions for GoalUIState to centralize common checks
/// and reduce conditional clutter across UI controllers.
/// </summary>
public static class GoalUIStateExtensions
{
    public static bool IsGameplayState(this GoalUIState state)
    {
        return state == GoalUIState.InGame ||
               state == GoalUIState.TutorialOverlay ||
               state == GoalUIState.Paused;
    }

    public static bool IsEndState(this GoalUIState state)
    {
        return state == GoalUIState.LevelSuccess ||
               state == GoalUIState.LevelFailure;
    }

    public static bool AllowsProgressUpdates(this GoalUIState state)
    {
        // Progress should not tick in pre-level or end states
        return state == GoalUIState.InGame ||
               state == GoalUIState.TutorialOverlay;
    }
}
