using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Utils;

namespace Entity.Player
{
    public class HealthPostProcessing : MonoBehaviour
    {
        [Tooltip("The intensity curve that scales the red screen effect depending on the player's health.")]
        [SerializeField]
        private AnimationCurve intensityCurve;

        [Tooltip("Maximum frequency of vignette pulsing.")]
        [SerializeField]
        private float maxVignettePulseFrequency;

        [Tooltip("Intensity of the vignette pulse.")]
        [SerializeField]
        [Range(0, 1)]
        private float vignettePulseIntensity;

        [Tooltip("The minimum sanity the player can have in order to display the red screen. If sanity goes below, red screen is not shown, but sanity vignette takes over.")]
        [SerializeField]
        [Range(0, 100)]
        private float sanityCap;

        private Pulser _pulser;
        private int _health;
        private int _sanity;

        private PostProcessVolume _postProcessVolume;
        private Vignette _vignette;
        private float _vignetteBaseIntensity;
        private float _vignettePulse;

        private void Awake()
        {
            _pulser = new Pulser();
            _postProcessVolume = GetComponent<PostProcessVolume>();
            _postProcessVolume.profile.TryGetSettings(out _vignette);
            // get base value
            _vignetteBaseIntensity = _vignette.intensity.value;
            // clamp intensity
            if (vignettePulseIntensity + _vignetteBaseIntensity > 1)
                vignettePulseIntensity = 1 - _vignetteBaseIntensity;
            _health = 100;
            _sanity = 100;
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

        /// <summary>
        /// Applies red screen effect with varying frequency and intensity depending on the player's health if sanity is high enough.
        /// </summary>
        private void Update()
        {
            if (_sanity < sanityCap)
            {
                _postProcessVolume.priority = -1;
                return;
            }
            else if (_postProcessVolume.priority == -1)
            {
                _postProcessVolume.priority = 1;
            }

            float y = intensityCurve.Evaluate(_health / 100f);
            float currentVignettePulseFrequency = maxVignettePulseFrequency * y;

            // pulse
            _vignettePulse = _pulser.Pulse("vignette", _vignettePulse, currentVignettePulseFrequency, vignettePulseIntensity);
            // update with masked value
            _vignette.intensity.value = _vignetteBaseIntensity + y * _vignettePulse;
        }

        private void OnHealthUpdated(int health) => _health = health;
        private void OnSanityUpdated(int sanity) => _sanity = sanity;
    }
}