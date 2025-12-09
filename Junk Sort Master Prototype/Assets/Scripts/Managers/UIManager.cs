using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ShowMainMenu();
    }

    // -------------------------------------------------------------------
    // PANEL MANAGEMENT
    // -------------------------------------------------------------------

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ShowDifficultyMenu()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ShowGameplayUI()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOverUI()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);

        finalScoreText.text = "Final Score: " + ScoreManager.Instance.Score;
    }

    // -------------------------------------------------------------------
    // UI UPDATES
    // -------------------------------------------------------------------

    public void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateTimerUI(float timeRemaining)
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Time: " + seconds;
    }

    public void UpdateComboUI(int multiplier)
    {
        comboText.text = "Combo: " + multiplier;
    }

    // -------------------------------------------------------------------
    // BUTTON EVENTS
    // -------------------------------------------------------------------

    public void OnPlayButton()
    {
        ShowDifficultyMenu();
    }

    public void OnDifficultySelected(int difficultyIndex)
    {
        GameManager.Instance.SelectDifficulty(difficultyIndex);
        GameManager.Instance.StartGame();
    }

    public void OnRetryButton()
    {
        GameManager.Instance.StartGame();
    }

    public void OnMainMenuButton()
    {
        ShowMainMenu();
    }

    public void ShowGameOverPanel(int score, int streak, int multiplier)
    {
        gameOverPanel.SetActive(true);

        finalScoreText.text = "Final Score: " + score;
        
    }

}
