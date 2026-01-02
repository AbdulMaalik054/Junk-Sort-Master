using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private int laneIndex;

    public void OnClick()
    {
        AbilityController ability =
            AbilityManager.Instance.GetAbilityForLane(laneIndex);
        Debug.Log("AbilityButton clicked for lane " + laneIndex);

        if (ability != null)
            ability.OnAbilityClicked();

    }
}
