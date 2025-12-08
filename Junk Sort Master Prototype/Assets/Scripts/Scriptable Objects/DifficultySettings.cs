using UnityEngine;

[System.Serializable]
public class DifficultySettings
{
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float rampDuration = 20f;   // time to reach max
    public AnimationCurve rampCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


}
