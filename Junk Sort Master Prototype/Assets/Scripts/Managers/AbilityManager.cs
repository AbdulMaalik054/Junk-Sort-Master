using UnityEngine;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    [Header("References")]
    [SerializeField] private List<AbilityController> laneAbilities = new();
    [SerializeField] private List<AbilityButton> uiButtons = new(); // Link your 4 UI buttons here

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Sync UI state every frame (or you can use Events for better performance)
        for (int i = 0; i < laneAbilities.Count; i++)
        {
            if (i < uiButtons.Count)
            {
                uiButtons[i].RefreshUI(
                    laneAbilities[i].currentState,
                    laneAbilities[i].GetActiveProgress());
            }
        }
    }
    // Inside AbilityManager.cs
    public void RefreshAllButtons()
    {
        for (int i = 0; i < laneAbilities.Count; i++)
        {
            if (i < uiButtons.Count)
                uiButtons[i].RefreshUI(laneAbilities[i].currentState);
        }
    }

    // You can still keep Update() but remove the logic, 
    // or just call RefreshAllButtons once in Start.
    public AbilityController GetAbilityForLane(int lane)
    {
        if (lane < 0 || lane >= laneAbilities.Count) return null;
        return laneAbilities[lane];
    }
}