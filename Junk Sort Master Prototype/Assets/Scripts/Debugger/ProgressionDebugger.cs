using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// On-screen monitor for difficulty progression stats.
/// Toggle with F1.
/// Purely for debugging and tuning.
/// </summary>
public class ProgressionDebugger : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI debugText; // Assign a TMP or legacy Text component

    [Header("Toggle Key")]
    public KeyCode toggleKey = KeyCode.F1;

    private bool showDebug = true;

    private void Update()
    {
        // Toggle debugger
        if (Input.GetKeyDown(toggleKey))
            showDebug = !showDebug;

        if (!showDebug)
        {
            if (debugText != null)
                debugText.gameObject.SetActive(false);
            return;
        }

        if (debugText != null)
            debugText.gameObject.SetActive(true);

        // Gather data
        float elapsed = 0f;
        int tierIndex = 0;
        float conveyorSpeed = 0f;
        float spawnInterval = 0f;

        var progression = ProgressionController.Instance;
        if (progression != null)
        {
            // Use reflection-free access
            var activeConfigField = typeof(ProgressionController)
                .GetField("activeConfig", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var currentTierIndexField = typeof(ProgressionController)
                .GetField("currentTierIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var elapsedTimeField = typeof(ProgressionController)
                .GetField("elapsedTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var config = activeConfigField.GetValue(progression) as DifficultyTierConfig;
            tierIndex = (int)currentTierIndexField.GetValue(progression);
            elapsed = (float)elapsedTimeField.GetValue(progression);

            if (config != null && config.tiers.Length > 0)
            {
                tierIndex = Mathf.Clamp(tierIndex, 0, config.tiers.Length - 1);
                var tier = config.tiers[tierIndex];
                conveyorSpeed = tier.maxSpeed;
                spawnInterval = tier.spawnInterval;
            }
        }

        // Update UI
        debugText.text =
            $"Elapsed Time: {elapsed:0.0}s\n" +
            $"Current Tier: {tierIndex}\n" +
            $"Conveyor Speed: {conveyorSpeed:0.00}\n" +
            $"Spawn Interval: {spawnInterval:0.00}s";
    }
}
