using UnityEngine;

public class BulbIndicatorController : MonoBehaviour
{
    [Header("Penalty Bulbs (Red)")]
    public BulbEmissionController[] penaltyBulbs;

    [Header("Bonus Bulbs (Green)")]
    public BulbEmissionController[] bonusBulbs;

    [Header("Sorting Lane Tracker")]
    public SortingLaneTracker laneTracker;

    [Header("Emission Settings")]
    [SerializeField] private float onIntensity = 40f;
    [SerializeField] private float offIntensity = 0f;

    [Header("Ability Controller")]
    [SerializeField] private AbilityController abilityController;

    [Header("Bulb Flashing Settings")]
    private bool flashing = false;
    [SerializeField] private float flashSpeed = 5f; // You can tweak
    private void Awake()
    {
        if (laneTracker == null)
            laneTracker = FindAnyObjectByType<SortingLaneTracker>();
        BreakdownManager.Instance.OnResetIndicators += ResetIndicators;

        ResetIndicators();
    }
    private void OnEnable()
    {
        if (laneTracker != null)
            laneTracker.OnLaneUpdated += UpdateIndicators;

        BreakdownManager.Instance.OnLaneReset += HandleLaneReset;
        BreakdownManager.Instance.OnResetIndicators += HandleGlobalReset;

        
    }

    private void OnDisable()
    {
        if (laneTracker != null)
            laneTracker.OnLaneUpdated -= UpdateIndicators;
        if (BreakdownManager.Instance != null)
        {
            BreakdownManager.Instance.OnLaneReset -= HandleLaneReset;
            BreakdownManager.Instance.OnResetIndicators -= HandleGlobalReset;
        }
    }

    private void Start()
    {
        UpdateIndicators();
    }
    private void UpdateIndicators()
    {
        UpdatePenaltyBulbs();
        UpdateBonusBulbs();
    }

    private void UpdatePenaltyBulbs()
    {
        if (penaltyBulbs == null || penaltyBulbs.Length == 0) return;

        int penalties = laneTracker != null ? laneTracker.penaltyCount : 0;

        for (int i = 0; i < penaltyBulbs.Length; i++)
        {
            if (penaltyBulbs[i] == null) continue;

            float intensity = i < penalties ? onIntensity : offIntensity;
            penaltyBulbs[i].SetSmallRed(intensity);
        }
    }

    private void UpdateBonusBulbs()
    {
        if (bonusBulbs == null || bonusBulbs.Length == 0) return;

        int activeBonuses = Mathf.Clamp(laneTracker.bonusCount, 0, bonusBulbs.Length);

        for (int i = 0; i < bonusBulbs.Length; i++)
        {
            if (bonusBulbs[i] == null) continue;

            float intensity = i < activeBonuses ? onIntensity : offIntensity;
            bonusBulbs[i].SetSmallGreen(intensity);
        }
        if (abilityController != null)
        {
            abilityController.NotifyBonusChanged(laneTracker.bonusCount, bonusBulbs.Length);
        }
    }
            


    public void StartAbilityFlash()
    {
        flashing = true;
    }

    private void RefreshBonusBulbs()
    {
        foreach (var bulb in bonusBulbs)
        {
            bulb.SetSmallGreen(0.0f);
        }
    }
    public void StopAbilityFlash()
    {
        flashing = false;

        // Ensure bulbs return to normal emission
        RefreshBonusBulbs();
    }
    private void Update()
    {
        if (flashing)
        {
            // basic emission pulsing for green bulbs
            float emissionPower = (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f;
            UpdateBonusBulbEmission(emissionPower);
        }
    }
    private void UpdateBonusBulbEmission(float intensity)
    {
        // Loop your green bulb emission controllers:
        foreach (var bulb in bonusBulbs)
        {
            float scaledIntensity = Mathf.Lerp(offIntensity, onIntensity, intensity);
            bulb.SetSmallGreen(scaledIntensity);
        }
    }

    public void ResetBonus()
    {
        laneTracker.bonusCount = 0;
        
    }
    private void ResetIndicators()
    {
        foreach (var bulb in penaltyBulbs)
            bulb.SetSmallRed(offIntensity);

        foreach (var bulb in bonusBulbs)
            bulb.SetSmallGreen(offIntensity);
    }
    private void HandleLaneReset(int laneIndex)
    {
        if (laneTracker == null) return;
        if (laneTracker.laneIndex != laneIndex) return;

        ResetIndicators();     // Only reset THIS lane
        UpdateIndicators();
    }

    private void HandleGlobalReset()
    {
        ResetIndicators();     // This runs only on RestartGame
        UpdateIndicators();
    }
}
