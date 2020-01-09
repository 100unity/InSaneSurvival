using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Entity.Player.Sanity
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class SanityPostProcessing : MonoBehaviour
    {
        [Tooltip("The intensity curve that scales all effects depending on the sanity.")]
        [SerializeField]
        private AnimationCurve intensityCurve;

        [Tooltip("The saturation of the color grading filter.")]
        [SerializeField]
        private int saturation;

        [Tooltip("The contrast of the color grading filter.")]
        [SerializeField]
        private int contrast;

        [Tooltip("The higher the shutter angle, the more motion blurr.")]
        [SerializeField]
        private int maxShutterAngle;

        [Tooltip("Frequency of vignette pulsing.")]
        [SerializeField]
        private float vignettePulseFrequency;

        [Tooltip("Intensity of the vignette pulse.")]
        [SerializeField]
        [Range(0, 1)]
        private float vignettePulseIntensity;

        [Tooltip("Frequency of chromatic aberration pulsing.")]
        [SerializeField]
        private float aberrationPulseFrequency;

        [Tooltip("Intensity of chromatic aberration pulse.")]
        [SerializeField]
        [Range(0, 1)]
        private float aberrationPulseIntensity;


        [Range(0,100)]
        private int _sanity;
        // have different frequencies for different effects
        private bool[] _isGrowing;

        //filters:
        private PostProcessVolume _postProcessVolume;

        private ColorGrading _colorGrading;
        private int _currentSaturation;
        private int _currentContrast;

        private MotionBlur _motionBlur;
        private int _baseShutterAngle;
        private int _currentShutterAngle;

        private Vignette _vignette;
        [Range(0, 1)]
        private float _vignetteBaseIntensity;
        [Range(0, 1)]
        private float _currentVignetteIntensity;
        private float _vignettePulse;

        private ChromaticAberration _chromaticAberration;
        [Range(0, 1)]
        private float _aberrationBaseIntensity;
        [Range(0, 1)]
        private float _currentAberrationIntensity;
        private float _aberrationPulse;

        
        private void Awake()
        {
            _isGrowing = new bool[3];
            _postProcessVolume = GetComponent<PostProcessVolume>();
            _postProcessVolume.profile.TryGetSettings(out _colorGrading);
            _postProcessVolume.profile.TryGetSettings(out _motionBlur);
            _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
            _postProcessVolume.profile.TryGetSettings(out _vignette);
            _sanity = 100; // init with 100, since the first sent out event doesn't seem to be received here
        }

        /// <summary>
        /// Remembers base intensities (resp. shutter angle).
        /// </summary>
        private void Start()
        {
            _aberrationBaseIntensity = _chromaticAberration.intensity.value;
            _vignetteBaseIntensity = _vignette.intensity.value;
            _baseShutterAngle = (int) _motionBlur.shutterAngle.value;
            ClampAll();
        }

        private void OnEnable()
        {
            PlayerState.OnPlayerSanityUpdate += (int sanity) => _sanity = sanity;
        }

        private void OnDisable()
        {
            PlayerState.OnPlayerSanityUpdate -= (int sanity) => _sanity = sanity;
        }

        /// <summary>
        /// Pulses all pulses. Masks them. Sets current values in post processing profile.
        /// </summary>
        private void Update()
        {
            PulseAll();
            MaskAll();

            // update values in effects
            _colorGrading.saturation.value = _currentSaturation;
            _colorGrading.contrast.value = _currentContrast;
            _motionBlur.shutterAngle.value = _currentShutterAngle;
            _vignette.intensity.value = _currentVignetteIntensity;
            _chromaticAberration.intensity.value = _currentAberrationIntensity;
        }
        
        private void ClampAll()
        {
            if (vignettePulseIntensity + _vignetteBaseIntensity > 1)
                vignettePulseIntensity = 1 - _vignetteBaseIntensity;
            if (aberrationPulseIntensity + _aberrationBaseIntensity > 1)
                aberrationPulseIntensity = 1 - _aberrationBaseIntensity;
            Mathf.Clamp(maxShutterAngle, 0, 360);
            Mathf.Clamp(saturation, -100, 100);
            Mathf.Clamp(contrast, -100, 100);
        }

        /// <summary>
        /// Imitates a different pulse for vignette andd aberration.
        /// </summary>
        private void PulseAll()
        {
            _vignettePulse = Pulse(_vignettePulse, vignettePulseFrequency, vignettePulseIntensity, 0);
            _aberrationPulse = Pulse(_aberrationPulse, aberrationPulseFrequency, aberrationPulseIntensity, 1);
        }

        /// <summary>
        /// Masks all values depending on the sanity level and the <see cref="intensityCurve"/>.
        /// </summary>
        private void MaskAll()
        {
            float y = intensityCurve.Evaluate(_sanity / 100f);
            _currentSaturation = (int)(y * saturation);
            _currentContrast = (int)(y * contrast);
            _currentShutterAngle = (int)(_baseShutterAngle + y * maxShutterAngle);
            _currentVignetteIntensity = _vignetteBaseIntensity + y * _vignettePulse;
            _currentAberrationIntensity = _aberrationBaseIntensity + y * _aberrationPulse;
        }

        /// <summary>
        /// Pulse a value with a certain frequency and intensity.
        /// </summary>
        /// <param name="valueToBePulsed">The value pulsing should be applied to.</param>
        /// <param name="pulseFrequency">The frequency of the pulse.</param>
        /// <param name="pulseIntensity">The intensity of the pulse.</param>
        /// <param name="boolIndex">Index of the boolean to be used for creating the pulse effect.</param>
        /// <returns></returns>
        private float Pulse(float valueToBePulsed, float pulseFrequency, float pulseIntensity, int boolIndex)
        {
            if (_isGrowing[boolIndex])
            {
                valueToBePulsed += Time.deltaTime * pulseFrequency;
                if (valueToBePulsed > pulseIntensity)
                    _isGrowing[boolIndex] = false;
            }
            else
            {
                valueToBePulsed -= Time.deltaTime * (pulseFrequency / 2);
                if (valueToBePulsed <= 0)
                    _isGrowing[boolIndex] = true;
            }
            return valueToBePulsed;
        }
    }
}