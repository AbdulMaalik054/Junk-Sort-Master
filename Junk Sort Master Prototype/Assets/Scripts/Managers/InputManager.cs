using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera cam;
    private DragController currentDrag;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        // -------------------------------
        // 1. On Mouse Down → Begin Drag
        // -------------------------------
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out DragController drag))
                {
                    currentDrag = drag;
                    currentDrag.BeginDrag(hit.point); // pass hit point
                }
            }
        }

        // -------------------------------
        // 2. While Mouse Held → Dragging
        // -------------------------------
        if (Input.GetMouseButton(0) && currentDrag != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            currentDrag.Drag(ray);
        }

        // -------------------------------
        // 3. Mouse Released → End Drag
        // -------------------------------
        if (Input.GetMouseButtonUp(0) && currentDrag != null)
        {
            currentDrag.EndDrag();
            currentDrag = null;
        }
    }
}
