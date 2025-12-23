using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BulbEmissionController : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();

        // Always initialize
        _renderer.GetPropertyBlock(_mpb);
        ApplyDefaults();
        _renderer.SetPropertyBlock(_mpb);
    }

    void ApplyDefaults()
    {
        // All bulbs OFF by default
        _mpb.SetFloat("_SmallGreenIntensity", 1f);
        _mpb.SetFloat("_SmallRedIntensity", 1f);
        _mpb.SetFloat("_BigGreenIntensity", 0f);
        _mpb.SetFloat("_BigRedIntensity", 0f);

        // Rim emission defaults
        _mpb.SetFloat("_RimStrength", 25.0f);
        _mpb.SetFloat("_RimPower", 4f);
    }

    public void SetSmallGreen(float intensity)
    {
        _renderer.GetPropertyBlock(_mpb); // CRITICAL
        _mpb.SetFloat("_SmallGreenIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }

    public void SetSmallRed(float intensity)
    {
        _renderer.GetPropertyBlock(_mpb); // CRITICAL
        _mpb.SetFloat("_SmallRedIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }

    public void SetBigGreen(float intensity)
    {
        _renderer.GetPropertyBlock(_mpb); // CRITICAL
        _mpb.SetFloat("_BigGreenIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }

    public void SetBigRed(float intensity)
    {
        _renderer.GetPropertyBlock(_mpb); // CRITICAL
        _mpb.SetFloat("_BigRedIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }
}
