using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BulbEmissionController : MonoBehaviour
{
    private static readonly int SmallRedId = Shader.PropertyToID("_SmallRedIntensity");
    private static readonly int SmallGreenId = Shader.PropertyToID("_SmallGreenIntensity");
    private static readonly int BigRedId = Shader.PropertyToID("_BigRedIntensity");
    private static readonly int BigGreenId = Shader.PropertyToID("_BigGreenIntensity");

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

    private void Write(int propertyId, float value)
    {
        if (!initialized)
            Initialize();

        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat(propertyId, value);
        _renderer.SetPropertyBlock(_mpb);
    }

    public void SetSmallRed(float intensity) => Write(SmallRedId, intensity);
    public void SetSmallGreen(float intensity) => Write(SmallGreenId, intensity);
    public void SetBigRed(float intensity) => Write(BigRedId, intensity);
    public void SetBigGreen(float intensity) => Write(BigGreenId, intensity);

    public void ResetAll()
    {
        Write(SmallRedId, 0f);
        Write(SmallGreenId, 0f);
        Write(BigRedId, 0f);
        Write(BigGreenId, 0f);
    }
}
