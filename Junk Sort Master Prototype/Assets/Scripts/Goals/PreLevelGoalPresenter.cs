using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Presents goals in the pre-level screen before gameplay starts.
/// 
/// Characteristics:
/// - Read-only presentation of goals
/// - No live progress updates
/// - Driven exclusively by GoalUIState.PreLevel
/// 
/// This presenter intentionally shares GoalRuntimeData with HUD and End Screen
/// to guarantee consistency across all goal surfaces.
/// </summary>
public class PreLevelGoalPresenter : MonoBehaviour
{
    [Header("Primary Goal")]
    [SerializeField] private TextMeshProUGUI primaryGoalTitle;
    [SerializeField] private TextMeshProUGUI primaryGoalDescription;

    [Header("Secondary Goals")]
    [SerializeField] private Transform secondaryGoalsContainer;
    [SerializeField] private PreLevelSecondaryGoalRow secondaryGoalRowPrefab;

    [Header("Visibility")]
    [SerializeField] private CanvasGroup canvasGroup;

    private readonly List<PreLevelSecondaryGoalRow> secondaryRows = new();

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        SetVisible(false);

    }

    private void Start()
    {
        if (GoalUIController.Instance == null)
        {
            Debug.LogError("Presenter: Controller Instance is NULL!");
            return;
        }
        Debug.Log($"Presenter: Initializing with state {GoalUIController.Instance.CurrentState}");
        HandleUIStateChanged(
            GoalUIController.Instance.CurrentState,
            GoalUIController.Instance.CurrentState
        );
    }

    private void OnEnable()
    {
        // Start a tiny routine to make sure the Controller is ready
        StopAllCoroutines();
        StartCoroutine(WaitAndSubscribe());
    }

    private System.Collections.IEnumerator WaitAndSubscribe()
    {
        // Wait until the Instance exists
        while (GoalUIController.Instance == null)
        {
            yield return null;
        }

        // Unsubscribe first to avoid doubles
        GoalUIController.Instance.OnGoalUIStateChanged -= HandleUIStateChanged;
        GoalUIController.Instance.OnGoalUIStateChanged += HandleUIStateChanged;

        Debug.Log($"<color=green>[Presenter]</color> Successfully subscribed to {GoalUIController.Instance.name}");

        // Sync state immediately in case we missed the transition
        HandleUIStateChanged(GoalUIState.Hidden, GoalUIController.Instance.CurrentState);
    }

    private void OnDisable()
    {
        Debug.Log($"<color=red>[Presenter Case Study]</color> {gameObject.name} was just DISABLED! " +
                  $"Stack Trace: {System.Environment.StackTrace}");

        if (GoalUIController.Instance != null)
            GoalUIController.Instance.OnGoalUIStateChanged -= HandleUIStateChanged;
    }

    private void HandleUIStateChanged(GoalUIState previous, GoalUIState current)
    {
        Debug.Log($"[GoalUI] Presenter received state change: {previous} -> {current}");
        bool isPreLevel = current == GoalUIState.PreLevel;

        SetVisible(isPreLevel);

        if (isPreLevel)
            Refresh();
    }


    private void Refresh()
    {
        ClearSecondaryGoals();

        foreach (var goal in GoalSystem.ActiveGoals)
        {
            if (goal.IsPrimary)
            {
                BindPrimaryGoal(goal);
            }
            else
            {
                CreateSecondaryGoal(goal);
            }
        }
    }

    #region Primary Goal

    private void BindPrimaryGoal(GoalRuntimeData goal)
    {
        primaryGoalTitle.text = goal.DisplayName;
        primaryGoalDescription.text = GetPrimaryGoalDescription(goal);
    }

    private string GetPrimaryGoalDescription(GoalRuntimeData goal)
    {
        return $"Complete {goal.TargetValue} to succeed";
    }

    #endregion

    #region Secondary Goals

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

    #endregion

    #region Visibility

    private void SetVisible(bool visible)
    {
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is MISSING on " + gameObject.name);
            return;
        }

        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
        Debug.Log($"[GoalUI] {gameObject.name} visibility set to {visible}. Alpha is now: {canvasGroup.alpha}");
    }

    #endregion
}


