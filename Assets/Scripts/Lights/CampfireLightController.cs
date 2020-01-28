using GameTime;
using UnityEngine;
using Utils;

namespace Lights
{
    public class CampfireLightController : MonoBehaviour
    {
        [Tooltip("The range of the flare frequency.")]
        [SerializeField]
        private Range flareFrequencyScale;

        [Tooltip("The range of the light range.")]
        [SerializeField]
        private Range rangeRange;

        [Tooltip("The range of the light intensity.")]
        [SerializeField]
        private Range intensityRange;


        private Light _light;
        private System.Random _random;
        private float[] _timer;
        private float _nextRange;
        private float _nextIntensity;
        private float _oldRange;
        private float _oldIntensity;
        private float[] _flareFrequency;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _random = new System.Random();
            _timer = new float[2];
            _flareFrequency = new float[2];
            _nextRange = rangeRange.min;
            _nextIntensity = intensityRange.min;
            Next(0, rangeRange.min);
            Next(1, intensityRange.min);
        }

        /// <summary>
        /// Flare the light.
        /// </summary>
        private void Update()
        {
            if (_light.range == rangeRange.max || _light.range == rangeRange.min)
                Next(0, _light.range);

            if (_light.intensity == intensityRange.max || _light.intensity == intensityRange.min)
                Next(1, _light.intensity);

            _timer[0] += Time.deltaTime;
            _timer[1] += Time.deltaTime;

            _light.range = Mathf.Lerp(_oldRange, _nextRange, _timer[0] * _flareFrequency[0]);
            _light.intensity = Mathf.Lerp(_oldIntensity, _nextIntensity, _timer[1] * _flareFrequency[1]);
        }

        /// <summary>
        /// Set next value to be approached by a property to min if max reached or to max if min reached,
        /// reset timer, remember current value, set new random flare frequency.
        /// </summary>
        /// <param name="index">Whether to do it for range or intensity</param>
        /// <param name="currentValue">The value of the property to change the next value to be approached for.</param>
        private void Next(int index, float currentValue)
        {
            _timer[index] = 0;
            if (index == 0)
                _oldRange = _light.range;
            else
                _oldIntensity = _light.intensity;
            _flareFrequency[index] = GetRandomFloat(flareFrequencyScale.min, flareFrequencyScale.max);

            if (index == 0)
            {
                if (currentValue == rangeRange.min)
                    _nextRange = rangeRange.max;
                else
                    _nextRange = rangeRange.min;
            }
            
            if (index == 1)
            {
                if (currentValue == intensityRange.min)
                    _nextIntensity = intensityRange.max;
                else
                    _nextIntensity = intensityRange.min;
            }
        }

        private float GetRandomFloat(float minimum, float maximum)
        {
            return (float) (_random.NextDouble() * (maximum - minimum) + minimum);
        }
    }
}

