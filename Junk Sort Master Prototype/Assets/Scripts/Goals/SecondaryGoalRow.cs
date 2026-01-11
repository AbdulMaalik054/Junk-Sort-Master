using TMPro;
using UnityEngine;

/// <summary>
/// Simple row presenter for a secondary goal inside the HUD.
/// </summary>
public class SecondaryGoalRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI progressText;

    private int goalId;

    public void Bind(GoalRuntimeData goal)
    {
        goalId = goal.Id;
        titleText.text = goal.DisplayName;
        UpdateProgress(goal);
    }

    public void UpdateProgress(GoalRuntimeData goal)
    {
        if (goal.Id != goalId)
            return;

        progressText.text = goal.GetProgressText();
    }
}
