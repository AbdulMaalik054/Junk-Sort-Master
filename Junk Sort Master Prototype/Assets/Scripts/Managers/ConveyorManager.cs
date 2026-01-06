using UnityEngine;
public class ConveyorManager : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 moveDirection = Vector3.back;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float maxSpeed = 5f;
   // [SerializeField] private float acceleration = 4f;

    [Header("Material Scrolling")]
    [SerializeField] private Renderer beltRenderer;
    [SerializeField] private float uvMultiplier = .04f;
    [SerializeField] private string shaderPropertyName = "_Offset"; // Double check this!
    private float currentUVOffset = 0f; // Tracks the total distance scrolled

    [Header("Movement")]
    [SerializeField] private float smoothTime = 0.5f; // Time in seconds to reach max speed
    private float speedVelocity; // Required for SmoothDamp to track internal velocity

    private Material beltMaterial;
    private bool conveyorRunning = false;

    // Property Block variables
    private MaterialPropertyBlock propBlock;
    private int shaderID;
    private void Awake()
    {

        //if (beltRenderer != null)
        //    beltMaterial = beltRenderer.sharedMaterial;
        propBlock = new MaterialPropertyBlock();
        // Converting the string name to an ID is much faster for the CPU
        shaderID = Shader.PropertyToID(shaderPropertyName);
    }
    private void Update()
    {
        
        // 1. Determine target speed
        float targetSpeed = conveyorRunning ? maxSpeed : 0f;

        // 2. SmoothDamp handles the acceleration curve for you
        // It automatically calculates the necessary acceleration to reach targetSpeed 
        // in approximately 'smoothTime' seconds.
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothTime);

        // 3. CUMULATIVE OFFSET CALCULATION
        // Instead of Time * Speed, we add to the offset every frame
        // We use Time.deltaTime so it's frame-rate independent
        currentUVOffset += (-1 * currentSpeed * uvMultiplier) * Time.deltaTime;
        
        // 4. Keep the float value small to prevent precision issues over long play sessions
        // This loops the offset between 0 and 1
        currentUVOffset %= 1.0f;

        if (beltRenderer != null)
        {
            // Get the current block from the renderer
            beltRenderer.GetPropertyBlock(propBlock);

            // Set our specific value in the block
            propBlock.SetFloat(shaderID, currentUVOffset);

            // Push the block back to the renderer
            beltRenderer.SetPropertyBlock(propBlock);
        }
        
    }
    

    private void OnTriggerStay(Collider other)
    {
        
        if (currentSpeed <= 0.001f) return;
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && !rb.isKinematic)
        {
            Vector3 displacement =
                moveDirection.normalized * (-1 *currentSpeed ) * Time.fixedDeltaTime;

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
        //this.acceleration = Mathf.Max(0f, accel);
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
        //acceleration = Mathf.Max(0f, newAcceleration);
    }



    // ---------------------------------------------------------
    // LINKING GameManager
    // ---------------------------------------------------------

    /// <summary>
    /// Begin conveyor movement
    /// </summary>
    public void StartConveyor() => conveyorRunning = true;


    /// <summary>
    /// Stop conveyor movement
    /// </summary>
    public void StopConveyor(bool resetSpeed)
    {
        conveyorRunning = false;
        if (resetSpeed)
        {
            currentSpeed = 0f;
            currentUVOffset = 0f;
            speedVelocity = 0f;

            // Push the reset values one last time
            if (beltRenderer != null)
            {
                beltRenderer.GetPropertyBlock(propBlock);
                propBlock.SetFloat(shaderID, 0f);
                beltRenderer.SetPropertyBlock(propBlock);
            }
        }
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
