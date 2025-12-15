// IPoolable.cs
// ----------------------------------------------------
// Any pooled object implements this to receive
// spawn / despawn lifecycle callbacks from the pool.
// ----------------------------------------------------
public interface IPoolable
{
    void OnSpawn();     // Called when taken from pool
    void OnDespawn();   // Called when returned to pool
}
