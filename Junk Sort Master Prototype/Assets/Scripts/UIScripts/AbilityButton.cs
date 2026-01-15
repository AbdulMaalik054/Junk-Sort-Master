using System.Linq;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private int uiLaneIndex;
    private int playableLaneIndex => uiLaneIndex - 1;
    public void OnClick()
    {
        AbilityController ability = AbilityManager.Instance.GetAbilityForLane(playableLaneIndex);


        if (ability != null)
        {
            ability.OnAbilityClicked();
            
        }
    }

    

}


