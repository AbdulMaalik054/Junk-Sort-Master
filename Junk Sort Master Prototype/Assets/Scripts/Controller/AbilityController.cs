using UnityEngine;
using System.Collections;
public class AbilityController : MonoBehaviour
{
    public enum AbilityState { Locked, Ready, Active }
    public AbilityState currentState = AbilityState.Locked;
    [SerializeField] private SortingLaneTracker laneTracker;
    [SerializeField] private BulbIndicatorController bulbIndicator;
    [SerializeField] private float abilityDuration = 5f;

    private Coroutine abilityRoutine;

    public void NotifyBonusChanged(int currentBonus, int maxBonus)
    {
        SetState(currentBonus >= maxBonus
            ? AbilityState.Ready
            : AbilityState.Locked);
    }

    private void SetState(AbilityState newState)
    {
        currentState = newState;

        if (newState == AbilityState.Active)
            bulbIndicator.StartAbilityFlash();
    }

    public void OnAbilityClicked()
    {
        if (currentState != AbilityState.Ready) return;

        if (abilityRoutine != null)
            StopCoroutine(abilityRoutine);

        abilityRoutine = StartCoroutine(AbilityTimer());
    }

    private IEnumerator AbilityTimer()
    {
        SetState(AbilityState.Active);
        yield return new WaitForSeconds(abilityDuration);
        SetState(AbilityState.Locked);
        bulbIndicator.ResetBonus();
        laneTracker.ResetBonus();
    }
}
