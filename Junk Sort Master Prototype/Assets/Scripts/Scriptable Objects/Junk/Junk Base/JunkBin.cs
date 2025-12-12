using UnityEngine;

public class JunkBin : MonoBehaviour
{
    public JunkType correctType;
    public ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponent<JunkItem>();
        if (junk == null) return;

        // Check if the items list contains the correct junk type
        bool isCorrect = junk.junkType == correctType;
        if (isCorrect)
        {
            GameManager.Instance.AddScore(1, transform.position);
        }
        else
        {
            GameManager.Instance.WrongSortingPenalty(2, transform.position);
        }

        other.gameObject.SetActive(false);

    }

}
