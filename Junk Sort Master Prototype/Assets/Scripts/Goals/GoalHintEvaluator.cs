using System.Collections.Generic;

public static class GoalHintEvaluator
{
    private static readonly HashSet<string> shownHintIds = new();

    public static List<GoalHint> Evaluate(
        IEnumerable<GoalHint> availableHints,
        GoalHintContext context
    )
    {
        var result = new List<GoalHint>();

        foreach (var hint in availableHints)
        {
            if (hint.Trigger.Matches(context))
            {
                if (hint.Trigger.OnlyOnce &&
                    shownHintIds.Contains(hint.Id))
                    continue;

                result.Add(hint);

                if (hint.Trigger.OnlyOnce)
                    shownHintIds.Add(hint.Id);
            }
        }

        return result;
    }

    public static void ResetSession()
    {
        shownHintIds.Clear();
    }
}
