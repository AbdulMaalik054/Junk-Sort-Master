using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    //--------General Buttons-------
    public Button startButton;
    public Button backButton;

    //------Diffuclties-------------

    public Button easy;
    public Button medium;
    public Button hard;
    public Button endless;

    //--------------------------------

    public GameObject difficultyMenu;
    public GameObject timer;
    public GameObject scoreMenu;
    


    void Start()
    {
       
        difficultyMenu.SetActive(false);
        backButton.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        scoreMenu.gameObject.SetActive(false);
    }

 

    public void StartGame()
    {
        
        startButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        difficultyMenu.SetActive(true);
        GameManager.Instance.isGameStarted = true;

    }

    public void BackButton()
    {
        
        difficultyMenu.SetActive(false);
        startButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);

    }

    public void SetEasyDifficulty()
    {
        ActivateDifficulty("Easy");

        Debug.Log("Easy is activated");
    }

    public void SetMediumDifficulty()
    {
        ActivateDifficulty("Medium");
        Debug.Log("Medium is activated");
    }
    public void SetHardDifficulty()
    {
        ActivateDifficulty("Hard");
        Debug.Log("Hard is activated");
    }
    public void SetEndlessDifficulty()
    {
        ActivateDifficulty("Endless");
        Debug.Log("Endless is activated");
    }


    void ActivateDifficulty(string currentdifficulty)
    {
        difficultyMenu.SetActive(false);
        backButton.gameObject.SetActive(false);
        string difficulty = currentdifficulty;
        GameManager.Instance.isGameStarted = true;
        GameManager.Instance.SetDifficulty(difficulty);
        scoreMenu.gameObject.SetActive(true);

    }

    

}
