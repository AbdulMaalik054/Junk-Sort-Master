using UnityEngine;

public class JunkBin : MonoBehaviour
{
    public JunkType correctType;
    public ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponentInChildren<JunkItem>();
        if (junk.IsResolved) return;

        
        // Check if the items list contains the correct junk type
        bool isCorrect = junk.junkType == correctType;

        if (isCorrect)
            SortingResolver.Instance.Resolve(SortResult.Correct , junk);
        else
            SortingResolver.Instance.Resolve(SortResult.Wrong , junk);

    }

}

