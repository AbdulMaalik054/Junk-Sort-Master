using UnityEngine;

public class UnifiedDragManager : MonoBehaviour
{
    [Header("Snap Settings")]
    [SerializeField] private float snapDistance = 1.0f;


    private RepairButton activeRepairButton = null;

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
        if (TouchIsOverUI(touch)) return;

       

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
    private bool TouchIsOverUI(Touch touch)
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }


    private void HandleMouse()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Mouse down
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHitRepairButton(ray)) return;
            TryBeginDrag(ray);
        }

        // Mouse held
        if (Input.GetMouseButton(0))
        {
            if (activeRepairButton != null)
                return; // block dragging while repairing

            currentDrag?.Drag(ray);
        }

        // Mouse up
        if (Input.GetMouseButtonUp(0))
        {
            if (activeRepairButton != null)
            {
                activeRepairButton.EndHold();
                activeRepairButton = null;
                return;
            }

            if (currentDrag != null)
                EndDragWithSnap();
        }
    }


    #endregion



    private void TryBeginDrag(Ray ray)
    {
        if (DragController.DisableDrag) return;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 1. Repair button interaction
            if (hit.collider.TryGetComponent(out RepairButton repair))
            {
                repair.BeginHold();
                currentDrag = null; // block junk drag while repairing
                return;
            }

            if (DragController.DisableDrag) return;
            // 2. Junk dragging
            if (hit.collider.TryGetComponent(out DragController drag))
            {
                currentDrag = drag;
                currentDrag.BeginDrag(hit.point);
            }
        }
    }

    
    private void EndDragWithSnap()
    {
        RepairButton[] buttons = FindObjectsByType<RepairButton>(FindObjectsSortMode.None);
        foreach (var button in buttons)
            button.EndHold();
        
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
            
        }
        
        

        currentDrag = null;
    }
    

    private bool TryHitRepairButton(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out RepairButton repair))
            {
                activeRepairButton = repair;
                repair.BeginHold();
                currentDrag = null;
                return true;
            }
        }
        return false;
    }

}
