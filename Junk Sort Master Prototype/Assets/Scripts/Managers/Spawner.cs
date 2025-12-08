using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float spawnPlasticInterval = 5f;
    [SerializeField] float spawnPaperInterval = 10f;
    [SerializeField] float minSpawnRange ;
    [SerializeField] float maxSpawnRange ;
     
    Vector3 spawnOffset ;


    

    private void Start()
    {
        RandomPositionAlingX(spawnOffset);
        StartSpawning();
        SpawnPaperObject();

    }

    private Vector3 RandomPositionAlingX(Vector3 newOffset)
    {
        Vector3 offset;
        offset.z = Random.Range(minSpawnRange, maxSpawnRange);
        newOffset = new Vector3(0, 0, (transform.position.z + offset.z));
        
        return newOffset;

    }

    public void StartSpawning()
    {
        if(GameManager.Instance.isGameStarted == true)
        {
            InvokeRepeating(nameof(SpawnPlasticObject), 0f, spawnPlasticInterval);
            InvokeRepeating(nameof(SpawnPaperObject), 2f, spawnPaperInterval);

        }
    }

    void SpawnPlasticObject()
    {
        GameObject obj = PlasticPooler.Instance.GetPooledObject();

        obj.transform.position = transform.position + RandomPositionAlingX(spawnOffset);
        obj.transform.rotation = Quaternion.identity;

        obj.SetActive(true);
    }

    void SpawnPaperObject()
    {
        GameObject obj = PaperPooler.Instance.GetPooledObject();

        obj.transform.position = transform.position + RandomPositionAlingX(spawnOffset);
        obj.transform.rotation = Quaternion.identity;

        obj.SetActive(true);
    }

}
