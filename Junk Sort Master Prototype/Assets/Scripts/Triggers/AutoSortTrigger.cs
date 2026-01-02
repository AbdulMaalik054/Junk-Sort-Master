using UnityEngine;

public class AutoSortTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        JunkItem junk = other.GetComponentInChildren<JunkItem>();
        if (junk == null) return;

        SortingLaneTracker tracker = junk.GetComponentInChildren<SortingLaneTracker>();
        if (tracker == null) return;

        AbilityController ability =
            AbilityManager.Instance.GetAbilityForLane(tracker.laneIndex);

        if (ability == null ||
            ability.currentState != AbilityController.AbilityState.Active)
            return;

        JunkBin targetBin = SortingRegistry.Instance.GetBinForLane(tracker.laneIndex);
        if (targetBin != null)
        {
            Debug.Log("Auto-sorting junk of type " + junk.junkType +
                " to bin for lane " + tracker.laneIndex);
            junk.AutoFlyTo(targetBin);
        }
    }

}
