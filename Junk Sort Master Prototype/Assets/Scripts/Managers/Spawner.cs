using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    


    [Header("Spawn Intervals")]

    public float spawnObjectInterval = 1.0f;
    //public float plasticInterval = 3f;
    //public float paperInterval = 5f;

    [Header("Spawn Range")]
    [SerializeField] float minZOffset = -1f;
    [SerializeField] float maxZOffset = 1f;

    

    public JunkType[] junkTypes;
    

    private Coroutine spawnRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        foreach (var type in junkTypes)
        {
            PoolManager.Instance.CreatePool(type, type.initialSize);
        }
    }



    // Called by GameManager when game starts
    public void StartSpawning()
    {
        StopSpawning();
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }

    private IEnumerator SpawnLoop()
    {
        
        float spawnTimer = 0f;

        while (true)
        {
            
            spawnTimer += Time.deltaTime;

           

            if (spawnTimer >= spawnObjectInterval)
            {
                SpawnTypeObjects();
                spawnTimer = 0f;
            }

            yield return null;
        }
    }

    private Vector3 GetRandomOffset()
    {
        float z = Random.Range(minZOffset, maxZOffset);
        return new Vector3(0, 0, z);
    }

    // <summary>Adjust spawn intervals at runtime (called by GameManager.ApplyDifficulty)</summary>
    public void SetSpawnRates(float spawnInterval)
    {
        // clamp to safe, non-zero values
        spawnObjectInterval = Mathf.Max(0.001f, spawnInterval);
        Debug.Log($"[Spawner] Spawn interval set to {spawnObjectInterval}");
    }
    private void SpawnTypeObjects()
    {
        JunkType JunKObject = junkTypes[Random.Range(0, junkTypes.Length)];
        GameObject obj = PoolManager.Instance.GetFromPool(JunKObject);
        obj.transform.position = transform.position + GetRandomOffset();
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }
        
       

}
