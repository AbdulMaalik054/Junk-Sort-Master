using UnityEngine;

public static class PlayerMistakeTracker
{
    public static int TotalMistakes { get; private set; }

    public static void Reset()
    {
        TotalMistakes = 0;
    }

    public static void RegisterMistake()
    {
        TotalMistakes++;
    }
}

