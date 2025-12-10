using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Difficulty Settings")]
    public Difficulty currentDifficulty;
    public float easyTime = 60f;
    public float mediumTime = 45f;
    public float hardTime = 30f;

    [Header("Runtime Timer")]
    public float currentTime;
    private bool gameRunning;
    

    [Header("References")]
    [SerializeField] private Spawner spawner;
    [SerializeField] private ConveyorManager conveyor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UIManager.Instance.ShowMainMenu();
    }

    

    // ---------------------------------------------------------
    // PUBLIC METHODS CALLED FROM UI
    // ---------------------------------------------------------
    public void SelectDifficulty(int index)
    {
        currentDifficulty = (Difficulty)index;
    }

    public void StartGame()
    {
        ApplyDifficulty();
        ResetSystems();

        UIManager.Instance.ShowGameplayUI();

        gameRunning = true;
        DragController.DisableDrag = false;
        StartCoroutine(GameTimer());
    }

    public void GameOver()
    {
        gameRunning = false;
        spawner.StopSpawning();
        conveyor.StopConveyor(false);
        DragController.DisableDrag = true; // disable interaction

        UIManager.Instance.ShowGameOverUI();
    }

    // ---------------------------------------------------------
    // DIFFICULTY HANDLING
    // ---------------------------------------------------------
    private void ApplyDifficulty()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                currentTime = easyTime;
                conveyor.SetDifficulty(.5f, 2f, .05f);
                spawner.SetSpawnRates(4f, 7f);
                break;

            case Difficulty.Medium:
                currentTime = mediumTime;
                conveyor.SetDifficulty(2f, 5f, 12f);
                spawner.SetSpawnRates(3f, 6f);
                break;

            case Difficulty.Hard:
                currentTime = hardTime;
                conveyor.SetDifficulty(3f, 7f, 15f);
                spawner.SetSpawnRates(2f, 4f);
                break;
        }
    }

    // ---------------------------------------------------------
    // TIMER SYSTEM
    // ---------------------------------------------------------
    private IEnumerator GameTimer()
    {
        while (currentTime > 0 && gameRunning)
        {
            currentTime -= Time.deltaTime;
            UIManager.Instance.UpdateTimerUI(currentTime);
            yield return null;
        }

        GameOver();
    }

    // ---------------------------------------------------------
    // SYSTEM RESET
    // ---------------------------------------------------------
    private void ResetSystems()
    {
        ScoreManager.Instance.ResetScore();
        conveyor.StartConveyor();
        
        spawner.StartSpawning();
    }

    

    // ---------------------------------------------------------
    // PUBLIC GAME EVENTS
    // ---------------------------------------------------------
    public void AddScore(int amount , Vector3 objectPosition)
    {
        ScoreManager.Instance.AddCorrectScore(amount , objectPosition);

        UIManager.Instance.UpdateScoreUI(ScoreManager.Instance.Score);
    }

    public void WrongSortingPenalty(int amount , Vector3 objectPosition)
    {
        ScoreManager.Instance.AddWrongPenalty(amount , objectPosition);
        UIManager.Instance.UpdateScoreUI(ScoreManager.Instance.Score);

        // Optional time penalty
        currentTime -= 2f;
    }
}

// ---------------------------------------------------------
// DIFFICULTY ENUM
// ---------------------------------------------------------
public enum Difficulty { Easy, Medium, Hard }
