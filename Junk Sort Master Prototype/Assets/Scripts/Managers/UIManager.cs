using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;
[DefaultExecutionOrder(-50)]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Top Bar")]
    [SerializeField] private CanvasGroup topBarGroup;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Combo Popup")]
    [SerializeField] private CanvasGroup comboGroup;
    [SerializeField] private TextMeshProUGUI comboText;

    [Header("Loading Panel")]
    [SerializeField] private CanvasGroup loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingMessage;

    [Header("Start Panel")]
    [SerializeField] private CanvasGroup startPanel;
    [SerializeField] private TextMeshProUGUI startMessage;

    [Header("Game Over Panel")]
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    

    [Header("Pause Panel")]
    [SerializeField] private CanvasGroup pausePanel;
    [SerializeField] public Button pauseButton;

    private void Awake()
    {
        Instance = this;

        InitPanel(loadingPanel);
        InitPanel(gameOverPanel);
        InitPanel(pausePanel);

        comboGroup.alpha = 0;

        
    }

    private void InitPanel(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
        panel.gameObject.SetActive(true);  // keep active now
    }

   

    // SCORE UI ----------------------------------------------------
    public void UpdateScoreUI(int score) => UpdateScore(score);
    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
        UIAnimator.PunchScale(scoreText.transform, 1.1f, 0.15f);
    }

    public void UpdateTimer(float timeLeft)
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
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
    public void ShowLoadingTransition(string msg, float duration = 1f)
    {
        loadingMessage.text = msg;
        UIAnimator.FadeIn(loadingPanel, 0.0f);
        UIAnimator.FadeOut(loadingPanel, 0.35f, duration);
    }

    public void ShowStartTransition(string msg, float duration = 1f)
    {
        startMessage.text = msg;
        UIAnimator.FadeIn(startPanel, 0.35f);
        UIAnimator.FadeOut(startPanel, 0.35f, duration);
        
    }

    // PAUSE --------------------------------------------------------
    public void ShowPauseMenu()
    {
        pauseButton.gameObject.SetActive(false);
        UIAnimator.FadeIn(pausePanel, 0.2f);
    }
    public void HidePauseMenu()
    {
        UIAnimator.FadeOut(pausePanel, 0.2f);
        pauseButton.gameObject.SetActive(true);
    }

    // GAME OVER ----------------------------------------------------
    public void ShowGameOver(int finalScore, int bestScore)
    {   
        
        finalScoreText.text = $"Score: {finalScore}";
        bestScoreText.text = $"Best: {bestScore}";
        HideTopBarGroup();

        UIAnimator.FadeIn(gameOverPanel, 0.35f);
        UIAnimator.PunchScale(finalScoreText.transform, 1.2f, 0.22f);
        
    }
    public void ShowtopBarGroup() => topBarGroup.gameObject.SetActive(true);
    public void HideTopBarGroup()=>topBarGroup.gameObject.SetActive(false);
    public void HideGameOver() => UIAnimator.FadeOut(gameOverPanel, 0.2f);
}
