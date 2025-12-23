using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 moveDirection = Vector3.back;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 1f;

    [Header("Material Scrolling")]
    [SerializeField] private Renderer beltRenderer;
    [SerializeField] private float uvMultiplier = .04f;

    private Material beltMaterial;
    private bool conveyorRunning = false;

    private void Awake()
    {
        if (beltRenderer != null)
            beltMaterial = beltRenderer.sharedMaterial;
    }
    private void FixedUpdate()
    {
        if (!conveyorRunning)
            return;

        // Accelerate smoothly to max speed
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            Debug.Log(currentSpeed);
        }
    }


    private void Update()
    {


        if (!conveyorRunning || beltMaterial == null)
            return;
        beltMaterial.SetFloat("_Speed", currentSpeed * uvMultiplier);
        

    }

    private void OnTriggerStay(Collider other)
    {
        if (!conveyorRunning) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && !rb.isKinematic)
        {
            Vector3 displacement =
                moveDirection.normalized * currentSpeed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + displacement);
            
        }
    }

    // ---------------------------------------------------------
    // PUBLIC API
    // ---------------------------------------------------------

    /// <summary>
    /// Set speed ranges based on difficulty.
    /// </summary>
    public void SetDifficulty(float startSpeed, float maxSpeed, float accel)
    {
        this.currentSpeed = Mathf.Max(0f, startSpeed);
        this.maxSpeed = Mathf.Max(0f, maxSpeed);
        this.acceleration = Mathf.Max(0f, accel);
    }

    // ---------------------------------------------------------
    // UPDATING CONVEYOR
    // ---------------------------------------------------------

    public void UpdateMaxSpeed(float newMaxSpeed)
    {
        maxSpeed = Mathf.Max(0f, newMaxSpeed);
    }

    public void UpdateAcceleration(float newAcceleration)
    {
        acceleration = Mathf.Max(0f, newAcceleration);
    }



    // ---------------------------------------------------------
    // LINKING GameManager
    // ---------------------------------------------------------

    /// <summary>
    /// Begin conveyor movement
    /// </summary>
    public void StartConveyor()
    {
        conveyorRunning = true;
        
    }

    /// <summary>
    /// Stop conveyor movement
    /// </summary>
    public void StopConveyor(bool resetSpeed)
    {
        conveyorRunning = false;
        
        if (resetSpeed)
            currentSpeed = 0f;
    }

    /// <summary>
    /// Fully reset conveyor for a new game round
    /// </summary>
    public void ResetConveyor()
    {
        currentSpeed = 0f;
        conveyorRunning = false;
    }

    
}
