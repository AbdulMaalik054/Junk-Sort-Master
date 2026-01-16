using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private int uiLaneIndex;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image cooldownOverlay; // Assign the radial fill image
    [SerializeField] private Transform worldAnchor;

    private RectTransform canvasRect;
    private Camera mainCam;
    private AbilityController.AbilityState lastState = AbilityController.AbilityState.Locked;
    private float readyTimer = 0f;
    private int playableLaneIndex => uiLaneIndex - 1;

    private void Start()
    {
        mainCam = Camera.main;
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Initial hidden state
        canvasGroup.alpha = 0;
        rectTransform.localScale = Vector3.zero;
        if (cooldownOverlay) cooldownOverlay.fillAmount = 0;
    }

    private void Update()
    {
        FollowWorldPosition();
        HandleAnimations();
    }

    private void FollowWorldPosition()
    {
        if (worldAnchor == null) return;

        Vector3 screenPos = mainCam.WorldToScreenPoint(worldAnchor.position);

        // If object is behind camera, don't show it
        if (screenPos.z < 0)
        {
            canvasGroup.alpha = 0;
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 anchoredPos);
        rectTransform.anchoredPosition = anchoredPos;
    }

    private void HandleAnimations()
    {
        if (lastState == AbilityController.AbilityState.Ready)
        {
            readyTimer += Time.deltaTime;
            if (readyTimer >= 3f)
            {
                rectTransform.DOPunchScale(Vector3.one * 0.15f, 0.4f);
                readyTimer = 0;
            }
        }
    }

    public void RefreshUI(AbilityController.AbilityState newState, float activeProgress = 0)
    {
        if (newState != lastState)
        {
            rectTransform.DOKill();
            canvasGroup.DOKill();

            if (newState == AbilityController.AbilityState.Ready)
            {
                canvasGroup.DOFade(1, 0.3f);
                rectTransform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else if (newState == AbilityController.AbilityState.Active)
            {
                // Keep visible but maybe dim slightly, or just keep interactable false
                canvasGroup.interactable = false;
            }
            else // Locked
            {
                canvasGroup.DOFade(0, 0.2f);
                rectTransform.DOScale(0, 0.2f);
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            lastState = newState;
        }

        // Update Radial Fill if active
        if (newState == AbilityController.AbilityState.Active && cooldownOverlay != null)
        {
            // Assuming activeProgress is 0.0 to 1.0 (passed from AbilityController)
            cooldownOverlay.fillAmount = activeProgress;
        }
        else if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 0;
        }
    }

    public void OnClick()
    {
        AbilityController ability = AbilityManager.Instance.GetAbilityForLane(playableLaneIndex);
        if (ability != null) ability.OnAbilityClicked();
    }
}