using TMPro;
using UnityEngine;

public class GoalHintPresenter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI hintText;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        HideImmediate();
    }

    private void OnEnable()
    {
        // Subscribe to state changes
        if (GoalUIController.Instance != null)
        {
            GoalUIController.Instance.OnGoalUIStateChanged += HandleStateChange;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        if (GoalUIController.Instance != null)
        {
            GoalUIController.Instance.OnGoalUIStateChanged -= HandleStateChange;
        }
    }

    private void HandleStateChange(GoalUIState oldState, GoalUIState newState)
    {
        // 1. Show hints when the level ends
        if (newState == GoalUIState.LevelSuccess || newState == GoalUIState.LevelFailure)
        {
            TryDisplayHint();
        }
        // 2. Hide hints when returning to menu or restarting
        else if (newState == GoalUIState.Hidden || newState == GoalUIState.PreLevel)
        {
            Hide();
        }
    }

    public void TryDisplayHint()
    {
        var hints = GoalUIController.Instance.CachedHints;

        if (hints != null && hints.Count > 0)
        {
            // Pick the first applicable hint (or you could pick a random one)
            ShowHint(hints[0]);
        }
        else
        {
            HideImmediate();
        }
    }

    public void ShowHint(GoalHint hint)
    {
        hintText.text = hint.Message;

        // Check parents for alpha killers
        CanvasGroup[] parents = GetComponentsInParent<CanvasGroup>();
        foreach (var p in parents)
        {
            if (p.alpha <= 0.01f && p != canvasGroup)
            {
                Debug.LogWarning($"<color=red>[UI Alert]</color> Hint '{hint.Id}' is invisible because Parent '{p.gameObject.name}' has alpha 0!");
            }
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        UIAnimator.FadeOut(canvasGroup, fadeDuration);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void HideImmediate()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}