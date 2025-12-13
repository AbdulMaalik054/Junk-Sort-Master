using UnityEngine;

/// <summary>
/// Central lookup table that maps Difficulty enums
/// to their corresponding DifficultyTierConfig assets.
/// Assigned manually via Inspector for safety and clarity.
/// </summary>
[CreateAssetMenu(
    fileName = "DifficultyConfigDatabase",
    menuName = "Game/Difficulty Config Database",
    order = 2)]
public class DifficultyConfigDatabase : ScriptableObject
{
    [Header("Difficulty Configurations")]
    [Tooltip("Tier configuration for Easy difficulty")]
    public DifficultyTierConfig easyConfig;

    [Tooltip("Tier configuration for Medium difficulty")]
    public DifficultyTierConfig mediumConfig;

    [Tooltip("Tier configuration for Hard difficulty")]
    public DifficultyTierConfig hardConfig;

    [Tooltip("Tier configuration for Endless difficulty")]
    public DifficultyTierConfig endlessConfig;

    /// <summary>
    /// Returns the correct config for the given difficulty.
    /// This is the ONLY method ProgressionController should use.
    /// </summary>
    public DifficultyTierConfig GetConfig(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return easyConfig;

            case Difficulty.Medium:
                return mediumConfig;

            case Difficulty.Hard:
                return hardConfig;

            case Difficulty.Endless:
                return endlessConfig;

            default:
                Debug.LogError("DifficultyConfigDatabase: Unknown difficulty!");
                return null;
        }
    }
}
