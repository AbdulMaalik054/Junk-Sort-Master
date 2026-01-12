using UnityEngine;
using UnityEngine.UI;


public class UIActionButton : MonoBehaviour
{
    public enum UIButtonAction
    {
        Start,
        Exit,
        Restart,
        Home,
        Play,
        Resume,
        Pause,
        BackToMenu,
        BackToPause,
        BackOnePanel
    }

    public enum DifficultyButtons
    {
        Easy,
        Medium,
        Hard,
        Endless
    }

    public enum ButtonModes
    {
        UIButtonAction,
        DifficultyButtons
    }


    [Header("Configuration")]
    public ButtonModes CurrentMode;

    [HideInInspector]
    [Header("Action Buttons")]
    public UIButtonAction action;

    [HideInInspector]
    [Header("Difficulty Buttons")]
    public DifficultyButtons difficulty;

    private Button uiButton;
    private void Awake()
    {
        uiButton = GetComponent<Button>();
        if (uiButton != null)
        {
            // Assign the main handler method to the button's click event
            uiButton.onClick.AddListener(HandleButtonClick);
        }
        
    }

    public void HandleButtonClick()
    {
        switch (CurrentMode)
        {

            case ButtonModes.UIButtonAction:
                InvokeAction(action);
                break;
            case ButtonModes.DifficultyButtons:
                SelectDifficulty(difficulty);
                break;
        }


    }
    private void InvokeAction(UIButtonAction action)
    {
        switch (action)
        {
            case UIButtonAction.Start:
                MenuController.Instance.ShowDifficultyMenu();
                break;

            case UIButtonAction.Exit:
                GameManager.Instance.Quit();
                break;

            case UIButtonAction.Restart:
                GameManager.Instance.RestartGame();
                break;

            case UIButtonAction.Home:
                GoalUIController.Instance.EnterMainMenu();
                GameManager.Instance.ReturnToMenu();
                break;
            case UIButtonAction.Play:
                GameManager.Instance.StartGame();
                break;

            case UIButtonAction.Resume:
                GameManager.Instance.TogglePause();
                break;

            case UIButtonAction.Pause:
                GameManager.Instance.TogglePause();
                break;

            case UIButtonAction.BackToMenu:
                GameManager.Instance.ReturnToMenu();
                break;

            case UIButtonAction.BackToPause:
                UIManager.Instance.ShowPauseMenu();
                break;

            

            //case UIButtonAction.BackOnePanel:
            //    UIManager.Instance.GoBackOnePanel();
            //    break;
            default:

                break;
        }
    }

    private void SelectDifficulty(DifficultyButtons difficulty)
    {
        switch(difficulty)
        {
            case DifficultyButtons.Easy:
                MenuController.Instance.SelectDifficulty(0);
                break;

            case DifficultyButtons.Medium:
                MenuController.Instance.SelectDifficulty(1);
                break;

            case DifficultyButtons.Hard:
                MenuController.Instance.SelectDifficulty(2);
                break;

            case DifficultyButtons.Endless:
                MenuController.Instance.SelectDifficulty(3);
                break;
        }
    }
}
