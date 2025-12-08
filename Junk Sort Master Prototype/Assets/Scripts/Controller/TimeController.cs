using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    
    public TextMeshProUGUI timerText;
    public float startTime;
    float curretTime;
    public bool startGame = false;
    void Start()
    {

        



    }

    // Update is called once per frame
    void Update()
    {
        
        if (curretTime > 0 && startGame == true)
        {
            curretTime -= Time.deltaTime;
            UpdateTimerDisplay(curretTime);
            

        }
        else
        {

            curretTime = 0;
            UpdateTimerDisplay(curretTime) ;

        }
    }

    void UpdateTimerDisplay(float time)
    {

        int min = Mathf.FloorToInt(time / 60.0f);
        int sec = Mathf.FloorToInt(time % 60.0f);
        timerText.text = string.Format("{0:00}: {1:00}" , min , sec);

    }

    public void InitializeTimer(float time)
    {
        curretTime = time;
        startGame = true;
        UpdateTimerDisplay(curretTime);
    }


}
