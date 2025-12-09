using System;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    private float currentTime = 0f;
    private bool running = false;

    /// <summary>
    /// Invoked when timer reaches zero.
    /// </summary>
    public event Action OnTimerExpired;

    /// <summary>
    /// Initializes and starts the timer.
    /// </summary>
    public void InitializeTimer(float seconds)
    {
        currentTime = Mathf.Max(0f, seconds);
        running = currentTime > 0f;
        UpdateTimerDisplay(currentTime);
    }

    /// <summary>
    /// Stops the timer (pauses).
    /// </summary>
    public void StopTimer()
    {
        running = false;
    }

    /// <summary>
    /// Resume the timer if it has time left.
    /// </summary>
    public void ResumeTimer()
    {
        if (currentTime > 0f) running = true;
    }

    private void Update()
    {
        if (!running) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            running = false;
            UpdateTimerDisplay(currentTime);
            OnTimerExpired?.Invoke();
            return;
        }

        UpdateTimerDisplay(currentTime);
    }

    private void UpdateTimerDisplay(float time)
    {
        if (timerText == null) return;

        int min = Mathf.FloorToInt(time / 60f);
        int sec = Mathf.FloorToInt(time % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
}
