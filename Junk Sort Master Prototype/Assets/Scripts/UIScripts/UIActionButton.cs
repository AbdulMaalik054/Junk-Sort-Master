using UnityEngine;
using UnityEngine.UI;

public enum UIButtonAction
{
    Restart,
    Home,
    Resume,
    Pause,
    BackToMenu,
    BackToPause,
    BackOnePanel
}

public class UIActionButton : MonoBehaviour
{
    [SerializeField] private UIButtonAction action;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(InvokeAction);
    }

    private void InvokeAction()
    {
        switch (action)
        {
            case UIButtonAction.Restart:
                GameManager.Instance.RestartGame();
                break;

            case UIButtonAction.Home:
                GameManager.Instance.ReturnToMenu();
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
        }
    }
}
