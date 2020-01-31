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

        [Tooltip("The amount that the player gets healed")] [SerializeField]
        private int healValue;

        [Tooltip("The amount that the player gets sanity back")] [SerializeField]
        private int sanityValue;

        [Tooltip("The amount that the player looses hydration")] [SerializeField]
        private int hydrationValue;

        [Tooltip("The amount that the player looses saturation")] [SerializeField]
        private int saturationValue;

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
            _playerController.DeactivateMovement();
            InteractDisabled = true;
            _playerController.SetAnimationBool(Consts.Animation.SLEEP_BOOL, true);
            DayNightManager.Instance.SetDayTimeWithAnimation(newTime, animationTime,
                () =>
                {
                    _playerController.SetAnimationBool(Consts.Animation.SLEEP_BOOL, false);
                    _playerController.ActivateMovement();
                    InteractDisabled = false;
                    PlayerState playerState = PlayerManager.Instance.GetPlayerState();
                    playerState.ChangePlayerHealth(healValue);
                    playerState.ChangePlayerSanity(sanityValue);
                    playerState.ChangePlayerHydration(hydrationValue);
                    playerState.ChangePlayerSaturation(saturationValue);
                    SaveManager.Save();
                });
        }
    }
}