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
    public event Action OnResetIndicators;
    public event Action<int> OnLaneReset;



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
        OnGlobalRepaired?.Invoke();
        if (penaltyThresholds == null || penaltyThresholds.Length != laneCount)
        {
            penaltyThresholds = new int[laneCount];
            for (int i = 0; i < laneCount; i++)
                penaltyThresholds[i] = (i == mainConveyorIndex) ? 5 : 3;
        }
        OnResetIndicators?.Invoke(); // <-- Ensures bulbs off when game restarts
        
        for (int i = 0; i < laneCount; i++)
            OnLaneReset?.Invoke(i); // Reset per-lane tracking
    }

    private void HandleLocalBreakdown(int laneIndex)
    {
        
        UIManager.Instance.SpawnRepairButton(laneIndex);
        
    }

    private void HandleLocalRepaired()
    {
        UIManager.Instance.RemoveRepairButton();
    }
    public void AddPenalty(int laneIndex)
    {
        if (laneBroken[laneIndex]) return;

        currentPenalties[laneIndex]++;
        Debug.Log($"Adding penalty to lane {laneIndex}. Current: {currentPenalties[laneIndex]}");


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
        

    }

    public void RepairLane(int laneIndex)
    {
        

        laneBroken[laneIndex] = false;
        currentPenalties[laneIndex] = 0;

        OnLocalRepaired?.Invoke();
        OnResetIndicators?.Invoke(); // <-- NEW
        OnLaneReset?.Invoke(laneIndex); // Notify lane tracker to reset bonus + penalty counts

        bool anyBroken = false;
        foreach (var broken in laneBroken)
            if (broken) { anyBroken = true; break; }

        if (!anyBroken && globalBroken)
        {
            globalBroken = false;
            OnGlobalRepaired?.Invoke();
        }
        
    }
    public bool IsAnyLaneBroken() // optonal helper method
    {
        if (laneBroken == null) return false;
        foreach (var broken in laneBroken)
            if (broken) return true;
        return false;
    }


    public bool IsLaneBroken(int laneIndex) => laneBroken != null && laneBroken[laneIndex];
}
