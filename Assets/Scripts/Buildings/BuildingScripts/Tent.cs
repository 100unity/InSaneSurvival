using Constants;
using Entity.Player;
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

        private PlayerController _playerController;

        protected override void Awake()
        {
            base.Awake();
            _playerController = PlayerManager.Instance.GetPlayerController();
        }

        /// <summary>
        /// Changes the time of the day to the specified one using the <see cref="DayNightManager"/>.
        /// </summary>
        protected override void OnInteract()
        {
            _playerController.SetAnimationBool(Consts.Animation.SLEEP_BOOL, true);
            SaveManager.Save("");
            DayNightManager.Instance.SetDayTimeWithAnimation(newTime, animationTime,
                () => _playerController.SetAnimationBool(Consts.Animation.SLEEP_BOOL, false));
        }
    }
}