using System.Collections;
using System.Linq;
using UnityEngine;
public class AbilityController : MonoBehaviour
{
    public enum AbilityState { Locked, Ready, Active }
    public AbilityState currentState = AbilityState.Locked;
    
    [SerializeField] private SortingLaneTracker laneTracker;
    
    [SerializeField] private BulbIndicatorController bulbIndicator;
    
    [SerializeField] private float abilityDuration = 5f;

    [SerializeField] private int abilitySecondaryGoalId;

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
        ReportAbilityUsed();
    }

    private IEnumerator AbilityTimer()
    {
        SetState(AbilityState.Active);
        yield return new WaitForSeconds(abilityDuration);
        SetState(AbilityState.Locked);
        bulbIndicator.ResetBonus();
        laneTracker.ResetBonus();
    }

    private void ReportAbilityUsed()
    {
        var abilityGoal = GoalSystem.ActiveGoals
            .FirstOrDefault(g => g.Id == abilitySecondaryGoalId);

        if (abilityGoal == null)
            return;

        if (abilityGoal.IsCompleted)
            return; // important: prevent duplicate increments

        GoalSystem.ReportProgress(
            abilityGoal.Id,
            abilityGoal.CurrentValue + 1
        );

        Debug.Log("[SecondaryGoal] Ability usage reported");
    }
}
