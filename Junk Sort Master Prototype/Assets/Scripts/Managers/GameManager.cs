using System.Collections;
using System.Collections.Generic;
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

    [Header("Global Bulbs Status")]
    [SerializeField] private bool bulbStatus = true;
    [Header("Lanes Reference")]
    [SerializeField] public BulbIndicatorController[] laneControllers;

    private readonly Dictionary<int, BulbIndicatorController> laneByIndex
        = new Dictionary<int, BulbIndicatorController>();



    private void Awake()
    {
        Instance = this;
    }

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
        gameRunning = true;
        paused = false;
        Time.timeScale = 1;
        SetGlobalNormal(bulbStatus);
        SetGlobalBreakdown(!bulbStatus);
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

        CacheLaneControllers();
        BreakdownManager.Instance.Initialize(laneControllers.Length);
        BreakdownManager.Instance.OnLocalBreakdown += HandleLocalBreakdown;
        BreakdownManager.Instance.OnLocalRepaired += HandleLocalRepair;
        BreakdownManager.Instance.OnGlobalBreakdown += HandleGlobalBreakdown;
        BreakdownManager.Instance.OnGlobalRepaired += HandleGlobalRepair;
    }
        


    private void HandleGlobalBreakdown()
    {
        SetGlobalBreakdown(bulbStatus);
        SetGlobalNormal(!bulbStatus);
        conveyorController.StopAll(false);
        spawner.StopSpawning();
        DragController.DisableDrag = true;

    }

    private void HandleGlobalRepair()
    {
        SetGlobalNormal(bulbStatus);
        SetGlobalBreakdown(!bulbStatus);
        DragController.DisableDrag = false;
        conveyorController.StartAll();
        spawner.StartSpawning();
    }

    private void HandleLocalBreakdown(int laneIndex)
    {
        UIManager.Instance.SpawnRepairButton(laneIndex);

        if (!laneByIndex.TryGetValue(laneIndex, out var controller))
        {
            Debug.LogError($"[GameManager] No lane controller for lane {laneIndex}");
            return;
        }

        Debug.Log($"Lane {laneIndex} breakdown handled in GameManager.");
        controller.StartBreakdownFlash();
    }

    private void HandleLocalRepair()
    {
        UIManager.Instance.RemoveRepairButton();

        foreach (var controller in laneByIndex.Values)
            controller.StopBreakdownFlash();

        conveyorController.StartAll();
    }

    // BULBS CACHE----------------------------------------------------
    private void CacheLaneControllers()
    {
        laneByIndex.Clear();

        foreach (var controller in laneControllers)
        {
            if (controller == null)
                continue;

            var tracker = controller.GetComponentInParent<SortingLaneTracker>();
            if (tracker == null)
            {
                Debug.LogError(
                    $"[GameManager] BulbIndicatorController {controller.name} has no SortingLaneTracker",
                    controller
                );
                continue;
            }

            int laneIndex = tracker.physicalLaneIndex;

            if (laneByIndex.ContainsKey(laneIndex))
            {
                Debug.LogError(
                    $"[GameManager] Duplicate BulbIndicatorController for lane {laneIndex}",
                    controller
                );
                continue;
            }

            laneByIndex.Add(laneIndex, controller);
        }
    }
    // BIG BULBS FUNCTIONS ---------------------------------------------
    private void SetGlobalNormal(bool ON)
    {
        var mainLane = laneControllers[0];
        mainLane?.UpdateBigGreenBulb(ON);
    }

    private void SetGlobalBreakdown(bool ON)
    {
        var mainLane = laneControllers[0];
        mainLane?.FlashBigRed();
        
            
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
        UIManager.Instance.RemoveRepairButton();
        UIManager.Instance.HidePauseMenu();
        // Force breakdown reset (new)
        BreakdownManager.Instance.Initialize(laneControllers.Length);
        SetGlobalNormal(bulbStatus);
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
        BreakdownManager.Instance.Initialize(laneControllers.Length);
        CleanupGameplay();
        UIManager.Instance.HideGameOver();
        UIManager.Instance.RemoveRepairButton();
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
