using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BulbEmissionController : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;
    private bool initialized = false;

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

        // Ensure material isolation
        if (_renderer.material != null)
            _renderer.material = new Material(_renderer.material);

        initialized = true;
    }

    private void Write(string property, float value)
    {
        if (!initialized)
            Initialize();

        if (_renderer == null || _mpb == null)
            return; // hard safety

        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat(property, value);
        _renderer.SetPropertyBlock(_mpb);
    }

    // SMALL BULBS
    public void SetSmallRed(float intensity)
        => Write("_SmallRedIntensity", intensity);

    public void SetSmallGreen(float intensity)
        => Write("_SmallGreenIntensity", intensity);

    // BIG BULBS
    public void SetBigRed(float intensity)
        => Write("_BigRedIntensity", intensity);

    public void SetBigGreen(float intensity)
        => Write("_BigGreenIntensity", intensity);

    // RESET
    public void ResetAll()
    {
        if (!initialized)
            Initialize();

        Write("_SmallRedIntensity", 0f);
        Write("_SmallGreenIntensity", 0f);
        Write("_BigRedIntensity", 0f);
        Write("_BigGreenIntensity", 0f);
    }
}
