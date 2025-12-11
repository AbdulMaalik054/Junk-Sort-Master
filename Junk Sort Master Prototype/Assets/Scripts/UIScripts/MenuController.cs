using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [Header("Panels")]
    [SerializeField] private CanvasGroup mainMenuPanel;
    [SerializeField] private CanvasGroup difficultyPanel;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;

    private void Awake()
    {
        Instance = this;

        ShowPanel(mainMenuPanel);
        HidePanel(difficultyPanel);

        WireButtons();
    }

    private void WireButtons()
    {
        playButton?.onClick.AddListener(ShowDifficultyMenu);
        easyButton?.onClick.AddListener(() => SelectDifficulty(0));
        mediumButton?.onClick.AddListener(() => SelectDifficulty(1));
        hardButton?.onClick.AddListener(() => SelectDifficulty(2));
    }

    // ----------------------------------------------------
    // PANEL LOGIC
    // ----------------------------------------------------
    private void ShowPanel(CanvasGroup cg)
    {
        cg.gameObject.SetActive(true);
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    private void HidePanel(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        // We no longer disable the GameObject here!
    }

    // ----------------------------------------------------
    // MENUS
    // ----------------------------------------------------
    public void ShowDifficultyMenu()
    {
        HidePanel(mainMenuPanel);
        ShowPanel(difficultyPanel);
    }

    public void SelectDifficulty(int index)
    {
        GameManager.Instance.SelectDifficulty(index);
        GameManager.Instance.StartGame();

        // Hide menus when game starts
        HidePanel(mainMenuPanel);
        HidePanel(difficultyPanel);
    }

    public void ReturnToMainMenu()
    {
        ShowPanel(mainMenuPanel);
        HidePanel(difficultyPanel);
    }
}
