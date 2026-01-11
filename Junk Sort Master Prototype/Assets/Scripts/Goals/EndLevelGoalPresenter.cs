using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndLevelGoalPresenter : MonoBehaviour
{
    [Header("Primary Goal")]
    [SerializeField] private TextMeshProUGUI primaryGoalTitle;
    [SerializeField] private TextMeshProUGUI primaryGoalResult;

    [Header("Secondary Goals")]
    [SerializeField] private Transform secondaryGoalsContainer;
    [SerializeField] private EndLevelSecondaryGoalRow secondaryGoalRowPrefab;

    [Header("Visibility")]
    [SerializeField] private CanvasGroup canvasGroup;

    private readonly List<EndLevelSecondaryGoalRow> secondaryRows = new();

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        SetVisible(false);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitAndSubscribe());
    }

    private System.Collections.IEnumerator WaitAndSubscribe()
    {
        while (GoalUIController.Instance == null)
            yield return null;

        GoalUIController.Instance.OnGoalUIStateChanged -= HandleUIStateChanged;
        GoalUIController.Instance.OnGoalUIStateChanged += HandleUIStateChanged;

        HandleUIStateChanged(
            GoalUIState.Hidden,
            GoalUIController.Instance.CurrentState
        );
    }

    private void OnDisable()
    {
        if (GoalUIController.Instance != null)
            GoalUIController.Instance.OnGoalUIStateChanged -= HandleUIStateChanged;
    }

    private void HandleUIStateChanged(GoalUIState previous, GoalUIState current)
    {
        bool visible =
            current == GoalUIState.LevelSuccess ||
            current == GoalUIState.LevelFailure;

        SetVisible(visible);

        if (visible)
            Refresh();
    }

    private void Refresh()
    {
        ClearSecondaryGoals();

        foreach (var goal in GoalSystem.ActiveGoals)
        {
            if (goal.IsPrimary)
                BindPrimaryGoal(goal);
            else
                CreateSecondaryGoal(goal);
        }
    }

    private void BindPrimaryGoal(GoalRuntimeData goal)
    {
        primaryGoalTitle.text = goal.DisplayName;
        primaryGoalResult.text = goal.IsCompleted ? "SUCCESS" : "FAILED";
        primaryGoalResult.color = goal.IsCompleted ? Color.green : Color.red;
    }

    private void CreateSecondaryGoal(GoalRuntimeData goal)
    {
        var row = Instantiate(secondaryGoalRowPrefab, secondaryGoalsContainer);
        row.Bind(goal);
        secondaryRows.Add(row);
    }

    private void ClearSecondaryGoals()
    {
        foreach (var row in secondaryRows)
            Destroy(row.gameObject);

        secondaryRows.Clear();
    }

    private void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}
