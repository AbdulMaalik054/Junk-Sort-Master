using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Presents live goal progress on the in-game HUD.
/// 
/// This presenter:
/// - Listens to GoalUIState changes
/// - Displays primary goals prominently
/// - Displays secondary goals in a compact form
/// - Reacts to goal progress events (push-based, not polled)
/// 
/// It does NOT:
/// - Evaluate goal completion
/// - Decide UI phase transitions
/// - Show or hide global panels
/// </summary>
public class GoalHUDPresenter : MonoBehaviour
{
    [Header("Primary Goal UI")]
    [SerializeField] private TextMeshProUGUI primaryGoalTitle;
    [SerializeField] private TextMeshProUGUI primaryGoalProgress;

    [Header("Secondary Goals UI")]
    [SerializeField] private Transform secondaryGoalsContainer;
    [SerializeField] private SecondaryGoalRow secondaryGoalRowPrefab;

    [SerializeField] private CanvasGroup canvasGroup;
    private readonly Dictionary<int, SecondaryGoalRow> secondaryGoalRows = new();

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        if (GoalUIController.Instance != null)
            GoalUIController.Instance.OnGoalUIStateChanged += HandleUIStateChanged;

        GoalSystem.OnGoalProgressUpdated += HandleGoalProgressUpdated;
    }

    private void OnDisable()
    {
        if (GoalUIController.Instance != null)
            GoalUIController.Instance.OnGoalUIStateChanged -= HandleUIStateChanged;

        GoalSystem.OnGoalProgressUpdated -= HandleGoalProgressUpdated;
    }

    private void HandleUIStateChanged(GoalUIState previous, GoalUIState current)
    {
        SetVisible(current == GoalUIState.InGame);

        if (current == GoalUIState.InGame)
            RefreshAll();
    }

    /// <summary>
    /// Called whenever any goal reports progress.
    /// </summary>
    private void HandleGoalProgressUpdated(GoalRuntimeData goal)
    {
        if (!GoalUIController.Instance.CurrentState.AllowsProgressUpdates())
            return;

        if (goal.IsPrimary)
            UpdatePrimaryGoal(goal);
        else
            UpdateSecondaryGoal(goal);
    }

    private void RefreshAll()
    {
        ClearSecondaryGoals();

        foreach (var goal in GoalSystem.ActiveGoals)
        {
            if (goal.IsPrimary)
                UpdatePrimaryGoal(goal);
            else
                CreateOrUpdateSecondaryGoal(goal);
        }
    }
    private void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
    #region Primary Goal

    private void UpdatePrimaryGoal(GoalRuntimeData goal)
    {
        primaryGoalTitle.text = goal.DisplayName;
        primaryGoalProgress.text = goal.GetProgressText();
    }

    #endregion

    #region Secondary Goals

    private void CreateOrUpdateSecondaryGoal(GoalRuntimeData goal)
    {
        if (!secondaryGoalRows.TryGetValue(goal.Id, out var row))
        {
            row = Instantiate(secondaryGoalRowPrefab, secondaryGoalsContainer);
            secondaryGoalRows.Add(goal.Id, row);
        }

        row.Bind(goal);
    }

    private void UpdateSecondaryGoal(GoalRuntimeData goal)
    {
        if (secondaryGoalRows.TryGetValue(goal.Id, out var row))
            row.UpdateProgress(goal);
    }

    private void ClearSecondaryGoals()
    {
        foreach (var row in secondaryGoalRows.Values)
            Destroy(row.gameObject);

        secondaryGoalRows.Clear();
    }

    #endregion
}


