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


    public void ShowHint(GoalHint hint)
    {
        hintText.text = hint.Message;
        UIAnimator.FadeIn(canvasGroup, fadeDuration);
        UIAnimator.PunchScale(hintText.transform);

        Debug.Log($"[Tutorial] Showing hint: {hint.Id}");
    }

    public void Hide()
    {
        UIAnimator.FadeOut(canvasGroup, fadeDuration);
    }

    private void HideImmediate()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
