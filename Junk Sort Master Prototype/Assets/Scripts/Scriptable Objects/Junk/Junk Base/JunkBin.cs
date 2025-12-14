using UnityEngine;

public class JunkBin : MonoBehaviour
{
    public JunkType correctType;
    public ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponentInChildren<JunkItem>();
        if (junk == null) return;

        Debug.Log(other.name);
        // Check if the items list contains the correct junk type
        bool isCorrect = junk.junkType == correctType;
        if (isCorrect)
        {
            ScoreManager.Instance.AddCorrectScore(1,junk.transform.position);
        }
        else
        {
            ScoreManager.Instance.AddWrongPenalty(2, junk.transform.position);
        }

        other.gameObject.SetActive(false);

    }

}
