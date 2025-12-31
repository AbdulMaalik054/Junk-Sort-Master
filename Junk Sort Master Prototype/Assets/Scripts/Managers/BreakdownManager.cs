using System;
using UnityEngine;

public class BreakdownManager : MonoBehaviour
{
    public static BreakdownManager Instance;

    [Header("Main Lane")]
    [Tooltip("Which lane has the longer threshold")]
    public int mainConveyorIndex = 0;

    [Header("Penalty Thresholds")]
    public int[] penaltyThresholds;

    private int[] currentPenalties;
    private bool[] laneBroken;
    private bool globalBroken;

    public event Action<int> OnLocalBreakdown;
    public event Action OnLocalRepaired;
    public event Action OnGlobalBreakdown;
    public event Action OnGlobalRepaired;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        OnLocalBreakdown += HandleLocalBreakdown;
        OnLocalRepaired += HandleLocalRepaired;
    }

    public void Initialize(int laneCount)
    {
        currentPenalties = new int[laneCount];
        laneBroken = new bool[laneCount];
        globalBroken = false;

        if (penaltyThresholds == null || penaltyThresholds.Length != laneCount)
        {
            penaltyThresholds = new int[laneCount];
            for (int i = 0; i < laneCount; i++)
                penaltyThresholds[i] = (i == mainConveyorIndex) ? 5 : 3;
        }
        Debug.Log($"BreakdownManager initialized with {laneCount} lanes.");
    }

    private void HandleLocalBreakdown(int laneIndex)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(GameManager.Instance.laneControllers[laneIndex].transform.position);
        UIManager.Instance.SpawnRepairButton(screenPos, laneIndex);
        
    }

    private void HandleLocalRepaired()
    {
        UIManager.Instance.RemoveRepairButton();
    }
    public void AddPenalty(int laneIndex)
    {
        if (laneBroken[laneIndex]) return;

        currentPenalties[laneIndex]++;
        Debug.Log($"Lane {laneIndex} penalty added. Current: {currentPenalties[laneIndex]} / {penaltyThresholds[laneIndex]}");

        if (currentPenalties[laneIndex] >= penaltyThresholds[laneIndex])
            TriggerLocalBreakdown(laneIndex);
    }

    private void TriggerLocalBreakdown(int laneIndex)
    {
        laneBroken[laneIndex] = true;
        OnLocalBreakdown?.Invoke(laneIndex);

        if (!globalBroken)
        {
            globalBroken = true;
            OnGlobalBreakdown?.Invoke();
        }
        Debug.Log($"BREAKDOWN DEBUG | laneIndex={laneIndex} | marking laneBroken true");

    }

    public void RepairLane(int laneIndex)
    {
        

        laneBroken[laneIndex] = false;
        currentPenalties[laneIndex] = 0;
        OnLocalRepaired?.Invoke();

        bool anyBroken = false;
        foreach (var broken in laneBroken)
            if (broken) { anyBroken = true; break; }

        if (!anyBroken && globalBroken)
        {
            globalBroken = false;
            OnGlobalRepaired?.Invoke();
        }
        if (laneBroken == null || laneIndex < 0 || laneIndex >= laneBroken.Length)
        {
            Debug.LogWarning($"Invalid lane index {laneIndex} or laneBroken array not initialized.");
            return;
        }

        Debug.Log($"RepairLane called on lane {laneIndex}. Broken? {laneBroken[laneIndex]}");
        if (!laneBroken[laneIndex])
        {
            Debug.Log($"Lane {laneIndex} is not broken, nothing to repair.");
            return;
        }
        Debug.Log($"REPAIR DEBUG | laneIndex={laneIndex} | laneBroken={(laneBroken != null ? laneBroken[laneIndex].ToString() : "NULL")} | currentPenalties={currentPenalties[laneIndex]}");
        Debug.Log($"Lane {laneIndex} repaired.");
    }

    public bool IsLaneBroken(int laneIndex) => laneBroken != null && laneBroken[laneIndex];
}
