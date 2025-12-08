using UnityEngine;
using UnityEngine.EventSystems;

public class ConveyorManager : MonoBehaviour
{
    
    float currentTime = 0f;
    Vector3 moveDirection = Vector3.right;
    public Renderer belt;

    public float CurrentSpeed { get; private set; }


    private void Start()
    {
        
        belt = gameObject.GetComponent<Renderer>(); 
    }
    void Update()
    {
        var diff = GameManager.Instance.ActiveDifficulty;
        if (diff == null) return;

        currentTime += Time.deltaTime;

        float t = Mathf.Clamp01(currentTime / diff.speedRampTime);
        float curveValue = diff.rampCurve.Evaluate(t);

        CurrentSpeed = Mathf.Lerp(diff.minConveyorSpeed, diff.maxConveyorSpeed, curveValue);

        // Apply to shader
        float conveyorSpeed = CurrentSpeed / 50f;
        belt.material.SetFloat("_Speed", conveyorSpeed);
    }

    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
            rb.linearVelocity = moveDirection * CurrentSpeed;
    }
}
