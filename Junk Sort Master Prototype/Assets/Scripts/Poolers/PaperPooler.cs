using System.Collections.Generic;
using UnityEngine;

public class PaperPooler : MonoBehaviour
{
    public static PaperPooler Instance;
    public GameObject typePaper;
    public int poolSize = 10;

    private List<GameObject> pool;

    private void Awake()
    {
        Instance = this;

        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(typePaper);
            
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

        GameObject newObj = Instantiate(typePaper);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }
}
