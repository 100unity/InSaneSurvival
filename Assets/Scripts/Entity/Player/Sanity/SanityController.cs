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
        [Tooltip("Scales the gain factor of positive ticks built up by sufficient health, saturation and hydration.")]
        [SerializeField]
        private float healMultiplier;

        [Tooltip("The duration of the heal bonus in seconds.")]
        [SerializeField]
        private float healBonusDuration;

        [Tooltip("Stop any negative sanity changes caused by insufficient health, saturation, hydration when in range of a campfire.")]
        [SerializeField]
        private bool applyCampfireBonus;


        [Header("More negative event ticks")]
        [Tooltip("The time in seconds you have to be in combat to get a fight malus.")]
        [SerializeField]
        private float criticalFightTime;

        [Tooltip("The malus for being in a fight for a certain time.")]
        [SerializeField]
        private float fightMalus;

        [Tooltip("The malus for being in a fight with friendly NPCs for a certain time.")]
        [SerializeField]
        private float fightNonAggressivesMalus;

        [Tooltip("The time you have to be out of combat (not attacking, not being hit) to be considered out of combat.")]
        [SerializeField]
        private float fightStopTime;

        [Tooltip("The number of times to be hit to get a hit malus.")]
        [SerializeField]
        private int criticalHitCount;

        [Tooltip("The malus for being hit a certain number of times. (Kind of a \"long fight\" malus.)")]
        [SerializeField]
        private float hitMalus;


        private PlayerState _playerState;
        // for getting the target
        private AttackLogic _attackLogic;

        // depending on the player's needs build up to 1 or -1 for the next tick
        private float _needsTick;
        // for pausing any negative ticks caused by the player's needs
        private bool _pauseTick;
        // depending on events on the player build up a negative tick
        private float _eventTick;
        // whether the player is considered to be in combat
        private bool _isFighting;
        // the time the player has been in combat up until now
        private float _fightTime; 
        // (if you attack a hostile NPC but change targets to a friendly NPC just before criticalFightTime 
        // is reached, you'll get the fightNonAgressivesTick)

        // the time the player has stopped fighting while being in combat
        private float _attackStopTimer;
        // the number of hits performed on the player
        private int _hitCounter;
        // whether heal bonus is currently active
        private bool _healBonus;
        // a timer counting up to stop deactivate the heal bonus after a certain time
        private float _healBonusStopTimer;

        // the enemy the player is currently in combat with (fighting/being hit)
        private EnemyController _enemy;

        private void Awake()
        {
            _playerState = GetComponent<PlayerState>();
            _attackLogic = GetComponent<AttackLogic>();
        }
        
        private void OnEnable()
        {
            PlayerState.OnPlayerHealthUpdate += OnHealthUpdated;
            PlayerState.OnPlayerSaturationUpdate += OnSaturationUpdated;
            PlayerState.OnPlayerHydrationUpdate += OnHydrationUpdated;
            AttackLogic.OnPlayerAttackPerformed += OnAttackPerformed;
            PlayerState.OnPlayerHit += OnPlayerHit;
            PlayerState.OnPlayerHealed += OnPlayerHealed;
        }

        private void OnDisable()
        {
            PlayerState.OnPlayerHealthUpdate -= OnHealthUpdated;
            PlayerState.OnPlayerSaturationUpdate -= OnSaturationUpdated;
            PlayerState.OnPlayerHydrationUpdate -= OnHydrationUpdated;
            AttackLogic.OnPlayerAttackPerformed -= OnAttackPerformed;
            PlayerState.OnPlayerHit -= OnPlayerHit;
            PlayerState.OnPlayerHealed -= OnPlayerHealed;
        }

        /// <summary>
        /// Tick sanity. Influenced by character needs (health, saturation, hydration)
        /// and character events (like hit, fight, heal, etc.).
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
                    // check if last attacked target was aggressive
                    if (_enemy.IsAggressive)
                        AddUpEventTick(fightMalus);
                    else
                        AddUpEventTick(fightNonAggressivesMalus);
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

        private void OnHealthUpdated(int health) => sanityMath.InfluenceSanityByStat(StatType.Health, health);
        private void OnSaturationUpdated(int saturation) => sanityMath.InfluenceSanityByStat(StatType.Saturation, saturation);
        private void OnHydrationUpdated(int hydration) => sanityMath.InfluenceSanityByStat(StatType.Hydration, hydration);

        /// <summary>
        /// Depending on the player's needs (health, saturation, hydration), sum up a tick 
        /// weighting the total change rate with <see cref="severity"/>. 
        /// Only sum up a positive tick if player doesn't have full sanity. Ticks are either
        /// positive or negative 1. If summed up tick, change player sanity.
        /// </summary>
        private void TickSanity()
        {
            _needsTick += sanityMath.GetCurrentChange();
            if (_playerState.Sanity == 100 && _needsTick > 0)
            {
                // don't build up a positive tick value when sanity == 100
                _needsTick = 0;
                return;
            }
            if (_needsTick >= 1)
            {
                _playerState.ChangePlayerSanity(1);
                _needsTick = 0;
            }
            else if (_needsTick <= -1)
            {
                if (!_pauseTick)
                    _playerState.ChangePlayerSanity(-1);
                _needsTick = 0;
            }
        }

        /// <summary>
        /// Set player's status to fighting as soon as he performs an attack.
        /// </summary>
        private void OnAttackPerformed()
        {
            if (!_isFighting)
                _isFighting = true;
            _enemy = _attackLogic.lastAttacked;
        }

        /// <summary>
        /// Count up the hits performed on the player. If <see cref="criticalHitCount"/> is reached,
        /// apply a hit malus.
        /// Also make the player stay in / enter combat if he is being hit.
        /// </summary>
        /// <param name="attacker">The EnemyController of the attacker</param>
        private void OnPlayerHit(EnemyController attacker)
        {
            // enter combat
            if (!_isFighting)
                _isFighting = true;

			// get the EnemyController passed here, so that e.g. if you attack a non-aggressive,
            // run away and then get hit by an aggressive (which you don't fight back/runaway), you don't get the non-aggressive malus
            // also necessary to prevent nullref if not having attacked at all, but get hit
            _enemy = attacker;

            _hitCounter += 1;
            if (_hitCounter >= criticalHitCount)
            {
                AddUpEventTick(hitMalus);
                _hitCounter = 0;
            }
            // also reset the attack stop timer due to still being in combat
            _attackStopTimer = 0;
        }

        /// <summary>
        /// Add up a tick that, if greater than 1, is subtracted from the sanity.
        /// </summary>
        /// <param name="tick">The tick to be applied negatively.</param>
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

        /// <summary>
        /// Set the player's status to not fighting if he has been out of
        /// combat (not hit, not attacking) for a certain time.
        /// Also reset the hit counter if player gets out of combat.
        /// </summary>
        private void MightEndFighting()
        {
            if (_attackLogic.Target == null)
                _attackStopTimer += Time.deltaTime;
            else
                _attackStopTimer = 0;
            if (_attackStopTimer >= fightStopTime)
            {
                _isFighting = false;
                _fightTime = 0;
                _attackStopTimer = 0;
                // reset hit counter as well
                _hitCounter = 0;
            }
        }

        /// <summary>
        /// If not already in heal state, a bonus to sanity 
        /// regeneration is applied after the player has restored health.
        /// </summary>
        private void OnPlayerHealed()
        {
            if (!_healBonus)
            {
                sanityMath.GainFactor = sanityMath.GainFactor * healMultiplier;
                _healBonus = true;
            }
        }

        /// <summary>
        /// End the heal bonus after <see cref="healBonusDuration"/> past.
        /// </summary>
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

        /// <summary>
        /// Pause any mali that are applied because of insufficient character 
        /// needs (health, saturation, hydration) when being in range of a campfire.
        /// </summary>
        private void CheckCampfireRange()
        {
            _pauseTick = CraftingManager.Instance.HasCraftingStation(CraftingManager.CraftingStation.Fire);
        }
    }
}
