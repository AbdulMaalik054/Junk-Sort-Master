using UnityEngine;

/// <summary>
/// Holds tier-based difficulty scaling data for ONE difficulty mode.
/// Pure data container consumed by ProgressionController.
/// </summary>
[CreateAssetMenu(
    fileName = "DifficultyTierConfig",
    menuName = "Game/Difficulty Tier Config",
    order = 1)]
public class DifficultyTierConfig : ScriptableObject
{
    [Header("Difficulty Identity")]
    public Difficulty difficulty;

    [Header("Tier Progression (Ordered by unlockTime)")]
    public TierData[] tiers;
}

[System.Serializable]
public class TierData
{
    [Tooltip("Seconds since game start when this tier becomes active")]
    public float unlockTime;

    [Header("Conveyor")]
    public float startSpeed;
    public float maxSpeed;
    public float acceleration;

    [Header("Spawner")]
    public float spawnInterval;
}
