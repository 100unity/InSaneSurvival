using Managers;
using UnityEngine;

namespace Buildings.BuildingScripts
{
    /// <summary>
    /// Sets the spawn point after the build.
    /// </summary>
    public class Tent : Building
    {
        [Header("Tent")] [Tooltip("The new time of day after sleeping")] [SerializeField]
        private float newTime;

        [Tooltip("The animation duration for the animation. Used by animation... Animation")] [SerializeField]
        private float animationTime;

        /// <summary>
        /// Changes the time of the day to the specified one using the <see cref="DayNightManager"/>.
        /// </summary>
        public override void Interact()
        {
            base.Interact();
            DayNightManager.Instance.SetDayTimeWithAnimation(newTime, animationTime);
        }
    }
}