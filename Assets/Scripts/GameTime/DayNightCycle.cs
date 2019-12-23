using UnityEngine;
using UnityEngine.Serialization;

namespace GameTime
{
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField][Tooltip("Clock which is managing the game time ")]
        private Clock clock;

        [SerializeField] [Tooltip("Parent of the light sources")]
        private Transform dayNightCycle;
        
        [SerializeField][Tooltip("Sun light source")]
        private Light sunLight;
        
        [SerializeField][Tooltip("base intensity of the sun")]
        private float sunBaseIntensity;

        [SerializeField][Tooltip("A gradient that changes the sun color depending of day time")]
        private Gradient sunColor;

        [SerializeField][Tooltip("tilt of the sun")] [Range(-45f, 45f)]
        private float sunTilt;
        
        
        private float _intensity;

        
        private void Awake()
        {
        }

        void Update()
        {
            RotateSun();
            SunIntensity();
            AdjustSunColor();
        }
        
        private void RotateSun()
        {
            float sunAngle = clock.TimeOfDay * 360f;
            dayNightCycle.transform.localRotation = Quaternion.Euler(new Vector3(sunTilt,0f, sunAngle));
        }

        private void SunIntensity()
        {
            _intensity = Mathf.Clamp01(Vector3.Dot(sunLight.transform.forward, Vector3.down));

            sunLight.intensity = _intensity + sunBaseIntensity;
        }

        private void AdjustSunColor()
        {
            sunLight.color = sunColor.Evaluate(_intensity);
        }
        
    }
}
