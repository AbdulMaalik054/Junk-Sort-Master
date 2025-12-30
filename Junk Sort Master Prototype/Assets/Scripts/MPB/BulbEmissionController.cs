using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BulbEmissionController : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;

    

    [Header("Flashing Settings")]
    public float flashFrequency = 2f;

    private bool flashRed = false;
    private float flashTimer;

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

    private void Update()
    {
        if (flashRed)
        {
            flashTimer += Time.deltaTime * flashFrequency;
            float intensity = Mathf.PingPong(flashTimer, 1f) * 40f; // adjustable

            SetBigRedIntensity(intensity);
            SetBigGreenIntensity(0f);
        }
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

    // Big Bulbs (Shader Control Only)
    private void SetBigGreenIntensity(float intensity)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat("_BigGreenIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }

    private void SetBigRedIntensity(float intensity)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat("_BigRedIntensity", intensity);
        _renderer.SetPropertyBlock(_mpb);
    }
    public void SetBigGreen(bool active)
    {
        flashRed = false;
        flashTimer = 0f;

        SetBigGreenIntensity(active ? 40f : 0f);
        SetBigRedIntensity(0f);
    }

    public void FlashBigRed(bool flashing)
    {
        flashRed = flashing;
        flashTimer = 0f;

        if (!flashing)
        {
            SetBigRedIntensity(0f);
            SetBigGreenIntensity(40f);
        }
    }
}
