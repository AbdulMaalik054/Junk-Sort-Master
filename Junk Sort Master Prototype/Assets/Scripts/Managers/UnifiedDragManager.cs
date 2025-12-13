using UnityEngine;

public class UnifiedDragManager : MonoBehaviour
{
    [Header("Snap Settings")]
    public float snapDistance = 1.0f;           // Distance threshold for snapping
    public LayerMask binLayerMask;              // Layer for bins
    public ParticleSystem correctParticle;      // Optional particle for correct sorting
    public ParticleSystem incorrectParticle;    // Optional particle for incorrect sorting
    public AudioSource audioSource;             // Optional audio source
    public AudioClip correctSFX;
    public AudioClip incorrectSFX;

    private DragController currentDrag;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchSupported && Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    #region Touch Input
    private void HandleTouchInput()
    {
        Touch touch = Input.GetTouch(0);
        Ray ray = mainCamera.ScreenPointToRay(touch.position);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                TryBeginDrag(ray);
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (currentDrag != null) currentDrag.Drag(ray);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (currentDrag != null)
                {
                    EndDragWithSnap();
                }
                break;
        }
    }
    #endregion

    #region Mouse Input
    private void HandleMouseInput()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            TryBeginDrag(ray);
        }
        else if (Input.GetMouseButton(0))
        {
            if (currentDrag != null) currentDrag.Drag(ray);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (currentDrag != null)
            {
                EndDragWithSnap();
            }
        }
    }
    #endregion

    private void TryBeginDrag(Ray ray)
    {
        if (DragController.DisableDrag) return;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            DragController drag = hit.collider.GetComponent<DragController>();
            if (drag != null)
            {
                currentDrag = drag;
                currentDrag.BeginDrag(hit.point);
                BringToFront(currentDrag.gameObject);
            }
        }
    }

    private void EndDragWithSnap()
    {
        // First end the drag
        currentDrag.EndDrag();

        // Check for nearby bins
        Collider[] bins = Physics.OverlapSphere(currentDrag.transform.position, snapDistance, binLayerMask);
        bool snapped = false;

        foreach (Collider bin in bins)
        {
            // Optionally, you could verify bin type here
            float distance = Vector3.Distance(currentDrag.transform.position, bin.transform.position);
            if (distance <= snapDistance)
            {
                currentDrag.transform.position = bin.transform.position; // Snap
                snapped = true;

                // Trigger correct particle + SFX hook
                if (correctParticle != null)
                {
                    ParticleSystem p = Instantiate(correctParticle, bin.transform.position, Quaternion.identity);
                    p.Play();
                }
                if (audioSource != null && correctSFX != null)
                {
                    audioSource.PlayOneShot(correctSFX);
                }

                break; // Snap only to the first bin in range
            }
        }

        if (!snapped)
        {
            // Optional: trigger incorrect particle + SFX
            if (incorrectParticle != null)
            {
                ParticleSystem p = Instantiate(incorrectParticle, currentDrag.transform.position, Quaternion.identity);
                p.Play();
            }
            if (audioSource != null && incorrectSFX != null)
            {
                audioSource.PlayOneShot(incorrectSFX);
            }
        }

        currentDrag = null; // Reset
    }

    private void BringToFront(GameObject go)
    {
        // Optional: raise Z or sorting order so dragged object is visually on top
        Vector3 pos = go.transform.position;
        pos.z = -5f; // adjust based on your camera setup
        go.transform.position = pos;
    }
}
