using UnityEngine;

public class OverflowDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponentInChildren<JunkItem>();
        
        if (junk == null || junk.IsResolved)
            return;

        // Overflow = penalty
        SortingResolver.Instance.Resolve(SortResult.Overflow, junk);
    }
}
