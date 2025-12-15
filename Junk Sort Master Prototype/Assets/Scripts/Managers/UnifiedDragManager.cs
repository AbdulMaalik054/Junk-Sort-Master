using UnityEngine;

public class UnifiedDragManager : MonoBehaviour
{
    [Header("Snap Settings")]
    public float snapDistance = 1.0f;

    [Header("Optional Feedback")]
    public ParticleSystem correctParticle;
    public ParticleSystem incorrectParticle;
    public AudioSource audioSource;
    public AudioClip correctSFX;
    public AudioClip incorrectSFX;

    private DragController currentDrag;
    private Camera mainCamera;
    private BinMarker[] allBins;

    private void Awake()
    {
        mainCamera = Camera.main;

        // Automatically find all bins in the scene
        allBins = FindObjectsByType<BinMarker>(FindObjectsSortMode.None);
    }

    private void Update()
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

        switch (touch.phase)
        {
            case TouchPhase.Began:
                TryBeginDrag(touch.position);
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (currentDrag != null)
                    currentDrag.Drag(touch.position);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (currentDrag != null)
                    EndDragWithSnap();
                break;
        }
    }

    #endregion

    #region Mouse Input
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryBeginDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && currentDrag != null)
        {
            currentDrag.Drag(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && currentDrag != null)
        {
            EndDragWithSnap();
        }
    }

#endregion


    private void TryBeginDrag(Vector3 screenPosition)
    {
        if (DragController.DisableDrag)
            return;

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out DragController drag))
            {
                currentDrag = drag;
                currentDrag.BeginDrag(screenPosition);
                BringToFront(currentDrag.transform);
            }
        }
    }


    private void EndDragWithSnap()
    {
        currentDrag.EndDrag();

        BinMarker nearestBin = null;
        float closestDistance = snapDistance;

        foreach (var bin in allBins)
        {
            float dist = Vector3.Distance(currentDrag.transform.position, bin.transform.position);
            if (dist <= closestDistance)
            {
                closestDistance = dist;
                nearestBin = bin;
            }
        }

        if (nearestBin != null)
        {
            // Snap to bin center
            currentDrag.transform.position = nearestBin.transform.position;

            PlayCorrectFeedback(nearestBin.transform.position);
        }
        else
        {
            PlayIncorrectFeedback(currentDrag.transform.position);
        }

        currentDrag = null;
    }

    private void BringToFront(Transform target)
    {
        if (target.TryGetComponent(out Renderer r))
        {
            r.sortingOrder = 10;
        }
    }


    #region Feedback
    private void PlayCorrectFeedback(Vector3 position)
    {
        if (correctParticle != null)
            Instantiate(correctParticle, position, Quaternion.identity);

        if (audioSource != null && correctSFX != null)
            audioSource.PlayOneShot(correctSFX);
    }

    private void PlayIncorrectFeedback(Vector3 position)
    {
        if (incorrectParticle != null)
            Instantiate(incorrectParticle, position, Quaternion.identity);

        if (audioSource != null && incorrectSFX != null)
            audioSource.PlayOneShot(incorrectSFX);
    }
    #endregion
}
