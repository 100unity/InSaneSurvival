using System.Collections;
using GameTime;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Can be used to change the time of the day with an animation
    /// </summary>
    public class DayNightManager : Singleton<DayNightManager>
    {
        [Tooltip("Component reference for grabbing the current time")] [SerializeField]
        private Clock clock;

        [Tooltip("Used for calculations. Determines the speed of the TimeOfDay change")] [SerializeField]
        private AnimationCurve animationCurve;

        /// <summary>
        /// Flag for preventing multiple animations.
        /// </summary>
        private bool _isAnimating;

        /// <summary>
        /// Sets the <see cref="Clock.TimeOfDay"/> using the clock.
        /// </summary>
        /// <param name="newTimeOfDay">The new time of the day</param>
        public void SetTimeOfDay(float newTimeOfDay) => clock.SetTimeOfDay(newTimeOfDay);

        /// <summary>
        /// Sets the <see cref="Clock.TimeOfDay"/> over a specified time, using the <see cref="animationCurve"/>.
        /// </summary>
        /// <param name="newTimeOfDay">The new time of the day</param>
        /// <param name="animationTime">The animation time</param>
        public void SetDayTimeWithAnimation(float newTimeOfDay, float animationTime)
        {
            if (_isAnimating) return;
            StartCoroutine(DayTimeAnimation(newTimeOfDay, animationTime));
        }

        /// <summary>
        /// The Coroutine for <see cref="SetDayTimeWithAnimation"/>.
        /// </summary>
        private IEnumerator DayTimeAnimation(float newTimeOfDay, float animationTime)
        {
            // Prevents multiple animations
            _isAnimating = true;

            float time = 0;
            float oldTimeOfDay = clock.TimeOfDay;

            // Used for going to an earlier time e.g. from 0.8 to 0.3
            if (newTimeOfDay < oldTimeOfDay)
            {
                newTimeOfDay += 1;
                clock.AddOneDay();
            }

            while (time < animationTime)
            {
                time += Time.deltaTime;
                float t = time * (1 / animationTime);
                SetTimeOfDay(Mathf.Lerp(oldTimeOfDay, newTimeOfDay, animationCurve.Evaluate(t)));
                yield return null;
            }

            _isAnimating = false;
        }
    }
}