using UnityEngine;

public class OverflowDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponent<JunkItem>();
        Vector3 scoreOffset = new Vector3(-10 , 0 ,0);
        if (junk == null)
            return;

        // Overflow = penalty
        SortingResolver.Instance.Resolve(SortResult.Overflow, junk);
    }
}
