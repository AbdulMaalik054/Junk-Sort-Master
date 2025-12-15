using UnityEngine;

public class JunkBin : MonoBehaviour
{
    public JunkType correctType;
    public ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponentInChildren<JunkItem>();
        if (junk == null) return;

        
        // Check if the items list contains the correct junk type
        bool isCorrect = junk.junkType == correctType;

        SortingResolver.Instance.Resolve(

            isCorrect? SortResult.Correct : SortResult.Wrong , junk

            );

    }

}

