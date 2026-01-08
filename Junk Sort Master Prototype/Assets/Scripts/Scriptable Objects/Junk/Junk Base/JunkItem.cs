using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
   
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
        if (TryGetComponent(out Collider col)) col.enabled = true;
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
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
    }

    public void AutoFlyTo(JunkBin targetBin)
    {
        if (TryGetComponent(out Collider col)) col.enabled = false;
        if (TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;

        StartCoroutine(FlyAnimation(targetBin.transform.position));
    }


    private IEnumerator FlyAnimation(Vector3 targetPos)
    {
        float duration = 0.35f;
        Transform root = pooledRoot.transform;
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            root.position = Vector3.Lerp(start, targetPos, elapsed / duration);
            yield return null;
        }

        var bin = SortingRegistry.Instance.GetBinForItem(this);
        if (bin != null)
        {
            SortingLaneTracker tracker = GetComponent<SortingLaneTracker>();
            bool correct = (junkType == bin.correctType);

            SortingResolver.Instance.Resolve(
                correct ? SortResult.Correct : SortResult.Wrong,
                this,
                tracker
            );
        }
    }


}
