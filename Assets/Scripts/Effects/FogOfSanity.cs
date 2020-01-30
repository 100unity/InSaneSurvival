using Entity.Player;
using Managers;
using UnityEngine;

namespace Effects
{
    public class FogOfSanity : MonoBehaviour
    {
        [SerializeField] [Tooltip("Reference to the fog generating mesh")]
        private Renderer fogOfSanityMesh;

        [SerializeField] [Tooltip("Intensity of the pulse animation")]
        private float pulseIntensity;

        [SerializeField] [Tooltip("Determines how fast pulse animation is. (Although called frequency, the smaller the value the faster it pulses)")]
        private float pulseDelay;

        [SerializeField] [Tooltip("Determines the the center area size of the sanity texture")]
        private AnimationCurve apertureRadiusCurve;

        [SerializeField] [Tooltip("Determines the opacity of the center area size")]
        private AnimationCurve apertureOpacityCurve;

        [SerializeField] [Tooltip("Determines the opacity of the whole sanity mesh")]
        private AnimationCurve materialOpacityCurve;

        [SerializeField] [Tooltip("Animation curve for the fog density")]
        private AnimationCurve fogDensityCurve;

        // radius of the fog
        private float _apertureBaseRadius;

        // properties needed for the pulse animation
        private float _apertureCurrentRadius;
        private bool _isGrowing;

        // shader properties
        private static readonly int PlayerPosition = Shader.PropertyToID("_Center");
        private static readonly int ApertureRadius = Shader.PropertyToID("_FogRadius");
        private static readonly int ApertureAlpha = Shader.PropertyToID("_ApertureAlpha");

        private GameObject _player;
        
        // Fog density; from 0 - 1
        private float _newSanity = 1f;
        
        private void Awake()
        {
            _apertureBaseRadius = 100;
            _apertureCurrentRadius = _apertureBaseRadius;
            _isGrowing = true;
        	_player = PlayerManager.Instance.GetPlayer();
            UpdateFogRadius(100);
        }

        private void Update()
        {
            CenterFog();
            Pulse(pulseIntensity);
            float fogDensity = fogDensityCurve.Evaluate(_newSanity);
            if(Mathf.Abs(RenderSettings.fogDensity - fogDensity) > 0.01f)
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, fogDensity, Time.deltaTime * pulseDelay);
        }

        /// <summary>
        /// Attaches the center of the fog to the center of the screen
        /// </summary>
        private void CenterFog()
        {
            fogOfSanityMesh.material.SetVector(PlayerPosition, _player.transform.position);
        }

        /// <summary>
        /// Scales the fog radius up and down
        /// </summary>
        /// <param name="intensity">Distance between min and max size of the radius</param>
        private void Pulse(float intensity)
        {
            fogOfSanityMesh.material.SetFloat(ApertureRadius, _apertureCurrentRadius);

            if (_isGrowing)
            {
                _apertureCurrentRadius += Time.deltaTime / pulseDelay;
                if (_apertureCurrentRadius >= _apertureBaseRadius + intensity) _isGrowing = false;
            }
            else
            {
                _apertureCurrentRadius -= Time.deltaTime / (pulseDelay / 2);
                if (_apertureCurrentRadius <= _apertureBaseRadius) _isGrowing = true;
            }
        }

        private void OnEnable()
        {
            PlayerState.OnPlayerSanityUpdate += OnSanityUpdated;
        }

        private void OnDisable()
        {
            PlayerState.OnPlayerSanityUpdate -= OnSanityUpdated;
        }

        private void OnSanityUpdated(int value)
        {
            _newSanity = value / 100f;
            UpdateFogRadius(value);
        }

        private void UpdateFogRadius(int sanityLevel)
        {
            float sanity = (float) sanityLevel / 100;

            _apertureBaseRadius = apertureRadiusCurve.Evaluate(sanity) * 100;

            float apertureOpacity = apertureOpacityCurve.Evaluate(sanity);
            fogOfSanityMesh.material.SetFloat(ApertureAlpha, Mathf.Lerp(1, 10, apertureOpacity));

            float materialOpacity = materialOpacityCurve.Evaluate(sanity);
            Color fogColor = fogOfSanityMesh.material.color;
            fogColor.a = Mathf.Lerp(0.2f, 1f, materialOpacity);
            fogOfSanityMesh.material.color = fogColor;
        }
    }
}