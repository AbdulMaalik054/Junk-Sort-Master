using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BulbEmissionController : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;
    private bool initialized;

    private float currentIntensity;
    public float CurrentIntensity => currentIntensity;

    private void Awake()
    {
        Initialize();
        ResetAll();
    }

    private void Initialize()
    {
        if (initialized) return;

        _renderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();

        // DO NOT TOUCH renderer.material
        // Shared material is REQUIRED for instancing

        initialized = true;
    }

    private void Write(string property, float value)
    {
        if (!initialized)
            Initialize();

        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat(property, value);
        _renderer.SetPropertyBlock(_mpb);
    }

    public void SetSmallRed(float intensity) => Write("_SmallRedIntensity", intensity);
    public void SetSmallGreen(float intensity) => Write("_SmallGreenIntensity", intensity);
    public void SetBigRed(float intensity) => Write("_BigRedIntensity", intensity);
    public void SetBigGreen(float intensity) => Write("_BigGreenIntensity", intensity);

    public void ResetAll()
    {
        Write("_SmallRedIntensity", 0f);
        Write("_SmallGreenIntensity", 0f);
        Write("_BigRedIntensity", 0f);
        Write("_BigGreenIntensity", 0f);
    }
}
