using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BulbEmissionController : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;

    

    [Header("Big Bulbs Flash Settings")]
    public float flashFrequency = 2f;
    private bool flashRed = false;
   
    [Header("Small Bulbs Flash Settings")]
    private bool flashSmallRed = false;
    private bool flashSmallGreen = false;

    [Header("Bulbs Flash Settings")]
    private float bigRedTimer;
    private float smallRedTimer;
    private float smallGreenTimer;

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
            bigRedTimer += Time.deltaTime * flashFrequency;
            float intensity = Mathf.PingPong(bigRedTimer, 1f) * 40f;
            SetBigRedIntensity(intensity);
            SetBigGreenIntensity(0f);
        }

        if (flashSmallRed)
        {
            smallRedTimer += Time.deltaTime * flashFrequency;
            float intensity = Mathf.PingPong(smallRedTimer, 1f) * 40f;
            SetSmallRed(intensity);
        }

        if (flashSmallGreen)
        {
            smallGreenTimer += Time.deltaTime * flashFrequency;
            float intensity = Mathf.PingPong(smallGreenTimer, 1f) * 40f;
            SetSmallGreen(intensity);
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
        

        SetBigGreenIntensity(active ? 40f : 0f);
        SetBigRedIntensity(0f);
    }

    public void FlashBigRed(bool flashing)
    {
        flashRed = flashing;
        bigRedTimer = 0f;

        if (!flashing)
        {
            SetBigRedIntensity(0f);
            SetBigGreenIntensity(40f);
        }
    }
    public void FlashSmallRed(bool flashing)
    {
        flashSmallRed = flashing;
        smallRedTimer = 0f;

        if (!flashing)
            SetSmallRed(0f);
    }

    public void FlashSmallGreen(bool flashing)
    {
        flashSmallGreen = flashing;
        smallGreenTimer = 0f;

        if (!flashing)
            SetSmallGreen(0f);
    }

    public void ResetAll()
    {
        flashRed = false;
        flashSmallRed = false;
        flashSmallGreen = false;

        bigRedTimer = 0f;
        smallRedTimer = 0f;
        smallGreenTimer = 0f;

        SetSmallRed(0f);
        SetSmallGreen(0f);
        SetBigRedIntensity(0f);
        SetBigGreenIntensity(0f);
    }


}
