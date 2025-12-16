using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private readonly Dictionary<string, Queue<GameObject>> pools = new();

    private readonly HashSet<GameObject> activeObjects = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ----------------------------------------------------
    // Create pools
    // ----------------------------------------------------
    public void CreatePool(JunkType type, int initialSize)
    {
        for (int i = 0; i < type.prefabVariants.Length; i++)
        {
            string poolKey = GetKey(type, i);

            if (pools.ContainsKey(poolKey))
                continue;

            Queue<GameObject> queue = new();
            pools.Add(poolKey, queue);

            for (int j = 0; j < initialSize; j++)
            {
                queue.Enqueue(CreateInstance(type, i, poolKey));
            }
        }
    }

    // ----------------------------------------------------
    // Spawn
    // ----------------------------------------------------
    public GameObject GetFromPool(JunkType type)
    {
        int index = Random.Range(0, type.prefabVariants.Length);
        string key = GetKey(type, index);

        if (!pools.TryGetValue(key, out var queue))
        {
            Debug.LogError($"Pool missing for key {key}");
            return null;
        }

        if (queue.Count == 0)
        {
            queue.Enqueue(CreateInstance(type, index, key));
        }

        GameObject root = queue.Dequeue();
        root.SetActive(true);

        activeObjects.Add(root);

        root.GetComponentInChildren<IPoolable>()?.OnSpawn();
        return root;

    }

    // ----------------------------------------------------
    // Despawn
    // ----------------------------------------------------
    public void ReturnToPool(GameObject root, string poolKey)
    {
        if (root == null || !pools.ContainsKey(poolKey))
            return;

        activeObjects.Remove(root);

        root.GetComponentInChildren<IPoolable>()?.OnDespawn();
        root.SetActive(false);

        pools[poolKey].Enqueue(root);
    }


    // ----------------------------------------------------
    // Internal creation (NO runtime parents)
    // ----------------------------------------------------
    private GameObject CreateInstance(JunkType type, int index, string poolKey)
    {
        GameObject prefab = type.prefabVariants[index];

        GameObject root = Instantiate(prefab);
        root.transform.SetParent(transform, false);
        root.SetActive(false);

        JunkItem item = root.GetComponentInChildren<JunkItem>(true);
        if (item == null)
        {
            Debug.LogError($"Prefab '{prefab.name}' has no JunkItem.");
            return root;
        }

        item.Initialize(root, poolKey, type);
        return root;
    }

    private string GetKey(JunkType type, int index)
    {
        return $"{type.typeName}_{index}";
    }

    // ----------------------------------------------------
    // Reset all pooled + active objects (LEVEL RESTART SAFE)
    // ----------------------------------------------------
    public void ResetPools()
    {
        // Force-return all active junk first
        foreach (var root in activeObjects.ToArray())
        {
            if (root == null) continue;

            JunkItem item = root.GetComponentInChildren<JunkItem>(true);
            if (item != null)
            {
                item.ReturnToPool();
            }
        }

        activeObjects.Clear();

        // Reset pooled objects
        foreach (var kvp in pools)
        {
            foreach (GameObject root in kvp.Value)
            {
                if (root == null) continue;

                root.GetComponentInChildren<IPoolable>()?.OnDespawn();
                root.SetActive(false);
            }
        }
    }


}
