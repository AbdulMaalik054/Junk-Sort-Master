using UnityEngine;

/// <summary>
/// Bridges abstract GoalUIState changes to concrete UIManager actions.
/// 
/// This class is intentionally thin and imperative.
/// It translates "what phase are we in" into "which panels animate".
/// 
/// Responsibilities:
/// - Listen to GoalUIController state changes
/// - Call UIManager methods
/// - Apply global side effects (e.g. time scale) where appropriate
/// </summary>
public class GoalUIRouter : MonoBehaviour
{
    private void OnEnable()
    {
        if (GoalUIController.Instance != null)
            GoalUIController.Instance.OnGoalUIStateChanged += HandleGoalUIStateChanged;
    }

    private void OnDisable()
    {
        if (GoalUIController.Instance != null)
            GoalUIController.Instance.OnGoalUIStateChanged -= HandleGoalUIStateChanged;
    }

    private void HandleGoalUIStateChanged(GoalUIState previous, GoalUIState current)
    {
        Debug.Log($"<color=magenta>[Router]</color> Routing state: {current}");
        switch (current)
        {
            case GoalUIState.Hidden:
                HandleHiddenState();
                break;

            case GoalUIState.PreLevel:
                HandlePreLevelState();
                break;

            case GoalUIState.InGame:
                HandleInGameState(previous);
                break;

            case GoalUIState.TutorialOverlay:
                HandleTutorialOverlayState();
                break;

            case GoalUIState.Paused:
                HandlePausedState();
                break;

            case GoalUIState.LevelSuccess:
                HandleLevelSuccessState();
                break;

            case GoalUIState.LevelFailure:
                HandleLevelFailureState();
                break;
        }
    }

    #region State Handlers

    private void HandleHiddenState()
    {
        Time.timeScale = 1f;
    }

    private void HandlePreLevelState()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowStartTransition("Level Start");
    }

    private void HandleInGameState(GoalUIState previous)
    {
        Time.timeScale = 1f;

        // If resuming from pause, simply hide pause UI
        if (previous == GoalUIState.Paused)
        {
            UIManager.Instance.HidePauseMenu();
            return;
        }

        UIManager.Instance.ShowtopBarGroup();
    }

    private void HandleTutorialOverlayState()
    {
        Time.timeScale = 0f;
        // Tutorial UI will be handled by dedicated tutorial presenters
    }

    private void HandlePausedState()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowPauseMenu();
    }

    private void HandleLevelSuccessState()
    {
        Time.timeScale = 0f;
        // Success screen will be driven by a dedicated end screen presenter
    }

    private void HandleLevelFailureState()
    {
        Time.timeScale = 0f;
        // Final score values should be passed in by game flow controller
        UIManager.Instance.ShowGameOver(0, 0);
    }

    #endregion
}
