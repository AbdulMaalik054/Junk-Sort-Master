using UnityEngine;
using TMPro;

/// <summary>
/// Static secondary goal row shown on the pre-level screen.
/// </summary>
public class PreLevelSecondaryGoalRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void Bind(GoalRuntimeData goal)
    {
        titleText.text = goal.DisplayName;
        descriptionText.text = $"Bonus: Complete {goal.TargetValue}";
    }
}
