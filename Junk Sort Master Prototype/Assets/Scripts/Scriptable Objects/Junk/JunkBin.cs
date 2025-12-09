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

        if (junk.junkType == correctjunkType)
            GameManager.Instance.AddScore(1);
        else
            GameManager.Instance.WrongSortingPenalty();

        other.gameObject.SetActive(false);

    }

}
