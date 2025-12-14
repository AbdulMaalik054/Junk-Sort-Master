using UnityEngine;

public class OverflowDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponent<JunkItem>();
        Vector3 scoreOffset = new Vector3(-10 , 0 ,0);
        if (junk == null)
            return;

        // Overflow = penalty
        ScoreManager.Instance.AddOverflowPenalty(5, junk.transform.position + scoreOffset);

        Debug.Log("Overflow! -5 score");

        other.gameObject.SetActive(false);
    }
}
