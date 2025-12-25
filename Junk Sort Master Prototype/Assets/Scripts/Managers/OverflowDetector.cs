using UnityEngine;

public class OverflowDetector : MonoBehaviour
{
    public BulbEmissionController bulb;

    public SortingLaneTracker laneTracker;

    private void Start()
    {
        
        if (bulb != null)
        {
            bulb.SetSmallRed(0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponentInChildren<JunkItem>();
        
        if (junk == null || junk.IsResolved)
            return;

        // Overflow = penalty
        SortingResolver.Instance.Resolve(SortResult.Overflow, junk , laneTracker);
        // Activate red bulb
        bulb.SetSmallRed(20f);
    }
}
