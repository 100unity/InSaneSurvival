using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Entity.Player.Sanity
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class SanityPostProcessing : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve intensityCurve;

        [SerializeField]
        private float vignettePulseFrequency;

        [SerializeField]
        [Range(0, 1)]
        private float vignettePulseIntensity;

        [SerializeField]
        private float aberrationPulseFrequency;

        [SerializeField]
        [Range(0, 1)]
        private float aberrationPulseIntensity;

        [Range(0,100)]
        private int _sanity;
        private bool[] _isGrowing;

        private PostProcessVolume _postProcessVolume;
        private MotionBlur _motionBlur;

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
            _postProcessVolume.profile.TryGetSettings(out _motionBlur);
            _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
            _postProcessVolume.profile.TryGetSettings(out _vignette);
        }

        private void Start()
        {
            _aberrationBaseIntensity = _chromaticAberration.intensity.value;
            _vignetteBaseIntensity = _vignette.intensity.value;
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
        /// Pulse all pulses.
        /// </summary>
        private void Update()
        {
            PulseAll();
            MaskAll();

            // update values in effects
            _vignette.intensity.value = _currentVignetteIntensity;
            _chromaticAberration.intensity.value = _currentAberrationIntensity;
        }

        private void ClampAll()
        {
            if (vignettePulseIntensity + _vignetteBaseIntensity > 1)
                vignettePulseIntensity = 1 - _vignetteBaseIntensity;
            if (aberrationPulseIntensity + _aberrationBaseIntensity > 1)
                aberrationPulseIntensity = 1 - _aberrationBaseIntensity;
        }

        private void PulseAll()
        {
            _vignettePulse = Pulse(_vignettePulse, vignettePulseFrequency, vignettePulseIntensity, 0);
            _aberrationPulse = Pulse(_aberrationPulse, aberrationPulseFrequency, aberrationPulseIntensity, 1);
        }

        /// <summary>
        /// Masks all pulses depending on the sanity level.
        /// </summary>
        private void MaskAll()
        {
            _currentVignetteIntensity = _vignetteBaseIntensity + intensityCurve.Evaluate(_sanity / 100f) * _vignettePulse;
            _currentAberrationIntensity = _aberrationBaseIntensity + intensityCurve.Evaluate(_sanity / 100f) * _aberrationPulse;
        }

        private float Pulse(float valueToBePulsed, float pulseFrequency, float pulseIntensity, int isGrowing)
        {
            if (_isGrowing[isGrowing])
            {
                valueToBePulsed += Time.deltaTime * pulseFrequency;
                if (valueToBePulsed > pulseIntensity)
                    _isGrowing[isGrowing] = false;
            }
            else
            {
                valueToBePulsed -= Time.deltaTime * (pulseFrequency / 2);
                if (valueToBePulsed <= 0)
                    _isGrowing[isGrowing] = true;
            }
            return valueToBePulsed;
        }

    }
}