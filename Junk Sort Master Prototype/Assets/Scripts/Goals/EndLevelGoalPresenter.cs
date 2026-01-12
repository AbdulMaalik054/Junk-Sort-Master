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
    [SerializeField] private TextMeshProUGUI totalBonusText;


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

        if (!visible)
            return;

        var result = GoalUIController.Instance.CachedEndLevelResult;

        if (result.HasValue)
            Present(result.Value);
        else
            Debug.LogWarning("[EndLevelGoalPresenter] No cached GoalSummaryResult.");
    }

    private void Present(GoalSummaryResult result)
    {
        ClearSecondaryGoals();

        BindPrimaryGoal(result.PrimaryGoal);

        foreach (var goal in result.SecondaryGoals)
            CreateSecondaryGoal(goal);

        BindTotalBonus(result);
    }

    private void BindTotalBonus(GoalSummaryResult result)
    {
        if (totalBonusText == null)
            return;

        if (result.TotalBonusScore > 0)
        {
            totalBonusText.gameObject.SetActive(true);
            totalBonusText.text = $"BONUS +{result.TotalBonusScore}";
            totalBonusText.color = Color.cyan;
        }
        else
        {
            totalBonusText.gameObject.SetActive(false);
        }
    }


    private void BindPrimaryGoal(GoalRuntimeData goal)
    {
        if (goal == null)
        {
            primaryGoalTitle.text = "No Primary Goal";
            primaryGoalResult.text = "FAILED";
            primaryGoalResult.color = Color.red;
            return;
        }
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
