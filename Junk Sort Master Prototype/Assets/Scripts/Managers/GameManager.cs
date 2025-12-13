using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(0)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Difficulty")]
    public Difficulty currentDifficulty;
    public float easyTime = 60f;
    public float mediumTime = 45f;
    public float hardTime = 30f;
    public float endlessTime = 3600f;

    public float currentTime;
    private bool gameRunning;
    private bool paused;

    [Header("Progresseion Controller")]

    [SerializeField] private ProgressionController progression;

    [Header("References")]
    [SerializeField] private Spawner spawner;
    [SerializeField] private ConveyorManager conveyor;

    private void Awake()
    {
        Instance = this;
    }

    // Replace all calls to progression.StartProgression() with progression.Initialize(currentDifficulty);
    // and progression.StartProgression() with progression.ApplyTier(0); if you want to start the first tier.

    private void Start()
    {
        UIManager.Instance.ShowLoadingTransition("LOADING", 2.0f);
        MenuController.Instance.ReturnToMainMenu();
    }

    // MENU FLOW -------------------------------------------------------
    public void SelectDifficulty(int index)
    {
        currentDifficulty = (Difficulty)index;
    }

    public void StartGame()
    {
        ApplyDifficulty();
        //-----------------Progressions Parameters

        progression.Initialize(currentDifficulty);
        progression.ApplyTier(0); // Start the first tier
        gameRunning = true;
        paused = false;
        DragController.DisableDrag = false;
        UIManager.Instance.ShowStartTransition("GO!", 2.0f);
        UIManager.Instance.ShowtopBarGroup();
        UIManager.Instance.pauseButton.gameObject.SetActive(true);
        ScoreManager.Instance.ResetScore();
        Invoke(nameof(StartDelayedCoroutine), 1.8f);
        conveyor.StartConveyor();
        spawner.StartSpawning();
    }


    public void ReturnToMenu()
    {
        DragController.DisableDrag = true;

        CleanupGameplay();

        UIManager.Instance.HideGameOver();
        UIManager.Instance.HidePauseMenu();
        progression.Initialize(currentDifficulty); // Re-initialize progression
        progression.ApplyTier(0); // Start the first tier
        MenuController.Instance.ReturnToMainMenu();
    }

    // CLEAR OBJECT POOL------------------------------------------------

    private void CleanupGameplay()
    {
        conveyor.StopConveyor(false);
        spawner.StopSpawning();
        PoolManager.Instance.ResetAllObjects();
        ScoreManager.Instance.ResetScore();
        
    }


    // PAUSE -----------------------------------------------------------
    public void TogglePause()
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0;
            UIManager.Instance.ShowPauseMenu();
        }
        else
        {
            paused = false;
            Time.timeScale = 1;
            UIManager.Instance.HidePauseMenu();
        }
    }

    // GAME OVER -------------------------------------------------------
    public void GameOver()
    {
        if (!gameRunning) return;

        gameRunning = false;
        paused = false;
        Time.timeScale = 1;

        progression.Initialize(currentDifficulty); // Re-initialize progression to stop/cleanup
        spawner.StopSpawning();
        conveyor.StopConveyor(false);
        DragController.DisableDrag = true;

        int finalScore = ScoreManager.Instance.Score;
        int best = PlayerPrefs.GetInt("BestScore", 0);

        if (finalScore > best)
            PlayerPrefs.SetInt("BestScore", finalScore);

        UIManager.Instance.pauseButton.gameObject.SetActive(false);
        UIManager.Instance.ShowGameOver(finalScore, PlayerPrefs.GetInt("BestScore"));
    }

    // DIFFICULTY ------------------------------------------------------
    private void ApplyDifficulty()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                currentTime = easyTime;
                conveyor.SetDifficulty(0.5f, 2f, 0.05f);
                spawner.SetSpawnRates(4f);
                break;

            case Difficulty.Medium:
                currentTime = mediumTime;
                conveyor.SetDifficulty(1.5f, 2.75f, 0.05f);
                spawner.SetSpawnRates(3f);
                break;

            case Difficulty.Hard:
                currentTime = hardTime;
                conveyor.SetDifficulty(2.25f, 3.25f, 0.05f);
                spawner.SetSpawnRates(2f);
                break;
            case Difficulty.Endless:
                currentTime = endlessTime;
                conveyor.SetDifficulty(1.75f, 3.75f, 0.05f);
                spawner.SetSpawnRates(2f);
                break;
        }
    }

    // TIMER -----------------------------------------------------------

    public void StartDelayedCoroutine()
    {
        StartCoroutine(GameTimer());
    }
    private IEnumerator GameTimer()
    {
        
        while (currentTime > 0 && gameRunning)
        {
            if (!paused)
            {
                currentTime -= Time.unscaledDeltaTime;
                UIManager.Instance.UpdateTimer(currentTime);
            }
            yield return null;
        }

        GameOver();
    }

    // RESET -----------------------------------------------------------
    public void RestartGame()
    {
        
        Time.timeScale = 1;
        CleanupGameplay();
        UIManager.Instance.HideGameOver();
        UIManager.Instance.HidePauseMenu();
        StartGame();
       
    }

    // SCORE EVENTS ----------------------------------------------------
    public void AddScore(int amount, Vector3 pos)
    {
        ScoreManager.Instance.AddCorrectScore(amount, pos);
    }

    public void WrongSortingPenalty(int amount, Vector3 pos)
    {
        ScoreManager.Instance.AddWrongPenalty(amount, pos);
        currentTime -= 2f;
    }
}

public enum Difficulty { Easy, Medium, Hard , Endless }
