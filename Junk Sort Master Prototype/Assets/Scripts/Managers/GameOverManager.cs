using UnityEngine;

public class GameOverManager : MonoBehaviour
{
   public static GameOverManager Instance;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsGameOver => isGameOver;

    public void TriggerGameOver()
    {
        if (!isGameOver) return;
        
        isGameOver = true;

        //stop game systems

        ConveyorManager.Instance.StopConveyor(false);
        Spawner.Instance.StopSpawning();
        DragController.DisableDrag = true;

       

        // Update UI
        UIManager.Instance.ShowGameOverPanel(
            ScoreManager.Instance.Score,
            ScoreManager.Instance.GetStreak(),
            ScoreManager.Instance.GetMultiplier()
        );

    }

}
