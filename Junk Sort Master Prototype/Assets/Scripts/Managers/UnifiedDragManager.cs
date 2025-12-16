using UnityEngine;

public class UnifiedDragManager : MonoBehaviour
{
    [Header("Snap Settings")]
    [SerializeField] private float snapDistance = 1.0f;

    [Header("Feedback")]
    [SerializeField] private ParticleSystem correctParticle;
    [SerializeField] private ParticleSystem incorrectParticle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctSFX;
    [SerializeField] private AudioClip incorrectSFX;

    private DragController currentDrag;
    private Camera mainCamera;
    private BinMarker[] allBins;

    private void Awake()
    {
        mainCamera = Camera.main;
        allBins = FindObjectsByType<BinMarker>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (Input.touchSupported && Input.touchCount > 0)
            HandleTouch();
        else
            HandleMouse();
    }

    #region Input Handling

    private void HandleTouch()
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
                if (currentDrag != null)
                    currentDrag.Drag(ray);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (currentDrag != null)
                    EndDragWithSnap();
                break;
        }
    }

    private void HandleMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
            TryBeginDrag(ray);
        else if (Input.GetMouseButton(0) && currentDrag != null)
            currentDrag?.Drag(ray);
        else if (Input.GetMouseButtonUp(0) && currentDrag != null)
            EndDragWithSnap();
    }

    #endregion

    private void TryBeginDrag(Ray ray)
    {
        if (DragController.DisableDrag) return;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out DragController drag))
            {
                currentDrag = drag;
                currentDrag.BeginDrag(hit.point);
            }
        }
    }

    private void EndDragWithSnap()
    {
        currentDrag.EndDrag();

        BinMarker nearestBin = null;
        float closestDistance = snapDistance;

        Vector3 draggedPos = currentDrag.transform.position;
        draggedPos.y = 0f; // Project to plane

        foreach (BinMarker bin in allBins)
        {
            Vector3 binPos = bin.transform.position;
            binPos.y = 0f;

            float dist = Vector3.Distance(draggedPos, binPos);
            if (dist <= closestDistance)
            {
                closestDistance = dist;
                nearestBin = bin;
            }
        }

        if (nearestBin != null)
        {
            currentDrag.transform.position = nearestBin.transform.position;
            PlayCorrectFeedback(nearestBin.transform.position);
        }
        else
        {
            PlayIncorrectFeedback(currentDrag.transform.position);
        }

        currentDrag = null;
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
