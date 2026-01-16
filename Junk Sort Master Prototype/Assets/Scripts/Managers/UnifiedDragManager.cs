using UnityEngine;
using UnityEngine.EventSystems;

public class UnifiedDragManager : MonoBehaviour
{
    [SerializeField] private float snapDistance = 1.0f;
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
        // If the pointer is over an actual UI Button/Element, don't allow starting a drag
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.touchSupported && Input.touchCount > 0)
            HandleTouch();
        else
            HandleMouse();
    }

    private void HandleMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) TryBeginDrag(ray);
        
        if (Input.GetMouseButton(0)) currentDrag?.Drag(ray);

        if (Input.GetMouseButtonUp(0))
        {
            if (currentDrag != null) EndDragWithSnap();
        }
    }

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
        Vector3 draggedPos = new Vector3(currentDrag.transform.position.x, 0, currentDrag.transform.position.z);

        foreach (BinMarker bin in allBins)
        {
            Vector3 binPos = new Vector3(bin.transform.position.x, 0, bin.transform.position.z);
            float dist = Vector3.Distance(draggedPos, binPos);
            if (dist <= closestDistance)
            {
                closestDistance = dist;
                nearestBin = bin;
            }
        }

        if (nearestBin != null)
            currentDrag.transform.position = nearestBin.transform.position;

        currentDrag = null;
    }

    private void HandleTouch()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

        Ray ray = mainCamera.ScreenPointToRay(touch.position);
        switch (touch.phase)
        {
            case TouchPhase.Began: TryBeginDrag(ray); break;
            case TouchPhase.Moved: currentDrag?.Drag(ray); break;
            case TouchPhase.Ended: if (currentDrag != null) EndDragWithSnap(); break;
        }
    }
}