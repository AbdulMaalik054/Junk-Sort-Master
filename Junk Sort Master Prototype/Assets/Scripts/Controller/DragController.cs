using UnityEngine;

public class DragController : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Plane dragPlane;

    public void BeginDrag(Vector3 hitPoint)
    {
        // Create a drag plane facing the camera
        dragPlane = new Plane(-Camera.main.transform.forward, transform.position);

        // Calculate offset
        offset = transform.position - hitPoint;

        isDragging = true;
    }

    public void Drag(Ray ray)
    {
        if (!isDragging) return;

        // Ray-plane intersection math
        if (dragPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            transform.position = worldPoint + offset;
        }
    }

    public void EndDrag()
    {
        isDragging = false;
    }
}
