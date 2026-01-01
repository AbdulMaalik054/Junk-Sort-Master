using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityController : MonoBehaviour
{
    public enum AbilityState { Locked, Ready, Active }
    public AbilityState currentState = AbilityState.Locked;

    [Header("Linked Systems")]
    [SerializeField] private BulbIndicatorController bulbIndicator;

    [Header("UI")]
    [SerializeField] private Button abilityButton; // Replaces GameObject icon (must be Button)

    [Header("Ability Stats")]
    [SerializeField] private float abilityDuration = 5f;
    private Coroutine abilityRoutine;
    private void Start()
    {
        SetState(AbilityState.Locked);
        if (abilityButton != null)
        {
            abilityButton.onClick.AddListener(OnAbilityClicked);
        }
    }

    public void NotifyBonusChanged(int currentBonus, int maxBonus)
    {
        if (currentBonus >= maxBonus)
        {
            SetState(AbilityState.Ready);
        }
        else
        {
            SetState(AbilityState.Locked);
        }
    }

    private void SetState(AbilityState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case AbilityState.Locked:
                abilityButton.gameObject.SetActive(false);
                
                break;

            case AbilityState.Ready:
                abilityButton.gameObject.SetActive(true);
                break;

            case AbilityState.Active:
                abilityButton.gameObject.SetActive(false);
                bulbIndicator.StartAbilityFlash();
                break;
        }
    }
    private void OnAbilityClicked()
    {
        if (currentState == AbilityState.Ready)
        {
            SetState(AbilityState.Active);
            if (abilityRoutine != null)
                StopCoroutine(abilityRoutine);

            abilityRoutine = StartCoroutine(AbilityTimer());
        }
    }

    private IEnumerator AbilityTimer()
    {
        yield return new WaitForSeconds(abilityDuration);

        // Ability ends
        SetState(AbilityState.Locked);
        bulbIndicator.ResetBonus(); // Reset bonus after ability used
    }
}
