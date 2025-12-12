using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<string, Queue<GameObject>> poolDict;

    private void Awake()
    {
        Instance = this;
        poolDict = new Dictionary<string, Queue<GameObject>>();
    }

    public void CreatePool(JunkType type, int amountPerPrefab)
    {
        for (int i = 0; i < type.prefabVariants.Length; i++)
        {
            GameObject prefab = type.prefabVariants[i];
            string poolKey = GetKey(type, i);

            Queue<GameObject> newQueue = new Queue<GameObject>();
            poolDict.Add(poolKey, newQueue);

            for (int j = 0; j < amountPerPrefab; j++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);

                // Assign junk type to each variant
                obj.GetComponent<JunkItem>().junkType = type;

                newQueue.Enqueue(obj);
            }
        }
    }

    public GameObject GetFromPool(JunkType type)
    {
        int randomIndex = Random.Range(0, type.prefabVariants.Length);
        string poolKey = GetKey(type, randomIndex);

        Queue<GameObject> pool = poolDict[poolKey];

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        pool.Enqueue(obj);

        return obj;
    }

    private string GetKey(JunkType type, int index)
    {
        return type.typeName + "_" + index;
    }
    // -----------------------------------------------------------
    // 🔥 NEW METHOD: Reset all pooled objects when restarting game
    // -----------------------------------------------------------
    public void ResetAllObjects()
    {
        foreach (var kvp in poolDict)
        {
            Queue<GameObject> pool = kvp.Value;

            foreach (GameObject obj in pool)
            {
                if (obj == null) continue;

                // Deactivate
                obj.SetActive(false);

                // Reset physics
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                // Reset rotation (optional)
                obj.transform.localRotation = Quaternion.identity;

                // If JunkItem has any custom reset logic, call it
                JunkItem item = obj.GetComponent<JunkItem>();
                if (item != null)
                {
                    item.ResetState();   // We'll create this method if needed
                }
            }
        }
        }
    }
