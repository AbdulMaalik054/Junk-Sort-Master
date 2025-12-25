using UnityEngine;

public class JunkBin : MonoBehaviour
{
    public JunkType correctType;
    public SortingLaneTracker laneTracker;

    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponentInChildren<JunkItem>();
        if (junk == null || junk.IsResolved) return;

        if (laneTracker == null)
        {
            Debug.LogError($"{name}: Missing SortingLaneTracker reference!");
            return;
        }

        bool isCorrect = (junk.junkType == correctType);

        SortingResolver.Instance.Resolve(
            isCorrect ? SortResult.Correct : SortResult.Wrong,
            junk,
            laneTracker
        );
    }
}
