using UnityEngine;

public class LevelGoalBootstrapper : MonoBehaviour
{
    private void Start()
    {
        
        SeedGoals();
    }

    private void SeedGoals()
    {
        GoalSystem.RegisterGoals(new[]
        {
            new GoalRuntimeData
            {
                Id = 1,
                IsPrimary = true,
                DisplayName = "Sort 10 Items",
                CurrentValue = 0,
                TargetValue = 10
            },
            new GoalRuntimeData
            {
                Id = 2,
                IsPrimary = false,
                DisplayName = "No Mistakes",
                CurrentValue = 0,
                TargetValue = 1
            }
        });
    }
}
