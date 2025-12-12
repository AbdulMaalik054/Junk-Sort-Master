using System.Collections;
using UnityEngine;

public static class UIAnimator
{
    private class CoroutineRunner : MonoBehaviour { }
    private static CoroutineRunner _runner;

    private static CoroutineRunner Runner
    {
        get
        {
            if (_runner == null)
            {
                var go = new GameObject("[UIAnimatorRunner]");
                Object.DontDestroyOnLoad(go);
                _runner = go.AddComponent<CoroutineRunner>();
            }
            return _runner;
        }
    }

    // ---------------------------------------------------
    // FADE IN / OUT
    // ---------------------------------------------------
    public static void FadeIn(CanvasGroup group, float duration = 0.25f, float delay = 0f)
    {
        if (!group.gameObject.activeSelf)
            group.gameObject.SetActive(true);

        Runner.StartCoroutine(FadeCoroutine(group, group.alpha, 1f, duration, delay));
    }

    public static void FadeOut(CanvasGroup group, float duration = 0.25f, float delay = 0f)
    {
        Runner.StartCoroutine(FadeCoroutine(group, group.alpha, 0f, duration, delay));
    }

    private static IEnumerator FadeCoroutine(CanvasGroup g, float start, float end, float duration, float delay)
    {
        if (delay > 0) yield return new WaitForSeconds(delay);

        float t = 0f;
        g.alpha = start;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            g.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }

        g.alpha = end;

        bool visible = end > 0.95f;
        g.interactable = visible;
        g.blocksRaycasts = visible;
    }

    // ---------------------------------------------------
    // PUNCH SCALE
    // ---------------------------------------------------
    public static void PunchScale(Transform target, float punchMultiplier = 1.15f, float duration = 0.18f)
    {
        if (target == null) return;
        Runner.StartCoroutine(ScalePunchCoroutine(target, punchMultiplier, duration));
    }

    private static IEnumerator ScalePunchCoroutine(Transform target, float mult, float duration)
    {
        Vector3 original = target.localScale;
        Vector3 targetScale = original * mult;

        float half = duration * 0.5f;
        float t = 0f;

        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            float p = t / half;
            target.localScale = Vector3.Lerp(original, targetScale, Mathf.SmoothStep(0, 1, p));
            yield return null;
        }

        t = 0f;
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            float p = t / half;
            target.localScale = Vector3.Lerp(targetScale, original, Mathf.SmoothStep(0, 1, p));
            yield return null;
        }

        target.localScale = original;
    }
}
