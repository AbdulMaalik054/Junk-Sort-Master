using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Goals/Goal Hint Database")]
public class GoalHintDatabase : ScriptableObject
{
    public List<GoalHint> AllHints = new List<GoalHint>();

    // Singleton reference
    private static GoalHintDatabase _instance;

    /// <summary>
    /// Central database reference. Must be assigned in Inspector or loaded from Resources.
    /// </summary>
    public static GoalHintDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find in Resources
                _instance = Resources.Load<GoalHintDatabase>("Goals/GoalHintDatabase");

                if (_instance == null)
                {
                    Debug.LogError("[GoalHintDatabase] Could not find database in Resources/Goals! Please assign manually.");
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Allows manual assignment (e.g., in bootstrapper)
    /// </summary>
    public static void SetInstance(GoalHintDatabase db)
    {
        _instance = db;
    }
}
