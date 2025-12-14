using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FloatingTextPool : MonoBehaviour
{
    public static FloatingTextPool Instance;

    [Header("References (Assign Manually)")]
    public FloatingText floatingTextPrefab;     // prefab with RectTransform + CanvasGroup + TMP
    public RectTransform canvasParent;          // your main UI canvas RectTransform

    [Header("Pool Settings")]
    public int poolSize = 5;

    private Queue<FloatingText> pool = new Queue<FloatingText>();
    private Camera mainCam;
    private Camera uiCam;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCam = Camera.main;

        if (canvasParent == null)
        {
            Debug.LogError("[FloatingTextPool] ERROR: Canvas Parent is NOT assigned!");
            return;
        }

        if (floatingTextPrefab == null)
        {
            Debug.LogError("[FloatingTextPool] ERROR: Floating Text Prefab is NOT assigned!");
            return;
        }
        

        Canvas canvas = canvasParent.GetComponentInParent<Canvas>();
        uiCam = canvas.worldCamera;

        if (uiCam == null)
        {
            Debug.LogError("[FloatingTextPool] UI Camera is not assigned to the Canvas!");
        }
        // Pre-populate pool
        for (int i = 0; i < poolSize; i++)
        {
            var ft = Instantiate(floatingTextPrefab, transform);
            ft.gameObject.SetActive(false);
            pool.Enqueue(ft);
        }
    }

    private FloatingText GetFromPool()
    {
        if (pool.Count > 0)
        {
            var ft = pool.Dequeue();
            ft.gameObject.SetActive(true);
            return ft;
        }

        // Expand pool if necessary
        return Instantiate(floatingTextPrefab, canvasParent);
    }

    public void ReturnToPool(FloatingText ft)
    {
        ft.gameObject.SetActive(false);
        pool.Enqueue(ft);
    }

    // ------------------------------------------------------------
    // PUBLIC: SPAWN FLOATING TEXT AT WORLD POSITION
    // ------------------------------------------------------------
    public void SpawnFloatingText(string message, Color color, Vector3 worldPos)
    {
        if (mainCam == null) mainCam = Camera.main;

        FloatingText ft = GetFromPool();

        // convert world → screen
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);

        // convert screen → anchored canvas pos
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasParent,
            screenPos,
            uiCam,
            out Vector2 anchoredPos
        );

        // assign position
        ft.rect.anchoredPosition = anchoredPos;
        
        // set text and animate
        ft.Init(message, color);
        ft.Play();
    }
}


