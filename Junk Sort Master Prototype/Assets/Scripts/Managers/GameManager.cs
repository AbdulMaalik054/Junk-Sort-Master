using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance ;


    public bool isGameStarted = false;
    [Header("Managers")]

    public UIManager UIManager;
    public Spawner  Spawner;
    public TimeController TimeController;



    [Header("DifficultySettings")]

    public DifficultySettings easy;
    public DifficultySettings medium;
    public DifficultySettings hard;
    public DifficultySettings endless;
    public DifficultySettings ActiveDifficulty;



   [Header("Level Timers")]
    public float easyTimer = 150.0f;
    public float mediumTimer = 120.0f;
    public float hardTimer = 90.0f;
    public float endlessTimer = 3600.0f;


    void Awake()
    {

        if (Instance != null) // Check if an instance already exists
        {
            
            Destroy(gameObject); // Destroy this instance if it is a duplicate
            return;
        }


        Instance = this; 

        DontDestroyOnLoad(gameObject);
        
        
    }

    void Start()
    {
        UIManager = UIManager.GetComponent<UIManager>();
        Spawner = Spawner.GetComponent<Spawner>();
        TimeController = TimeController.GetComponent<TimeController>();
       

    }

    public void SetDifficulty(string difficulty)
    {
        
        switch (difficulty)
        {
            case "Easy":
                DifficultyMode("Easy" , easyTimer , easy);
                Debug.Log("Easy Mode");
                break;
            case "Medium":
                DifficultyMode("Medium", mediumTimer, medium);
                Debug.Log("Medium Mode");
                break;
            case "Hard":
                DifficultyMode("Hard", hardTimer, hard);
                Debug.Log("Hard Mode");
                break;
            case "Endless":
                DifficultyMode("Endless", endlessTimer, endless);
                Debug.Log("Endless Mode");
                break;



        }
        
        

    }

    private void DifficultyMode(string difficulty , float levelTime , DifficultySettings currentdifficulty)
    {
        Debug.Log("Difficulty Set To: " + difficulty);
        isGameStarted = true;
        UIManager.timer.gameObject.SetActive(true);
        TimeController.InitializeTimer(levelTime);
        Spawner.StartSpawning();
        ActiveDifficulty = currentdifficulty;
    }

        
        
        
   




}
