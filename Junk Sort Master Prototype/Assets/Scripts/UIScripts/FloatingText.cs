using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public RectTransform rectTransform;
    public TextMeshProUGUI tmp;
    public CanvasGroup canvasGroup;

    [Header("Animation")]
    public float lifetime = 0.8f;
    public float floatDistance = 60f;

    private Coroutine playRoutine;

    private void Awake()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if (tmp == null) tmp = GetComponentInChildren<TextMeshProUGUI>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(string text, Color color)
    {
        tmp.text = text;
        tmp.color = color;
    }

    public void Play()
    {
        gameObject.SetActive(true);
        if (playRoutine != null) StopCoroutine(playRoutine);
        playRoutine = StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        float t = 0f;
        canvasGroup.alpha = 1f;
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 endPos = startPos + Vector2.up * floatDistance;

        while (t < lifetime)
        {
            t += Time.deltaTime;
            float normalized = t / lifetime;

            // position + fade
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, normalized);
            canvasGroup.alpha = 1f - normalized;

            yield return null;
        }

        // cleanup
        canvasGroup.alpha = 0f;
        playRoutine = null;
        FloatingTextPool.Instance.ReturnToPool(this);
    }
}
