using UnityEngine;

[System.Serializable]
public class MediumDifficultySettings
{
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    public float rampDuration = 15f;   // time to reach max
    public AnimationCurve rampCurve = AnimationCurve.Linear(0, 0, 1, 1);
}
