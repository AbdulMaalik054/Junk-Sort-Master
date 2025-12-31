using UnityEngine;
using UnityEngine.EventSystems;

public class RepairButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int laneIndex;
    public float holdDuration = 1f;

    private bool holding = false;
    private float timer = 0f;

    public void OnPointerDown(PointerEventData data) => BeginHold();
    public void OnPointerUp(PointerEventData data) => EndHold();

    public void BeginHold()
    {
        holding = true;
        timer = 0f;
        Debug.Log($"Started holding repair button for lane {laneIndex}");
    }

    public void EndHold()
    {
        if (!holding) return;
        holding = false;
        timer = 0f;
        Debug.Log($"Stopped holding repair button for lane {laneIndex}");
    }

    private void Update()
    {
        if (!holding) return;

        timer += Time.deltaTime;
        if (timer >= holdDuration)
        {
            if (BreakdownManager.Instance != null)
            {
                Debug.Log($"Attempting to repair lane {laneIndex} via RepairButton.");
                BreakdownManager.Instance.RepairLane(laneIndex);
            }
            else
            {
                Debug.LogWarning("BreakdownManager.Instance is null!");
            }

            EndHold();
        }
    }
}
