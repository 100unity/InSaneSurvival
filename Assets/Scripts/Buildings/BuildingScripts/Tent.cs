using Managers;
using UnityEngine;

namespace Buildings.BuildingScripts
{
    /// <summary>
    /// Sets the spawn point after the build.
    /// </summary>
    public class Tent : Building
    {
        [Header("Tent")] [Tooltip("The spawn point of the player, in case he dies")] [SerializeField]
        private GameObject spawnPoint;

        [Tooltip("The new time of day after sleeping")] [SerializeField]
        private float newTime;

        [Tooltip("The animation duration for the animation. Used by animation... Animation")] [SerializeField]
        private float animationTime;

        /// <summary>
        /// Sets the spawn point after the player finished building it.
        /// </summary>
        protected override void OnBuild() => PlayerManager.Instance.SetSpawnPoint(spawnPoint.transform);

        /// <summary>
        /// Changes the time of the day to the specified one using the <see cref="DayNightManager"/>.
        /// </summary>
        public override void Interact()
        {
            if (!PlayerInReach) return;
            DayNightManager.Instance.SetDayTimeWithAnimation(newTime, animationTime);
        }
    }
}