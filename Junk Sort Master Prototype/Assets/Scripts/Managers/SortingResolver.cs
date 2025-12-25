using UnityEngine;

public enum SortResult
{
    Correct,
    Wrong,
    Overflow
}

public class SortingResolver : MonoBehaviour
{
    public static SortingResolver Instance;

    [Header("Score Values")]
    public int correctScore = 1;
    public int wrongPenalty = 2;
    public int overflowPenalty = 5;



    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Resolve(SortResult result, JunkItem junk , SortingLaneTracker tracker)
    {
        if (junk == null) return;

        junk.MarkResolved();

        

        Vector3 pos = junk.transform.position;

        switch (result)
        {
            case SortResult.Correct:
                
                ScoreManager.Instance.AddCorrectScore(correctScore, pos);
                tracker.RegisterCorrect();
                break;

            case SortResult.Wrong:
                
                ScoreManager.Instance.AddWrongPenalty(wrongPenalty, pos);
                tracker.RegisterWrong();
                break;

            case SortResult.Overflow:
                
                ScoreManager.Instance.AddOverflowPenalty(overflowPenalty, pos);
                tracker.RegisterOverflow();
                break;
        }

        junk.ReturnToPool();
    }
}
