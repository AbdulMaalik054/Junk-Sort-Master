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

    public float currentTime;
    private bool gameRunning;
    private bool paused;

    [Header("References")]
    [SerializeField] private Spawner spawner;
    [SerializeField] private ConveyorManager conveyor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        UIManager.Instance.ShowTransition("LOADING", 0.8f);
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
        RestartGame();

        gameRunning = true;
        paused = false;
        DragController.DisableDrag = false;

        UIManager.Instance.ShowTransition("GO!", 0.5f);
        StartCoroutine(GameTimer());
    }

    public void ReturnToMenu()
    {
        DragController.DisableDrag = true;

        spawner.StopSpawning();
        conveyor.StopConveyor(false);

        UIManager.Instance.HideGameOver();
        MenuController.Instance.ReturnToMainMenu();
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

        spawner.StopSpawning();
        conveyor.StopConveyor(false);
        DragController.DisableDrag = true;

        int finalScore = ScoreManager.Instance.Score;
        int best = PlayerPrefs.GetInt("BestScore", 0);

        if (finalScore > best)
            PlayerPrefs.SetInt("BestScore", finalScore);

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

    // TIMER -----------------------------------------------------------
    private IEnumerator GameTimer()
    {
        while (currentTime > 0 && gameRunning)
        {
            if (!paused)
            {
                currentTime -= Time.deltaTime;
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
        ScoreManager.Instance.ResetScore();

        conveyor.StartConveyor();
        spawner.StartSpawning();
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

public enum Difficulty { Easy, Medium, Hard }
