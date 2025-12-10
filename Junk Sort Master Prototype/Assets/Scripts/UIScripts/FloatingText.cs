using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private TMP_Text text;

    [Header("Animation")]
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float riseAmount = 40f;

    [HideInInspector] public RectTransform rect;

    private Color originalColor;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalColor = text.color;
    }

    // Called once when taken from pool
    public void Init(string value, Color color)
    {
        text.text = value;
        text.color = color;
        text.alpha = 1f;
    }

    // Start floating animation
    public void Play()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        float t = 0f;
        Vector2 start = rect.anchoredPosition;

        while (t < lifetime)
        {
            t += Time.deltaTime;
            float progress = t / lifetime;

            // Rise upward
            rect.anchoredPosition = start + new Vector2(0, riseAmount * progress);

            // Fade out
            text.alpha = 1f - progress;

            yield return null;
        }

        // Reset alpha for next use
        text.alpha = 1f;

        // Return to pool
        gameObject.SetActive(false);
        FloatingTextPool.Instance.ReturnToPool(this);
    }
}
