using UnityEngine;
using TMPro;
using UnityEngine.UI;
[DefaultExecutionOrder(-50)]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Top Bar")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Combo Popup")]
    [SerializeField] private CanvasGroup comboGroup;
    [SerializeField] private TextMeshProUGUI comboText;

    [Header("Transition Panel")]
    [SerializeField] private CanvasGroup transitionPanel;
    [SerializeField] private TextMeshProUGUI transitionMessage;

    [Header("Game Over Panel")]
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button homeButton;

    [Header("Pause Panel")]
    [SerializeField] private CanvasGroup pausePanel;
    [SerializeField] private Button resumeButton;

    private void Awake()
    {
        Instance = this;

        InitPanel(transitionPanel);
        InitPanel(gameOverPanel);
        InitPanel(pausePanel);

        comboGroup.alpha = 0;

        WireButtons();
    }

    private void InitPanel(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
        panel.gameObject.SetActive(true);  // keep active now
    }

    private void WireButtons()
    {
        retryButton?.onClick.AddListener(() => GameManager.Instance.RestartGame());
        homeButton?.onClick.AddListener(() => GameManager.Instance.ReturnToMenu());
        resumeButton?.onClick.AddListener(() => GameManager.Instance.TogglePause());
    }

    // SCORE UI ----------------------------------------------------
    public void UpdateScoreUI(int score) => UpdateScore(score);
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
        UIAnimator.PunchScale(scoreText.transform, 1.1f, 0.15f);
    }

    public void UpdateTimer(float timeLeft)
    {
        timerText.text = Mathf.CeilToInt(timeLeft).ToString();
    }

    // COMBO UI -----------------------------------------------------
    
    public void UpdateComboUI(int combo) => ShowCombo(combo);
    public void ShowCombo(int combo)
    {
        comboText.text = $"Combo x{combo}";

        UIAnimator.FadeIn(comboGroup, 0.15f);
        UIAnimator.PunchScale(comboText.transform, 1.15f, 0.2f);
        UIAnimator.FadeOut(comboGroup, 0.25f, 0.5f);
    }

    // TRANSITION ---------------------------------------------------
    public void ShowTransition(string msg, float duration = 1f)
    {
        transitionMessage.text = msg;
        UIAnimator.FadeIn(transitionPanel, 0.35f);
        UIAnimator.FadeOut(transitionPanel, 0.35f, duration);
    }

    // PAUSE --------------------------------------------------------
    public void ShowPauseMenu() => UIAnimator.FadeIn(pausePanel, 0.2f);
    public void HidePauseMenu() => UIAnimator.FadeOut(pausePanel, 0.2f);

    // GAME OVER ----------------------------------------------------
    public void ShowGameOver(int finalScore, int bestScore)
    {
        finalScoreText.text = $"Score: {finalScore}";
        bestScoreText.text = $"Best: {bestScore}";

        UIAnimator.FadeIn(gameOverPanel, 0.35f);
        UIAnimator.PunchScale(finalScoreText.transform, 1.2f, 0.22f);
    }

    public void HideGameOver() => UIAnimator.FadeOut(gameOverPanel, 0.2f);
}
