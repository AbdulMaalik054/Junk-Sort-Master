using System.Collections.Generic;
using UnityEngine;

public class FloatingTextPool : MonoBehaviour
{
    public static FloatingTextPool Instance;

    [Header("Pool")]
    [SerializeField] private FloatingText prefab;
    [SerializeField] private int initialSize = 10;

    // Parent rect (assign your UI Canvas transform here)
    [SerializeField] public RectTransform floatingTextParent;

    private Queue<FloatingText> pool = new Queue<FloatingText>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (floatingTextParent == null)
        {
            // try to find Canvas in scene
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null) floatingTextParent = canvas.transform as RectTransform;
        }

        for (int i = 0; i < initialSize; i++)
        {
            var t = Instantiate(prefab, floatingTextParent);
            t.gameObject.SetActive(false);
            pool.Enqueue(t);
        }
    }

    public FloatingText Get()
    {
        FloatingText ft;
        if (pool.Count > 0)
        {
            ft = pool.Dequeue();
        }
        else
        {
            ft = Instantiate(prefab, floatingTextParent);
        }

        ft.transform.SetParent(floatingTextParent, false);
        ft.gameObject.SetActive(true);
        return ft;
    }

    public void ReturnToPool(FloatingText ft)
    {
        ft.gameObject.SetActive(false);
        pool.Enqueue(ft);
    }
}
