using UnityEngine;
using System.Collections.Generic;

public class SortingRegistry : MonoBehaviour
{
    public static SortingRegistry Instance { get; private set; }

    [SerializeField] private List<JunkBin> binsByLane = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public JunkBin GetBinForLane(int laneIndex)
    {
        if (laneIndex < 0 || laneIndex >= binsByLane.Count)
            return null;

        return binsByLane[laneIndex];
    }
    public JunkBin GetBinForItem(JunkItem item)
    {
        if (item == null) return null;

        SortingLaneTracker tracker =
            item.GetComponent<SortingLaneTracker>();

        if (tracker == null) return null;

        return GetBinForLane(tracker.playableLaneIndex);
    }
}
