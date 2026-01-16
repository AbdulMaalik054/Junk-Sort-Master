using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GoalTutorialController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GoalHintPresenter hintPresenter;

    private void OnEnable()
    {
        StartCoroutine(WaitAndSubscribe());
    }

    private IEnumerator WaitAndSubscribe()
    {
        while (GoalUIController.Instance == null)
            yield return null;

        GoalUIController.Instance.OnGoalUIStateChanged -= HandleUIStateChanged;
        GoalUIController.Instance.OnGoalUIStateChanged += HandleUIStateChanged;
    }

    private void OnDisable()
    {
        if (GoalUIController.Instance != null)
            GoalUIController.Instance.OnGoalUIStateChanged -= HandleUIStateChanged;
    }

    private void HandleUIStateChanged(GoalUIState previous, GoalUIState current)
    {
        if (current == GoalUIState.LevelSuccess || current == GoalUIState.LevelFailure)
        {
            // Stop any running fades before starting a new one
            StopAllCoroutines();
            hintPresenter.TryDisplayHint();
        }
        else
            hintPresenter.Hide();
    }

    private void EvaluateAndPresentHints()
    {
        var result = GoalUIController.Instance.CachedEndLevelResult;
        if (!result.HasValue)
            return;

        var context = new GoalHintContext
        {
            PrimaryGoalFailed = !result.Value.LevelSucceeded,
            FailedSecondaryGoalIds = new HashSet<int>(
                result.Value.SecondaryGoals
                    .Where(g => !g.IsCompleted)
                    .Select(g => g.Id)
            ),
            MistakeCount = PlayerMistakeTracker.TotalMistakes
        };

        var db = GoalHintDatabase.Instance;
        if (db == null)
            return;

        var hints = GoalHintEvaluator.Evaluate(db.AllHints, context);

        if (hints.Count > 0)
            hintPresenter.ShowHint(hints[0]); // show first matching hint
        else
            hintPresenter.Hide();
    }
}