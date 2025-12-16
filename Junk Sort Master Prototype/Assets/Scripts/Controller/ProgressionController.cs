using UnityEngine;

public class ProgressionController : MonoBehaviour
{
    public static ProgressionController Instance;

    [SerializeField] private DifficultyConfigDatabase configDatabase;
    [SerializeField] private ConveyorController conveyorController;

    private DifficultyTierConfig activeConfig;
    private int currentTierIndex;
    private float elapsedTime;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ---------- OLD API (DO NOT REMOVE) ----------
    public void StartProgression()
    {
        Initialize(GameManager.Instance.currentDifficulty);
    }

    public void StopProgression()
    {
        enabled = false;
    }
    // --------------------------------------------

    public void Initialize(Difficulty difficulty)
    {
        elapsedTime = 0f;
        currentTierIndex = 0;

        activeConfig = configDatabase.GetConfig(difficulty);

        if (activeConfig == null || activeConfig.tiers == null || activeConfig.tiers.Length == 0)
        {
            Debug.LogError("ProgressionController: Invalid tier config");
            enabled = false;
            return;
        }

        ApplyTier(0);
        enabled = true;
    }

    private void Update()
    {
        // Guard against Update running before Initialize or when config is invalid
        if (activeConfig == null || activeConfig.tiers == null || activeConfig.tiers.Length == 0)
            return;

        elapsedTime += Time.deltaTime;

        if (currentTierIndex >= activeConfig.tiers.Length - 1)
            return;

        var nextIndex = currentTierIndex + 1;
        var nextTier = activeConfig.tiers[nextIndex];
        if (nextTier == null)
            return;

        if (elapsedTime >= nextTier.unlockTime)
        {
            ApplyTier(nextIndex);
        }
    }

    public void ApplyTier(int index)
    {
        // Validate before applying
        if (activeConfig == null || activeConfig.tiers == null ||
            index < 0 || index >= activeConfig.tiers.Length)
        {
            Debug.LogError($"ProgressionController: Cannot apply tier {index} - invalid config or index");
            return;
        }

        currentTierIndex = index;
        var tier = activeConfig.tiers[index];

        conveyorController.SetDifficulty(
            tier.startSpeed,
            tier.maxSpeed,
            tier.acceleration
        );

        Spawner.Instance.SetSpawnRates(tier.spawnInterval);
    }
}
