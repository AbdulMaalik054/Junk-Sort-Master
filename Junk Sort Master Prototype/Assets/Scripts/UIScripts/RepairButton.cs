using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RepairButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int laneIndex;
    public float holdDuration = 1f;

    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image holdProgressFill; // Radial fill image

    private Transform worldAnchor;
    private bool holding = false;
    private float timer = 0f;
    private bool isVisible = false;
    private Camera mainCam;
    private RectTransform canvasRect;
    private RectTransform myRect;

    private void Awake()
    {
        mainCam = Camera.main;
        myRect = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        SetVisible(false, true);
    }

    public void SetupRepair(int index, Transform anchor)
    {
        laneIndex = index;
        worldAnchor = anchor;
    }

    public void SetVisible(bool show, bool instant = false)
    {
        isVisible = show;
        canvasGroup.DOKill();
        transform.DOKill();

        float duration = instant ? 0 : 0.4f;

        if (show)
        {
            canvasGroup.DOFade(1, duration);
            transform.DOScale(1, duration).SetEase(Ease.OutBack);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            // Attention pulse
            transform.DOScale(1.1f, 0.6f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(duration);
        }
        else
        {
            canvasGroup.DOFade(0, duration);
            transform.DOScale(0, duration).SetEase(Ease.InBack);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            holding = false;
            timer = 0;
            holdProgressFill.fillAmount = 0;
        }
    }

    private void Update()
    {
        if (!isVisible || worldAnchor == null) return;

        // Follow World Position
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldAnchor.position);
        if (screenPos.z < 0)
        {
            canvasGroup.alpha = 0;
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 anchoredPos);
        myRect.anchoredPosition = anchoredPos;

        // Hold Logic
        if (holding)
        {
            timer += Time.deltaTime;
            holdProgressFill.fillAmount = timer / holdDuration;

            if (timer >= holdDuration)
            {
                BreakdownManager.Instance?.RepairLane(laneIndex);
                SetVisible(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        holding = true;
        transform.DOScale(0.9f, 0.1f);
    }

    public void OnPointerUp(PointerEventData data)
    {
        holding = false;
        timer = 0;
        holdProgressFill.fillAmount = 0;
        if (isVisible) transform.DOScale(1f, 0.1f);
    }
}