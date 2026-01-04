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
    [SerializeField] private float onIntensity = 20f;
    [SerializeField] private float offIntensity = 0f;

    [Header("Global Bulbs")]
    public BulbEmissionController BigGreenBulb;
    public BulbEmissionController BigRedBulb;

    [Header("Ability Controller")]
    [SerializeField] private AbilityController abilityController;
   


    // ---------------- FLASH STATE (PER LANE, PRIVATE) ----------------

    [Header("Breakdown Flashing Settings")]
    [SerializeField] private float breakdownFlashSpeed = 5f;
    private bool breakdownFlashing = false;
    private float breakdownTimer = 0f;

    [Header("Bonus Flashing Settings")]
    [SerializeField] private float bonusFlashSpeed = 5f;
    private bool bonusFlashing = false;
    private float bonusTimer = 0f;

    //[Header("Main Lane Flashing Settings")]
    //[SerializeField] private float mainLaneFlashSpeed = 5f;
    //private bool mainLaneFlashing = false;
    //private float mainLaneTimer = 0f;
    // ----------------------------------------------------------------

    private void Awake()
    {
        if (BreakdownManager.Instance != null)
            BreakdownManager.Instance.OnResetIndicators += ResetIndicators;

        ResetIndicators();
    }

    private void OnEnable()
    {
        if (laneTracker != null)
            laneTracker.OnLaneUpdated += UpdateIndicators;

        if (BreakdownManager.Instance != null)
        {
            BreakdownManager.Instance.OnLaneReset += HandleLaneReset;
            BreakdownManager.Instance.OnResetIndicators += HandleGlobalReset;
        }
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

    private void Update()
    {
        //FlashBigRed();
        UpdateBreakdownFlash();
        UpdateBonusFlash();
    }

    // ===================== FLASH DRIVERS =====================

    private void UpdateBreakdownFlash()
    {
        if (!breakdownFlashing) return;

        breakdownTimer += Time.deltaTime * breakdownFlashSpeed;
        float t = Mathf.PingPong(breakdownTimer, 1f);
        float intensity = Mathf.Lerp(offIntensity, onIntensity, t);

        foreach (var bulb in penaltyBulbs)
        {
            if (bulb == null) continue;
            bulb.SetSmallRed(intensity);
        }
    }

    private void UpdateBonusFlash()
    {
        if (!bonusFlashing) return;

        bonusTimer += Time.deltaTime * bonusFlashSpeed;
        float t = Mathf.PingPong(bonusTimer, 1f);
        float intensity = Mathf.Lerp(offIntensity, onIntensity, t);

        foreach (var bulb in bonusBulbs)
        {
            if (bulb == null) continue;
            bulb.SetSmallGreen(intensity);
        }
    }

    //public void FlashBigRed()
    //{
    //    if (!mainLaneFlashing) return;

    //    mainLaneTimer += Time.deltaTime * mainLaneFlashSpeed;
    //    float t = Mathf.PingPong(mainLaneTimer, 1f);
    //    float intensity = Mathf.Lerp(offIntensity, onIntensity, t);

    //    if (BigRedBulb != null)
    //            BigRedBulb.SetBigRed(intensity);
    //}


    // ===================== STATIC UPDATES =====================

    private void UpdateIndicators()
    {
        UpdatePenaltyBulbs();
        UpdateBonusBulbs();
    }

    private void UpdatePenaltyBulbs()
    {
        if (breakdownFlashing) return;

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

        int activeBonuses = laneTracker != null
            ? Mathf.Clamp(laneTracker.bonusCount, 0, bonusBulbs.Length)
            : 0;

        for (int i = 0; i < bonusBulbs.Length; i++)
        {
            if (bonusBulbs[i] == null) continue;
            float intensity = i < activeBonuses ? onIntensity : offIntensity;
            bonusBulbs[i].SetSmallGreen(intensity);
        }

        if (abilityController != null && laneTracker != null)
            abilityController.NotifyBonusChanged(laneTracker.bonusCount, bonusBulbs.Length);
    }

    public void UpdateBigGreenBulb(bool isActive)
    {
        if (BigGreenBulb == null) return;
        float intensity = isActive ? onIntensity : offIntensity;
        BigGreenBulb.SetBigGreen(intensity);
    }
    // ===================== PUBLIC API (UNCHANGED) =====================

    public void StartBreakdownFlash()
    {
        breakdownFlashing = true;
        breakdownTimer = 0f;
    }

    public void StopBreakdownFlash()
    {
        breakdownFlashing = false;
        breakdownTimer = 0f;
        UpdatePenaltyBulbs();
    }

    public void StartAbilityFlash()
    {
        bonusFlashing = true;
        bonusTimer = 0f;
    }

    public void StopAbilityFlash()
    {
        bonusFlashing = false;
        bonusTimer = 0f;
    }

    public void ResetBonus()
    {
        
        StopAbilityFlash();
        UpdateBonusBulbs();
    }

    // ===================== RESET HANDLERS =====================

    private void ResetIndicators()
    {
        breakdownFlashing = false;
        bonusFlashing = false;
        breakdownTimer = 0f;
        bonusTimer = 0f;

        foreach (var bulb in penaltyBulbs)
            if (bulb != null)
                bulb.SetSmallRed(offIntensity);

        foreach (var bulb in bonusBulbs)
            if (bulb != null)
                bulb.SetSmallGreen(offIntensity);
    }

    private void HandleLaneReset(int laneIndex)
    {
        if (laneTracker == null) return;
        if (laneTracker.physicalLaneIndex != laneIndex) return;

        ResetIndicators();
        UpdateIndicators();
    }

    private void HandleGlobalReset()
    {
        ResetIndicators();
        UpdateIndicators();
    }
}
