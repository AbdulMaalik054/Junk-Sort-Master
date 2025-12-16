using Unity.VisualScripting;
using UnityEngine;

public class JunkItem : MonoBehaviour, IPoolable
{
    [HideInInspector] public JunkType junkType;

    private GameObject pooledRoot;
    private string poolKey;
    private Rigidbody rb;
    public bool IsResolved { get; private set; }

    public void Initialize(GameObject root, string key, JunkType type)
    {
        pooledRoot = root;
        poolKey = key;
        junkType = type;
        rb = root.GetComponent<Rigidbody>();
    }

    public void OnSpawn()
    {
        if (rb == null) return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //rb.isKinematic = false;
    }

    public void OnDespawn()
    {
        if (rb == null) return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
       // rb.isKinematic = true;
        ResetState();
    }

    public void MarkResolved()
    {
        IsResolved = true;
    }
    public void ReturnToPool()
    {
        IsResolved = false;
        PoolManager.Instance.ReturnToPool(pooledRoot, poolKey);
    }

    public void ResetState()
    {
        IsResolved = false ;
        //If you later add:
        //Conveyor attachment
        //Bin lock state
        //Drag flags
    }
}
