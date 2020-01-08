using System;
using Managers;
using UnityEngine;

namespace GameTime
{
    public class Clock : MonoBehaviour
    {
        [SerializeField][Tooltip("Day length in minutes")]
        private float dayLength;
    
        [SerializeField][Tooltip("Year length in days")]
        private int yearLength;

        [SerializeField] [Tooltip("Current time")] [Range(0f, 1f)]
        private float timeOfDay;
        public float TimeOfDay => timeOfDay;

        private int _days;
        public int Days => _days;

        private int _years;
        public int Years => _years;

        private float _timeScale;
        
        [SerializeField][Tooltip("Controls timeScale so certain time periods (night) can pass faster than others (day)")]
        private AnimationCurve timeCurve;

        private float _timeCurveNormalization;

        private void Awake()
        {
            NormalizeTimeCurve();
        }

        private void Update()
        {
            UpdateTimeScale();
            UpdateTime();
        }

        /// <summary>
        /// Updates the current timeScale; Varies depending on the current time of day (=> timeCurve)
        /// </summary>
        private void UpdateTimeScale()
        {
            _timeScale = 24 / (dayLength / 60); // calculating basic tick rate
            _timeScale *= timeCurve.Evaluate(timeOfDay);
            _timeScale /= _timeCurveNormalization;
        }

        /// <summary>
        /// Updates the current time based on FPS and timeScale
        /// </summary>
        private void UpdateTime()
        {
            timeOfDay += Time.deltaTime * _timeScale / 86400;
            if (timeOfDay >= 1)
            {
                timeOfDay = 0;
                _days++;
                if (_days >= yearLength)
                {
                    _days = 0;
                    _years++;
                }
            }
        }

        /// <summary>
        /// Calculates the mean of timeCurve
        /// </summary>
        private void NormalizeTimeCurve()
        {
            float stepSize = 0.01f;
            int numberOfSteps = Mathf.FloorToInt(1f / stepSize);
            float curveTotal = 0;

            for (int i = 0; i < numberOfSteps; i++)
            {
                curveTotal += timeCurve.Evaluate(i * stepSize);
            }

            _timeCurveNormalization = curveTotal / numberOfSteps;
        }
    }
}