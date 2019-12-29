using Entity.Player;
using UnityEngine;

namespace Effects
{
    public class FogOfSanity : MonoBehaviour
    {
        [SerializeField][Tooltip("Reference to the fog generating mesh")]
        private Renderer fogOfSanityMesh;
    
        [SerializeField][Tooltip("Main camera in the scene")]
        private Camera mainCamera;

        [SerializeField] [Tooltip("Intensity of the pulse animation")]
        private float pulseIntensity;
    
        [SerializeField] [Tooltip("Determines how fast pulse animation is")]
        private float pulseFrequency;

        // radius of the fog
        private float _baseRadius;
    
        // properties needed for the pulse animation
        private float _currentRadius;
        private bool _isGrowing;
   
        // shader properties
        private static readonly int PlayerPosition = Shader.PropertyToID("_Center");
        private static readonly int FogRadius = Shader.PropertyToID("_FogRadius");

        private void Awake()
        {
            _baseRadius = 500;
            _currentRadius = _baseRadius;
            _isGrowing = true;
        }
    
        private void Update()
        {
            CenterFog();
            Pulse(pulseIntensity);
        }

        /// <summary>
        /// Attaches the center of the fog to the center of the screen
        /// </summary>
        private void CenterFog()
        {
            Ray rayToPlayer = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

            if (Physics.Raycast(rayToPlayer, out RaycastHit hit, 1000))
            {
                fogOfSanityMesh.material.SetVector(PlayerPosition, hit.point);
            }
        }

        /// <summary>
        /// Scales the fog radius up and down
        /// </summary>
        /// <param name="intensity">Distance between min and max size of the radius</param>
        private void Pulse(float intensity)
        {
            fogOfSanityMesh.material.SetFloat(FogRadius, _currentRadius);
        
            if (_isGrowing)
            {
                _currentRadius += Time.deltaTime / pulseFrequency;
                if (_currentRadius >= _baseRadius + intensity) _isGrowing = false;
            }
            else
            {
                _currentRadius -= Time.deltaTime / (pulseFrequency / 2);
                if (_currentRadius <= _baseRadius) _isGrowing = true;
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
    
        private void OnSanityUpdated(int value) => UpdateFogRadius(value);

        private void UpdateFogRadius(int sanityLevel)
        {
            if (sanityLevel > 50)
            {
                _baseRadius = 500;
            }
            else
            {
                _baseRadius = sanityLevel * 2;
            }
        }
    }
}
