using UnityEngine;

[CreateAssetMenu(menuName = "Goals/Goal Hint")]
public class GoalHint : ScriptableObject
{
    public string Id;
    [TextArea] public string Message;

    public GoalHintTrigger Trigger;
}
