using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    [Header("Spawn Intervals")]
    public float plasticInterval = 3f;
    public float paperInterval = 5f;

    [Header("Spawn Range")]
    [SerializeField] float minZOffset = -1f;
    [SerializeField] float maxZOffset = 1f;

    private Coroutine spawnRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
        float plasticTimer = 0f;
        float paperTimer = 0f;

        while (true)
        {
            plasticTimer += Time.deltaTime;
            paperTimer += Time.deltaTime;

            if (plasticTimer >= plasticInterval)
            {
                SpawnPlastic();
                plasticTimer = 0f;
            }

            if (paperTimer >= paperInterval)
            {
                SpawnPaper();
                paperTimer = 0f;
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
    public void SetSpawnRates(float plasticInterval, float paperInterval)
    {
        // clamp to safe, non-zero values
        plasticInterval = Mathf.Max(0.05f, plasticInterval);
        paperInterval = Mathf.Max(0.05f, paperInterval);

        this.plasticInterval = plasticInterval;
        this.paperInterval = paperInterval;
    }

    private void SpawnPlastic()
    {
        GameObject obj = PlasticPooler.Instance.GetPooledObject();
        obj.transform.position = transform.position + GetRandomOffset();
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }

    private void SpawnPaper()
    {
        GameObject obj = PaperPooler.Instance.GetPooledObject();
        obj.transform.position = transform.position + GetRandomOffset();
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }
}
