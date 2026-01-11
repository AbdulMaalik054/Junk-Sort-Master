using TMPro;
using UnityEngine;

public class EndLevelSecondaryGoalRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI result;

    public void Bind(GoalRuntimeData goal)
    {
        title.text = goal.DisplayName;
        result.text = goal.IsCompleted ? "COMPLETED" : "FAILED";
        result.color = goal.IsCompleted ? Color.green : Color.red;
    }
}
