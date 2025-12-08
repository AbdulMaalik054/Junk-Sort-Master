using UnityEngine;

public class JunkBin : MonoBehaviour
{
    public JunkType correctjunkType;
    public ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        Junk junk = other.GetComponent<Junk>();

        if (junk == null)
            return;

        bool isCorrect = (junk.junkType == correctjunkType);

        if (isCorrect)
        {
            other.gameObject.SetActive(false);

            scoreManager.IncreaseStreak();
            scoreManager.AddCorrectScore();  // multiplier applied internally
        }
        else
        {
            other.gameObject.SetActive(false);

            scoreManager.ResetStreak();
            scoreManager.AddWrongPenalty();  // does NOT multiply negative score
        }
    }

}
