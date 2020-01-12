using System;
using Remote;
using UnityEngine;

namespace GameTime
{
    public class Clock : MonoBehaviour
    {
        public delegate void DayTimeEvent();

        [SerializeField] [Tooltip("Day length in minutes")]
        private float dayLength;

        [SerializeField] [Tooltip("Year length in days")]
        private int yearLength;

        [SerializeField] [Tooltip("Current time")] [Range(0f, 1f)]
        private float timeOfDay;

        public float TimeOfDay => timeOfDay;
        
        private bool _isNight;
        public bool IsNight =>_isNight;
        
        private bool _moonShines;

        private int _days;
        public int Days => _days;

        private int _years;
        public int Years => _years;

        private float _timeScale;

        private float _baseTickRate;

        [SerializeField]
        [Tooltip("Controls timeScale so certain time periods (night) can pass faster than others (day)")]
        private AnimationCurve timeCurve;

        private float _timeCurveNormalization;
        
        public static event DayTimeEvent SunRise;
        public static event DayTimeEvent SunSet;
        public static event DayTimeEvent MoonRise;
        public static event DayTimeEvent MoonSet;

        private void Awake()
        {
            SetDayNightTriggers();
            NormalizeTimeCurve();
            _baseTickRate = 24 / (dayLength / 60); // calculating basic tick rate
        }
        
        private void Update()
        {
            UpdateTimeScale();
            UpdateTime();
            InvokeDayTimeEvent();
        }
        
        /// <summary>
        /// Sets the daynumber - used for loading savegames
        /// </summary>
        /// <param name="day"></param>
        public void SetDayNumber(int day) => this._days = day;

        /// <summary>
        /// Sets the time of the day instantly.
        /// </summary>
        /// <param name="newTimeOfDay">The new time of the day (0 to 1)</param>
        public void SetTimeOfDay(float newTimeOfDay) => timeOfDay = newTimeOfDay;

        /// <summary>
        /// Manually adds a day. Should be used if the time is set manually.
        /// </summary>
        public void AddOneDay() => _days++;

        /// <summary>
        /// Updates the current timeScale; Varies depending on the current time of day (=> timeCurve)
        /// </summary>
        private void UpdateTimeScale()
        {
            _timeScale = _baseTickRate;
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
        /// Triggers Events for Sunrise and Sunset
        /// </summary>
        private void InvokeDayTimeEvent()
        {
            if (_isNight && timeOfDay >= 0.25 && timeOfDay < 0.70)
            {
                _isNight = !_isNight;
                SunRise?.Invoke();
            }
            
            if (!_isNight && timeOfDay >= 0.70)
            {
                _isNight = !_isNight;
                SunSet?.Invoke();
            }

            if (!_moonShines && timeOfDay >= 0.85)
            {
                _moonShines = !_moonShines;
                MoonRise?.Invoke();
            }

            if (_moonShines && TimeOfDay > 0.15 && timeOfDay < 0.85)
            {
                _moonShines = !_moonShines;
                MoonSet?.Invoke();
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
        
        /// <summary>
        /// Checks if it is day or night and sets "_isNight" accordingly
        /// </summary>
        private void SetDayNightTriggers()
        {
            _isNight = !(TimeOfDay >= 0.25 && TimeOfDay <= 0.70);
            _moonShines = !(TimeOfDay >= 0.15 && TimeOfDay <= 0.85);
        }
    }
}