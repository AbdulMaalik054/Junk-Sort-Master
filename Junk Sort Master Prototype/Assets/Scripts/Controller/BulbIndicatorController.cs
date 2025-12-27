using UnityEngine;

public class BulbIndicatorController : MonoBehaviour
{
    [Header("Penalty Bulbs (Red)")]
    public BulbEmissionController[] penaltyBulbs; // optional

    [Header("Bonus Bulbs (Green)")]
    public BulbEmissionController[] bonusBulbs; // optional

    [Header("Sorting Lane Tracker")]
    public SortingLaneTracker laneTracker;

    [Header("Emission Settings")]
    [SerializeField] private float onIntensity = 40f;
    [SerializeField] private float offIntensity = 0f;

    private void OnEnable()
    {
        if (laneTracker != null)
            laneTracker.OnLaneUpdated += UpdateIndicators;

        UpdateIndicators();
    }

    private void OnDisable()
    {
        if (laneTracker != null)
            laneTracker.OnLaneUpdated -= UpdateIndicators;
    }

    private void UpdateIndicators()
    {
        UpdatePenaltyBulbs();
        UpdateBonusBulbs();
    }

    private void UpdatePenaltyBulbs()
    {
        if (penaltyBulbs == null || penaltyBulbs.Length == 0)
            return;

        int penalties = laneTracker != null ? laneTracker.penaltyCount : 0;

        for (int i = 0; i < penaltyBulbs.Length; i++)
        {
            if (penaltyBulbs[i] == null)
                continue;

            float intensity = i < penalties ? onIntensity : offIntensity;
            penaltyBulbs[i].SetSmallRed(intensity);
        }
    }

    private void UpdateBonusBulbs()
    {
        if (bonusBulbs == null || bonusBulbs.Length == 0)
            return;

        int activeBonuses = Mathf.Clamp(laneTracker.bonusCount, 0, bonusBulbs.Length);

        for (int i = 0; i < bonusBulbs.Length; i++)
        {
            if (bonusBulbs[i] == null)
                continue;

            float intensity = i < activeBonuses ? onIntensity : offIntensity;
            bonusBulbs[i].SetSmallGreen(intensity);
            Debug.Log($"GREEN BULB [{i}] → {(i < activeBonuses ? "ON" : "OFF")} | bonusCount: {laneTracker.bonusCount}");
        }
    }

}
