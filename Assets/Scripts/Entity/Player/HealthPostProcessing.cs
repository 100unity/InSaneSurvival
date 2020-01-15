using Entity.Player;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HealthPostProcessing : MonoBehaviour
{
    [Tooltip("The intensity curve that scales the red screen effect depending on the player's health.")]
    [SerializeField]
    private AnimationCurve intensityCurve;

    [Tooltip("Frequency of vignette pulsing.")]
    [SerializeField]
    private float vignettePulseFrequency;

    [Tooltip("Intensity of the vignette pulse.")]
    [SerializeField]
    [Range(0, 1)]
    private float vignettePulseIntensity;

    [Tooltip("The minimum sanity the player can have in order to display the red screen. If sanity goes below, red screen is not shown, but sanity vignette takes over.")]
    [SerializeField]
    [Range(0, 100)]
    private float sanityCap;
    
    private int _health;
    private int _sanity;
    // have different frequencies for different effects
    private bool _isGrowing;

    private PostProcessVolume _postProcessVolume;

    private Vignette _vignette;
    private float _vignetteBaseIntensity;
    private float _currentVignetteIntensity;
    private float _vignettePulse;

    private void Awake()
    {
        _postProcessVolume = GetComponent<PostProcessVolume>();
        _postProcessVolume.profile.TryGetSettings(out _vignette);
        _vignetteBaseIntensity = _vignette.intensity.value;
        if (vignettePulseIntensity + _vignetteBaseIntensity > 1)
            vignettePulseIntensity = 1 - _vignetteBaseIntensity;
        _health = 100;
    }

    private void OnEnable()
    {
        PlayerState.OnPlayerHealthUpdate += OnHealthUpdated;
        PlayerState.OnPlayerSanityUpdate += OnSanityUpdated;
    }

    private void OnDisable()
    {
        PlayerState.OnPlayerHealthUpdate -= OnHealthUpdated;
        PlayerState.OnPlayerSanityUpdate += OnSanityUpdated;
    }

    private void Update()
    {
        if (_sanity < sanityCap)
            return;

        _vignettePulse = Pulse(_vignettePulse, vignettePulseFrequency, vignettePulseIntensity, ref _isGrowing);
        float y = intensityCurve.Evaluate(_health / 100f);
        _currentVignetteIntensity = _vignetteBaseIntensity + y * _vignettePulse;
        Debug.Log(_health);
        _vignette.intensity.value = _currentVignetteIntensity;
    }

    private void OnHealthUpdated(int health) => _health = health;
    private void OnSanityUpdated(int sanity) => _sanity = sanity;

    /// <summary>
    /// Pulse a value with a certain frequency and intensity.
    /// </summary>
    /// <param name="valueToBePulsed">The value pulsing should be applied to.</param>
    /// <param name="pulseFrequency">The frequency of the pulse.</param>
    /// <param name="pulseIntensity">The intensity of the pulse.</param>
    /// <param name="boolIndex">Index of the boolean to be used for creating the pulse effect.</param>
    /// <returns></returns>
    private float Pulse(float valueToBePulsed, float pulseFrequency, float pulseIntensity, ref bool isGrowing)
    {
        if (isGrowing)
        {
            valueToBePulsed += Time.deltaTime * pulseFrequency;
            if (valueToBePulsed > pulseIntensity)
                isGrowing = false;
        }
        else
        {
            valueToBePulsed -= Time.deltaTime * (pulseFrequency / 2);
            if (valueToBePulsed <= 0)
                isGrowing = true;
        }
        return valueToBePulsed;
    }
}
