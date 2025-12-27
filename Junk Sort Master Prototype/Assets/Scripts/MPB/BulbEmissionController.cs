using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BulbEmissionController : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;

    void OnEnable()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();

        if (_mpb == null)
        {
            _mpb = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_mpb);
            ApplyDefaults();
            _renderer.SetPropertyBlock(_mpb);
        }
    }

    private void ApplyDefaults()
    {
        // Default values
        _mpb.SetFloat("_SmallGreenIntensity", 0f);
        _mpb.SetFloat("_SmallRedIntensity", 0f);
        _mpb.SetFloat("_BigGreenIntensity", 0f);
        _mpb.SetFloat("_BigRedIntensity", 0f);

        _mpb.SetFloat("_RimStrength", 25f);
        _mpb.SetFloat("_RimPower", 4f);
    }

    public void SetSmallGreen(float intensity)
    {
        if (_mpb == null) return;
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat("_SmallGreenIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }

    public void SetSmallRed(float intensity)
    {
        if (_mpb == null) return;
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat("_SmallRedIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }

    public void SetBigGreen(float intensity)
    {
        if (_mpb == null) return;
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat("_BigGreenIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }

    public void SetBigRed(float intensity)
    {
        if (_mpb == null) return;
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat("_BigRedIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }
}
