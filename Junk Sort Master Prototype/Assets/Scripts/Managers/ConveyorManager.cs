using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    public static ConveyorManager Instance;

    [Header("Movement")]
    [SerializeField] private Vector3 moveDirection = Vector3.back;
    [SerializeField] private float currentSpeed = 0f;
    private float maxSpeed = 5f;
    private float acceleration = 1f;

    [Header("Material Scrolling")]
    [SerializeField] private Renderer beltRenderer;
    [SerializeField] private float uvMultiplier = 0.25f;

    private Material beltMaterial;
    private bool conveyorRunning = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (beltRenderer != null)
            beltMaterial = beltRenderer.material;
    }

    private void Update()
    {
        if (!conveyorRunning) return;

        // Smooth acceleration up to max speed
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }

        // Belt UV scrolling effect
        if (beltMaterial != null)
        {
            float offset = Time.time * currentSpeed * uvMultiplier;

            if (beltMaterial.HasProperty("_BaseMap"))
                beltMaterial.SetTextureOffset("_BaseMap", new Vector2(0, offset));
            else if (beltMaterial.HasProperty("_MainTex"))
                beltMaterial.SetTextureOffset("_MainTex", new Vector2(0, offset));
        }
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
    // PUBLIC API — matches GameManager requirements
    // ---------------------------------------------------------

    /// <summary>
    /// Sets start speed, max speed, and acceleration based on difficulty.
    /// </summary>
    public void SetDifficulty(float startSpeed, float maxSpeed, float accel)
    {
        this.currentSpeed = Mathf.Max(0f, startSpeed);
        this.maxSpeed = Mathf.Max(0f, maxSpeed);
        this.acceleration = Mathf.Max(0f, accel);
    }

    /// <summary>
    /// Starts conveyor motion (with ramping).
    /// </summary>
    public void StartConveyor()
    {
        conveyorRunning = true;
    }

    /// <summary>
    /// Stops conveyor. Optionally resets speed to zero.
    /// </summary>
    public void StopConveyor(bool resetSpeed = false)
    {
        conveyorRunning = false;

        if (resetSpeed)
            currentSpeed = 0f;
    }

    public void ResetConveyor()
    {
        currentSpeed = 0f;
        conveyorRunning = false;
    }
}
