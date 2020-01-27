using UnityEngine;

namespace GameTime
{
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] [Tooltip("Clock which is managing the game time ")]
        private Clock clock;

        [SerializeField] [Tooltip("Parent of the light sources")]
        private Transform dayNightCycle;

        [SerializeField] [Tooltip("Sun light source")]
        private Light sunLight;

        [SerializeField] [Tooltip("base intensity of the sun")]
        private float sunBaseIntensity;

        [SerializeField] [Tooltip("A gradient that changes the sun color depending of day time")]
        private Gradient sunColor;

        [SerializeField] [Tooltip("tilt of the sun (and moon)")] [Range(-45f, 45f)]
        private float tilt; // could vary depending seasons in the future

        [SerializeField] [Tooltip("A gradient that changes the fog color depending of day time")]
        private Gradient fogColor;

        private float _intensity;

        private void Update()
        {
            RotateSun();
            SunIntensity();
            AdjustSunColor();
        }

        /// <summary>
        /// Rotates sun/moon based on the current time of the day
        /// </summary>
        private void RotateSun()
        {
            float zAngle = clock.TimeOfDay * 360f;
            dayNightCycle.transform.localRotation = Quaternion.Euler(new Vector3(tilt, 0f, zAngle));
        }

        /// <summary>
        /// Scales the light intensity of the sun based on the projection of its normal/ray onto the y-axis
        /// </summary>
        private void SunIntensity()
        {
            _intensity = Mathf.Clamp01(Vector3.Dot(sunLight.transform.forward, Vector3.down));

            sunLight.intensity = _intensity + sunBaseIntensity;
        }

        /// <summary>
        /// Picks a color from sunColor gradient depending on the current value of intensity
        /// </summary>
        private void AdjustSunColor()
        {
            sunLight.color = sunColor.Evaluate(_intensity);
            RenderSettings.fogColor = fogColor.Evaluate(_intensity);
        }
    }
}