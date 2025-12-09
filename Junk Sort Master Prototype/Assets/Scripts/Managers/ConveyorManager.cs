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
        {
            // Instantiate unique material so scrolling affects only this belt
            beltMaterial = beltRenderer.material;
        }
    }

    private void Update()
    {
        if (!conveyorRunning)
            return;

        // Accelerate smoothly to max speed
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            
        }

        // Apply UV scrolling
        if (beltMaterial != null)
        {
            float offset = Time.time * currentSpeed * uvMultiplier;

            // URP Lit uses _BaseMap, Standard shader uses _MainTex
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
