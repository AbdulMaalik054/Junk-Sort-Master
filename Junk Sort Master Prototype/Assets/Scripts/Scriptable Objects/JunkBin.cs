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

        if (junk.junkType == correctjunkType)
        {
            other.gameObject.SetActive(false);
            scoreManager.AddScore(+1);
        }
        else
        {
            other.gameObject.SetActive(false);
            scoreManager.AddScore(-1);
        }
    }
}
