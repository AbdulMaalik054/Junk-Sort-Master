using TMPro;
using UnityEngine;

public class EndLevelSecondaryGoalRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI bonusText;

    public void Bind(GoalRuntimeData goal)
    {
        titleText.text = goal.DisplayName;

        if (goal.IsCompleted)
        {
            resultText.text = "Complete";
            resultText.color = Color.green;

            if (goal.BonusScore > 0)
            {
                bonusText.gameObject.SetActive(true);
                bonusText.text = $"+{goal.BonusScore}";
                bonusText.color = Color.cyan;
            }
            else
            {
                bonusText.gameObject.SetActive(false);
            }
        }
        else
        {
            resultText.text = "Incomplete";
            resultText.color = Color.red;

            bonusText.gameObject.SetActive(false);
        }
    }
}
