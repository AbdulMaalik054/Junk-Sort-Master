using System.Collections;
using UnityEditor;
using UnityEngine;

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

    [Header("Progression")]
    [SerializeField] private ProgressionController progression;

    [Header("References")]
    [SerializeField] private Spawner spawner;
    [SerializeField] private ConveyorController conveyorController;

    [Header("Breakdowns")]
    [SerializeField] private bool breakdownSystemEnabled = true;

    [Header("Global Bulbs")]
    public BulbEmissionController BigGreenBulb;
    public BulbEmissionController BigRedBulb;

    [Header("Lanes Reference")]
    public BulbIndicatorController[] laneControllers;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIManager.Instance.ShowLoadingTransition("LOADING", 2.0f);
        MenuController.Instance.ReturnToMainMenu();
        BigGreenBulb.SetBigGreen(true);

    }

    // MENU FLOW -------------------------------------------------------
    public void SelectDifficulty(int index)
    {
        currentDifficulty = (Difficulty)index;
    }

    public void StartGame()
    {
        gameRunning = true;
        paused = false;
        Time.timeScale = 1;

        ApplyDifficulty();
        progression.Initialize(currentDifficulty);
        progression.ApplyTier(0);

        if (breakdownSystemEnabled)
            InitializeBreakdowns();

        DragController.DisableDrag = false;
        UIManager.Instance.ShowStartTransition("GO!", 2.0f);
        UIManager.Instance.ShowtopBarGroup();
        UIManager.Instance.pauseButton.gameObject.SetActive(true);

        ScoreManager.Instance.ResetScore();

        conveyorController.StartAll();
        spawner.StartSpawning();

        StartCoroutine(GameTimer());
    }

    // BREAKDOWNS ------------------------------------------------------
    private void InitializeBreakdowns()
    {
        
        BreakdownManager.Instance.Initialize(laneControllers.Length);
        BreakdownManager.Instance.OnLocalBreakdown += HandleLocalBreakdown;
        BreakdownManager.Instance.OnLocalRepaired += HandleLocalRepair;
        BreakdownManager.Instance.OnGlobalBreakdown += HandleGlobalBreakdown;
        BreakdownManager.Instance.OnGlobalRepaired += HandleGlobalRepair;
    }
        


    private void HandleGlobalBreakdown()
    {
        BigGreenBulb.SetBigGreen(false);
        BigRedBulb.FlashBigRed(true);
        conveyorController.StopAll(false);
        spawner.StopSpawning();
        DragController.DisableDrag = true;
    }

    private void HandleGlobalRepair()
    {
        BigRedBulb.FlashBigRed(false);
        BigGreenBulb.SetBigGreen(true);
        DragController.DisableDrag = false;
        conveyorController.StartAll();
        spawner.StartSpawning();
    }

    private void HandleLocalBreakdown(int laneIndex)
    {
        
        
        UIManager.Instance.SpawnRepairButton(laneIndex);
        
    }
        
    private void HandleLocalRepair()
    {
        UIManager.Instance.RemoveRepairButton();
        conveyorController.StartAll();
    }

    // CLEANUP ---------------------------------------------------------
    private void CleanupGameplay()
    {
        StopAllCoroutines();
        gameRunning = false;
        paused = false;

        conveyorController.StopAll(false);
        spawner.StopSpawning();
        PoolManager.Instance.ResetPools();
        ScoreManager.Instance.ResetScore();
    }

    public void ReturnToMenu()
    {
        DragController.DisableDrag = true;
        CleanupGameplay();
        UIManager.Instance.HideGameOver();
        UIManager.Instance.HidePauseMenu();
        MenuController.Instance.ReturnToMainMenu();
    }

    // PAUSE -----------------------------------------------------------
    public void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;

        if (paused)
            UIManager.Instance.ShowPauseMenu();
        else
            UIManager.Instance.HidePauseMenu();
    }

    // GAME OVER -------------------------------------------------------
    public void GameOver()
    {
        if (!gameRunning) return;

        gameRunning = false;
        paused = false;
        Time.timeScale = 0;

        spawner.StopSpawning();
        conveyorController.StopAll(false);
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
                conveyorController.SetDifficulty(0.5f, 2f, 0.05f);
                spawner.SetSpawnRates(4f);
                break;

            case Difficulty.Medium:
                currentTime = mediumTime;
                conveyorController.SetDifficulty(1.5f, 2.75f, 0.05f);
                spawner.SetSpawnRates(3f);
                break;

            case Difficulty.Hard:
                currentTime = hardTime;
                conveyorController.SetDifficulty(2.25f, 3.25f, 0.05f);
                spawner.SetSpawnRates(2f);
                break;

            case Difficulty.Endless:
                currentTime = endlessTime;
                conveyorController.SetDifficulty(1.75f, 3.75f, 0.05f);
                spawner.SetSpawnRates(2f);
                break;
        }
    }

    // TIMER -----------------------------------------------------------
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

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

public enum Difficulty { Easy, Medium, Hard, Endless }
