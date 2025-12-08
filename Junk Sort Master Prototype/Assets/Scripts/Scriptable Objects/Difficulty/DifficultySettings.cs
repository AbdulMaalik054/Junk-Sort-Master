using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings" , menuName = "Game/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    public string difficultyName;

    [Header("Scoring")]

    public int baseScore = 1;          // score for correct junk
    public int wrongPenalty = -1;      // penalty for wrong bin
    public int overflowPenalty = -2;   // penalty for conveyor overflow

    [Header("Conveyer Settibgs")]

    public float minConveyorSpeed = 1f;
    public float maxConveyorSpeed = 3f;
    public float speedRampTime = 30f;   // time to reach max
    public AnimationCurve rampCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


}
