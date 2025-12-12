using UnityEngine;

public class OverflowDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        JunkItem junk = other.GetComponent<JunkItem>();

        if (junk == null)
            return;

        // Overflow = penalty
        ScoreManager.Instance.AddOverflowPenalty(5, transform.position);

        Debug.Log("Overflow! -5 score");

        other.gameObject.SetActive(false);
    }
}
