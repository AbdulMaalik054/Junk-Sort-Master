using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    [SerializeField] private List<ConveyorManager> conveyors;

    public void StartAll()
    {
        foreach (var c in conveyors)
            c.StartConveyor();
          
        
    }

    public void StopAll(bool reset)
    {
        foreach (var c in conveyors)
            c.StopConveyor(reset);
    }

    public void SetDifficulty(float start, float max, float accel)
    {
        foreach (var c in conveyors)
            c.SetDifficulty(start, max, accel);
       Debug.Log(start + " " + max);
    }

    
}
