using System.Collections.Generic;
using UnityEngine;

public class PlasticPooler : MonoBehaviour
{
    public static PlasticPooler Instance;
    public GameObject typePlastic;
    public int poolSize = 10;

    private List<GameObject> pool;

    private void Awake()
    {
        Instance = this;

        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(typePlastic);
            
            obj.SetActive(false);
            
            pool.Add(obj);
            
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        GameObject newObj = Instantiate(typePlastic);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }
}
