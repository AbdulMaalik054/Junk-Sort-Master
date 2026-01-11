using UnityEngine;

public class UIStateActivation : MonoBehaviour
{


    public void ShowGoalUI()
    {
        GoalUIController.Instance.SetState(GoalUIState.InGame);
    }

    
}