using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Owns and transitions the current GoalUIState.
/// 
/// This controller does NOT directly show/hide concrete Unity panels.
/// That responsibility remains with UIManager.
/// 
/// Instead, GoalUIController:
/// - Tracks the active GoalUIState
/// - Validates state transitions
/// - Broadcasts state changes to goal-related UI presenters (HUD, pre-level, end screen)
/// </summary>
public class GoalUIController : MonoBehaviour
{
    public static GoalUIController Instance;
    public GoalSummaryResult? CachedEndLevelResult { get; private set; }
    [SerializeField]
    private GoalUIState currentState = GoalUIState.Hidden;
    

    public List<GoalHint> CachedHints { get; private set; }

    public GoalUIState CurrentState => currentState;

    /// <summary>
    /// Fired whenever the GoalUIState changes.
    /// Subscribers should update layout, visibility, or copy accordingly.
    /// </summary>
    public event Action<GoalUIState, GoalUIState> OnGoalUIStateChanged;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    /// <summary>
    /// Request a transition to a new GoalUIState.
    /// Illegal or redundant transitions are ignored.
    /// </summary>
    public void SetState(GoalUIState newState)
    {
        Debug.Log($"<color=orange>[GoalUIController]</color> SetState requested: {currentState} -> {newState}");

        if (newState == currentState)
        {
            Debug.Log("<color=yellow>[GoalUIController]</color> Transition ignored: Already in that state.");
            return;
        }

        if (!IsTransitionAllowed(currentState, newState))
        {
            Debug.LogWarning($"<color=red>[GoalUIController]</color> Illegal transition: {currentState} -> {newState}");
            return;
        }

        GoalUIState previousState = currentState;
        currentState = newState;

        Debug.Log($"<color=green>[GoalUIController]</color> Event firing! Subscribers: {OnGoalUIStateChanged?.GetInvocationList().Length}");
        OnGoalUIStateChanged?.Invoke(previousState, currentState);
    }
    public void CacheEndLevelResult(GoalSummaryResult result)
    {
        CachedEndLevelResult = result;

        var context = new GoalHintContext
        {
            PrimaryGoalFailed = !result.LevelSucceeded,
            FailedSecondaryGoalIds = result.SecondaryGoals
                .Where(g => !g.IsCompleted)
                .Select(g => g.Id)
                .ToHashSet(),
            MistakeCount = PlayerMistakeTracker.TotalMistakes
        };

        var db = GoalHintDatabase.Instance;
        if (db == null)
        {
            Debug.LogWarning("[GoalUIController] GoalHintDatabase not found!");
            CachedHints = new List<GoalHint>();
            return;
        }

        CachedHints = GoalHintEvaluator.Evaluate(db.AllHints, context);
    }
    public void EnterMainMenu()
    {
        ClearCachedResults();
        SetState(GoalUIState.Hidden);
    }

    public void EnterPreLevel()
    {
        ClearCachedResults();
        SetState(GoalUIState.PreLevel);
    }

    public void BeginGameplay()
    {
        SetState(GoalUIState.InGame);
    }

    public void OnLevelStart()
    {
        ClearCachedResults();
        SetState(GoalUIState.PreLevel);
    }

    public void ExitToMainMenu()
    {
        Debug.Log("[GoalUIController] Forcing exit to main menu");

        ClearCachedResults();
        currentState = GoalUIState.Hidden;
        OnGoalUIStateChanged?.Invoke(GoalUIState.InGame, GoalUIState.Hidden);
    }

    public void EndLevel(GoalSummaryResult result)
    {
        CachedEndLevelResult = result;
        SetState(result.LevelSucceeded
            ? GoalUIState.LevelSuccess
            : GoalUIState.LevelFailure);
    }
    private void ClearCachedResults()
    {
        CachedEndLevelResult = null;
    }
    

    
    /// <summary>
    /// Centralized transition rules to prevent UI chaos.
    /// Adjust here instead of scattering guards across systems.
    /// </summary>
    private bool IsTransitionAllowed(GoalUIState from, GoalUIState to)
    {
        switch (from)
        {
            case GoalUIState.Hidden:
                return to == GoalUIState.PreLevel || to == GoalUIState.InGame;

            case GoalUIState.PreLevel:
                return to == GoalUIState.InGame || to == GoalUIState.Hidden;

            case GoalUIState.InGame:
                return to == GoalUIState.Paused ||
                       to == GoalUIState.TutorialOverlay ||
                       to == GoalUIState.LevelSuccess ||
                       to == GoalUIState.LevelFailure;

            case GoalUIState.TutorialOverlay:
                return to == GoalUIState.InGame || to == GoalUIState.Paused;

            case GoalUIState.Paused:
                return to == GoalUIState.InGame || to == GoalUIState.Hidden || to == GoalUIState.LevelFailure;

            case GoalUIState.LevelSuccess:
            case GoalUIState.LevelFailure:
                return to == GoalUIState.Hidden;

            default:
                return false;
        }
    }
    


}



