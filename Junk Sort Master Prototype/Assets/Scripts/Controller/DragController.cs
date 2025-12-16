using UnityEngine;

public enum DragPlaneMode
{
    WorldPlane,
    CameraFacingPlane
}

[RequireComponent(typeof(Collider))]
public class DragController : MonoBehaviour
{
    public static bool DisableDrag = false;

    [Header("Drag Settings")]
    [SerializeField] private DragPlaneMode planeMode = DragPlaneMode.WorldPlane;

    private bool isDragging;
    private Vector3 offset;
    private Plane dragPlane;
    private Camera cam;
    private Collider cachedCollider;
    private float startY;

    private void Awake()
    {
        cam = Camera.main;
        cachedCollider = GetComponent<Collider>();
    }

    public void BeginDrag(Vector3 hitPoint)
    {
        if (DisableDrag) return;

        startY = transform.position.y;

        switch (planeMode)
        {
            case DragPlaneMode.WorldPlane:
                dragPlane = new Plane(Vector3.up, transform.position);
                break;

            case DragPlaneMode.CameraFacingPlane:
                dragPlane = new Plane(-cam.transform.forward, transform.position);
                break;
        }

        offset = transform.position - hitPoint;
        isDragging = true;
        cachedCollider.enabled = false;
    }

    public void Drag(Ray ray)
    {
        if (!isDragging) return;

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPoint = ray.GetPoint(enter);
            Vector3 targetPos = worldPoint + offset;
            targetPos.y = startY; // ✅ stable Y

            transform.position = targetPos;
        }
    }

    public void EndDrag()
    {
        isDragging = false;
        cachedCollider.enabled = true;
    }
}
