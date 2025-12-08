using UnityEngine;

public class OverflowDetector : MonoBehaviour
{
    public ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        Junk junk = other.GetComponent<Junk>();

        if (junk == null)
            return;

        // Overflow = penalty
        scoreManager.ResetStreak();
        scoreManager.AddOverflowPenalty();

        Debug.Log("Overflow! -1 score");

        other.gameObject.SetActive(false);
    }
}
