using System.Collections.Generic;
using System.Linq;

public static class GoalHintEvaluator
{
    // Tracks globally which hints have been shown
    private static readonly HashSet<string> shownHintIds = new();

    /// <summary>
    /// Evaluate hints given a context.
    /// Returns only hints that trigger and have not been shown if OnlyOnce = true.
    /// </summary>
    public static List<GoalHint> Evaluate(IEnumerable<GoalHint> availableHints, GoalHintContext context)
    {
        var result = new List<GoalHint>();

        foreach (var hint in availableHints)
        {
            if (!hint.Trigger.Matches(context))
                continue;

            if (hint.Trigger.OnlyOnce && shownHintIds.Contains(hint.Id))
                continue;

            result.Add(hint);

            if (hint.Trigger.OnlyOnce)
                shownHintIds.Add(hint.Id);
        }

        return result;
    }

    /// <summary>
    /// Reset per session (call at level start or game start)
    /// </summary>
    public static void ResetSession()
    {
        shownHintIds.Clear();
    }
}
