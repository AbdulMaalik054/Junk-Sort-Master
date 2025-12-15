using UnityEngine;

// IMPORTANT:
// This controller ONLY accepts screen-space positions (pixels).
// Do NOT pass rays, hit points, or world positions.


public class DragController : MonoBehaviour
{
    public static bool DisableDrag = false;

    private bool isDragging;
    private Vector3 offset;
    private Camera cam;

    // Fixed plane height (floor level or item base)
    private const float DragY = 0.3f;

    void Awake()
    {
        cam = Camera.main;
    }

    public void BeginDrag(Vector3 screenPositionPx)
    {
        if (DisableDrag) return;

        Vector3 worldPoint = ScreenToWorldOnPlane(screenPositionPx);
        offset = transform.position - worldPoint;

        isDragging = true;
    }

    public void Drag(Vector3 screenPosition)
    {
        if (!isDragging) return;

        Vector3 worldPoint = ScreenToWorldOnPlane(screenPosition);
        transform.position = worldPoint + offset;
    }

    public void EndDrag()
    {
        isDragging = false;
    }

    private Vector3 ScreenToWorldOnPlane(Vector3 screenPos)
    {
        screenPos.z = cam.nearClipPlane;
        Vector3 world = cam.ScreenToWorldPoint(screenPos);
        world.y = DragY;
        return world;
    }
}
