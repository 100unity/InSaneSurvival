using System;
using AbstractClasses;
using Buildings;
using Entity.Enemy;
using Managers;
using UnityEngine;

namespace Entity.Player.Sanity
{
    [RequireComponent(typeof(PlayerState))]
    [RequireComponent(typeof(AttackLogic))]
    public class SanityController : MonoBehaviour
    {
        public enum StatType { Health, Saturation, Hydration };
        public delegate void PlayerStateChanged(int newValue);

        [Tooltip("Handles the math and holds stats influencing the player's sanity.")]
        [SerializeField]
        private SanityMath sanityMath;

        [Header("More positive impacts")]

        [SerializeField]
        private float healMultiplier;

        [SerializeField]
        private float healBonusDuration;

        [Tooltip("Stop any negative sanity changes caused by the character's needs (saturation, hydration, health) when in range of the campfire.")]
        [SerializeField]
        private bool applyCampfireBonus;

        [Header("More negative event ticks")]

        [SerializeField]
        private float fightTick;

        [SerializeField]
        private float fightNonAggressivesTick;

        [SerializeField]
        private float attackStopTime;

        [SerializeField]
        private float criticalFightTime;

        [SerializeField]
        private float hitTick;

        [SerializeField]
        private int criticalHitCount;


        // builds up to 1 or -1 for the next tick
        private float _nextTick;
        private bool _pauseTick;
        private PlayerState _playerState;
        private AttackLogic _attackLogic;
        private float _eventTick;
        private bool _isFighting;
        private float _fightTime;
        private float _attackStopTimer;
        private int _hitCounter;
        private bool _healBonus;
        private float _healBonusStopTimer;

        private void Awake()
        {
            _playerState = GetComponent<PlayerState>();
            _attackLogic = GetComponent<AttackLogic>();
        }
        
        private void OnEnable()
        {
            PlayerState.OnPlayerHealthUpdate += (int health) => sanityMath.InfluenceSanityByStat(StatType.Health, health);
            PlayerState.OnPlayerSaturationUpdate += (int saturation) => sanityMath.InfluenceSanityByStat(StatType.Saturation, saturation);
            PlayerState.OnPlayerHydrationUpdate += (int hydration) => sanityMath.InfluenceSanityByStat(StatType.Hydration, hydration);
            AttackLogic.OnPlayerAttackPerformed += OnAttackPerformed;
            PlayerState.OnPlayerHit += OnPlayerHit;
            PlayerState.OnPlayerHealed += OnPlayerHealed;
        }

        private void OnDisable()
        {
            PlayerState.OnPlayerHealthUpdate -= (int health) => sanityMath.InfluenceSanityByStat(StatType.Health, health);
            PlayerState.OnPlayerSaturationUpdate -= (int saturation) => sanityMath.InfluenceSanityByStat(StatType.Saturation, saturation);
            PlayerState.OnPlayerHydrationUpdate -= (int hydration) => sanityMath.InfluenceSanityByStat(StatType.Hydration, hydration);
            AttackLogic.OnPlayerAttackPerformed -= OnAttackPerformed;
            PlayerState.OnPlayerHit -= OnPlayerHit;
            PlayerState.OnPlayerHealed -= OnPlayerHealed;
        }

        /// <summary>
        /// Tick sanity.
        /// </summary>
        private void Update()
        {
            // character needs
            TickSanity();

            // character events
            if (_isFighting)
            {
                _fightTime += Time.deltaTime;
                if (_fightTime >= criticalFightTime)
                {
                    if (/*!!*/_attackLogic.Target.gameObject.GetComponent<EnemyController>().IsAggressive/*!!*/)
                        // please tell me the correct way of doing it, I thought of using the reference 
                        // to AttackLogic that we have here, but it only has Movables which it doesn't 
                        // make sense to give a property IsAggressive... also instanceof stuff is not really in the sense of OOP
                        AddUpEventTick(fightTick);
                    else
                        AddUpEventTick(fightNonAggressivesTick);
                    // start new fight
                    _fightTime = 0;
                }
                MightEndFighting();
            }

            if (_healBonus)
                MightEndHealBonus();

            if (applyCampfireBonus)
                CheckCampfireRange();
        }

        /// <summary>
        /// Sums up a tick weighting the total change rate with <see cref="severity"/>. 
        /// Only sums up a positive tick if player doesn't have full sanity. Ticks are either
        /// positive or negative 1. If summed up tick, changes player sanity.
        /// </summary>
        private void TickSanity()
        {
            _nextTick += sanityMath.GetCurrentChange();
            if (_playerState.Sanity == 100 && _nextTick > 0)
            {
                // don't build up a positive tick value when sanity == 100
                _nextTick = 0;
                return;
            }
            if (_nextTick >= 1)
            {
                _playerState.ChangePlayerSanity(1);
                _nextTick = 0;
            }
            else if (_nextTick <= -1)
            {
                if (!_pauseTick)
                    _playerState.ChangePlayerSanity(-1);
                _nextTick = 0;
            }
        }

        private void OnAttackPerformed()
        {
            if (!_isFighting)
                _isFighting = true;
        }

        private void OnPlayerHit()
        {
            _hitCounter += 1;
            if (_hitCounter >= criticalHitCount)
            {
                AddUpEventTick(hitTick);
                _hitCounter = 0;
            }
        }

        private void AddUpEventTick(float tick)
        {
            _eventTick += tick;
            if (_eventTick >= 1)
            {
                int changeBy = (int) _eventTick;
                _playerState.ChangePlayerSanity(-changeBy);
                _eventTick -= changeBy;
            }
        }

        private void MightEndFighting()
        {
            if (_attackLogic.Target == null)
                _attackStopTimer += Time.deltaTime;
            else
                _attackStopTimer = 0;
            if (_attackStopTimer >= attackStopTime)
            {
                _isFighting = false;
                _fightTime = 0;
                _attackStopTimer = 0;
            }
        }

        private void OnPlayerHealed()
        {
            if (!_healBonus)
            {
                sanityMath.GainFactor = sanityMath.GainFactor * healMultiplier;
                _healBonus = true;
            }
        }

        private void MightEndHealBonus()
        {
            _healBonusStopTimer += Time.deltaTime;
            if (_healBonusStopTimer >= healBonusDuration)
            {
                sanityMath.GainFactor = sanityMath.GainFactor / healMultiplier;
                _healBonus = false;
                _healBonusStopTimer = 0;
            }
        }

        private void CheckCampfireRange()
        {
            _pauseTick = CraftingManager.Instance.HasCraftingStation(CraftingManager.CraftingStation.Fire);
        }
    }
}
