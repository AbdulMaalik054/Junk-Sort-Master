using UnityEngine;

[System.Serializable]
public class HardDifficultySettings
{
    public float minSpeed = 3f;
    public float maxSpeed = 8f;
    public float rampDuration = 10f;   // time to reach max
    public AnimationCurve rampCurve = AnimationCurve.Linear(0, 0, 1, 1);
}
