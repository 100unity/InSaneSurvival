using AbstractClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Entity.Player
{
    [RequireComponent(typeof(PlayerState))]
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(AttackLogic))]
    public class NeedsController : MonoBehaviour
    {
        public delegate void PlayerStateChanged(int newValue);

        [Tooltip("The probability in each frame to add up ticks for saturation and hydration.")]
        [SerializeField]
        private float tickProbability;

        [Tooltip("The tick size that is added up for saturation.")]
        [SerializeField]
        private float saturationTick;

        [Tooltip("The tick size that is added up for hydration.")]
        [SerializeField]
        private float hydrationTick;

        [Tooltip("A number that scales the saturation tick if moving.")]
        [SerializeField]
        private float moveMalusSaturation;

        [Tooltip("A number that scales the hydration tick if moving.")]
        [SerializeField]
        private float moveMalusHydration;

        [Tooltip("A number that scales saturation and hydration tick if moving for a certain time. Is applied on top of the normal move mali.")]
        [SerializeField]
        private float longMoveMalus;

        [Tooltip("The time the character has to be moving without stopping for the long move malus to be applied.")]
        [SerializeField]
        private float criticalMoveTime;

        [Tooltip("A number that scales the saturation tick if fighting. Just applied if not moving (running away).")]
        [SerializeField]
        private float fightMalusSaturation;

        [Tooltip("A number that scales the hydration tick if fighting. Just applied if not moving (running away).")]
        [SerializeField]
        private float fightMalusHydration;


        private PlayerState _playerState;
        private Movable _movable;
        private AttackLogic _attackLogic;

        private Probability _probability;
        private float _nextSaturationTick;
        private float _nextHydrationTick;
        private float _moveTimer;

        private void Awake()
        {
            _playerState = GetComponent<PlayerState>();
            _movable = GetComponent<Movable>();
            _attackLogic = GetComponent<AttackLogic>();
            _probability = new Probability();
        }

        private void Update()
        {
            Tick();
            if (_movable.IsMoving())
                _moveTimer += Time.deltaTime;
            else
                _moveTimer = 0;
        }

        /// <summary>
        /// With a certain probability, increases saturation and hydration tick. 
        /// If the tick is big enough, apply it to corresponding character need.
        /// </summary>
        private void Tick()
        {
            if (_probability.GetProbability(tickProbability))
            {
                ScaleTick();
                if (_nextSaturationTick >= 1)
                {
                    _playerState.ChangePlayerSaturation(GetNegativeChange(ref _nextSaturationTick));
                }

                if (_nextHydrationTick >= 1)
                {
                    _playerState.ChangePlayerSaturation(GetNegativeChange(ref _nextHydrationTick));
                }
            }
        }

        /// <summary>
        /// Make a negative tick which is an int.
        /// </summary>
        /// <param name="nextTick">The tick float that should be used.</param>
        /// <returns>The change to be applied to the character need. Is probably 1.</returns>
        private int GetNegativeChange(ref float nextTick)
        {
            int changeBy = (int)nextTick;
            nextTick -= changeBy;
            return -changeBy;
        }

        /// <summary>
        /// Scales the tick depending on whether the player is moving or fighting.
        /// When the player is moving, fight malus is not applied.
        /// </summary>
        private void ScaleTick()
        {
            float scaledSaturationTick = saturationTick;
            float scaledHydrationTick = hydrationTick;
            if (_movable.IsMoving())
            {
                scaledSaturationTick *= moveMalusSaturation;
                scaledHydrationTick *= moveMalusHydration;

                if (_moveTimer >= criticalMoveTime)
                {
                    scaledSaturationTick *= longMoveMalus;
                    scaledHydrationTick *= longMoveMalus;
                }
            }
            // don't apply fight malus, when running away (in a fight)
            // just apply if really fighting (mind that IsFighting is true a certain time after having killed an enemy)
            else if (_attackLogic.IsFighting)
            {
                scaledSaturationTick *= fightMalusSaturation;
                scaledHydrationTick *= fightMalusHydration;
            }
            Debug.Log(scaledSaturationTick);
            _nextSaturationTick += scaledSaturationTick;
            _nextHydrationTick += scaledHydrationTick;
        }
    }
}

