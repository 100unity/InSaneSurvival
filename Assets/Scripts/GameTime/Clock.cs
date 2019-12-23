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

        public int YearLength
        {
            get { return yearLength; }
        }

        [SerializeField] [Tooltip("Current time")] [Range(0f, 1f)]
        private float timeOfDay;
        public float TimeOfDay
        {
            get { return timeOfDay; }
        }

        private int _days;
        public int Days
        {
            get { return _days; }
        }
        
        private int _years;

        public int Years
        {
            get { return _years; }
        }
        private float _timeScale;
        
        [SerializeField]
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

        private void UpdateTimeScale()
        {
            _timeScale = 24 / (dayLength / 60);
            _timeScale *= timeCurve.Evaluate(timeOfDay);
            _timeScale /= _timeCurveNormalization;
        }

        private void UpdateTime()
        {
            timeOfDay += Time.deltaTime * _timeScale / 86400; // current day time in seconds
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