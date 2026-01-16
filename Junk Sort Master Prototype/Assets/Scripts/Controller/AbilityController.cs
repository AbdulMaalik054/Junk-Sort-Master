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

    // Inside AbilityController.cs
    private void SetState(AbilityState newState)
    {
        currentState = newState;
        if (newState == AbilityState.Active)
            bulbIndicator.StartAbilityFlash();

        // Notify the manager to refresh UI immediately
        AbilityManager.Instance.RefreshAllButtons();
    }

    public void OnAbilityClicked()
    {
        if (currentState != AbilityState.Ready) return;
        

        if (abilityRoutine != null)
            StopCoroutine(abilityRoutine);

        abilityRoutine = StartCoroutine(AbilityTimer());
        ReportAbilityUsed();
    }

    public float GetActiveProgress()
    {
        if (currentState != AbilityState.Active) return 0f;
        // Returns 1.0 at start, 0.0 at end
        return 1f - (timerCount / abilityDuration);
    }

    // Update your AbilityTimer to track progress
    private float timerCount = 0f;
    private IEnumerator AbilityTimer()
    {
        SetState(AbilityState.Active);
        timerCount = 0f;
        while (timerCount < abilityDuration)
        {
            timerCount += Time.deltaTime;
            // The Manager will poll this value via RefreshUI
            yield return null;
        }
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
