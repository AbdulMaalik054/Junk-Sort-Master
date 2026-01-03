using UnityEngine;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    [SerializeField] private List<AbilityController> laneAbilities = new();

    private void Awake()
    {
        Instance = this;
    }

    public AbilityController GetAbilityForLane(int lane)
    {
        if (lane < 0 || lane >= laneAbilities.Count)
            return null;
        Debug.Log($"Getting ability for lane {lane}");
        return laneAbilities[lane];
    }
}
