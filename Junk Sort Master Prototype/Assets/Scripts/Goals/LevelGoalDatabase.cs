using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Goals/Level Goal Database")]
public class LevelGoalDatabase : ScriptableObject
{
    public List<LevelGoalDefinition> Goals = new List<LevelGoalDefinition>();
}

[System.Serializable]
public class LevelGoalDefinition
{
    public int Id;
    public string DisplayName;
    public bool IsPrimary;
    public int TargetValue;
    public int BonusScore;
    public bool IsConstraintGoal;
    public SecondaryRewardType RewardType;
}
