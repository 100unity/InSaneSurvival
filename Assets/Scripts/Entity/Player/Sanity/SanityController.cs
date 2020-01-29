using AbstractClasses;
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

        [Tooltip("The factor that scales negative sanity ticks and applies them on health if sanity == 0.")]
        [SerializeField]
        private float insaneDamage;

        [Tooltip("Handles the math and holds stats influencing the player's sanity.")] [SerializeField]
        private SanityMath sanityMath;

        [Header("More positive impacts")]
        [Tooltip("Scales the gain factor of positive ticks built up by sufficient health, saturation and hydration.")]
        [SerializeField]
        private float healMultiplier;

        [Tooltip("The duration of the heal bonus in seconds.")] [SerializeField]
        private float healBonusDuration;

        [Tooltip(
            "Stop any negative sanity changes caused by insufficient health, saturation, hydration when in range of a campfire.")]
        [SerializeField]
        private bool applyCampfireBonus;


        [Header("More negative event ticks")]
        [Tooltip("The time in seconds you have to be in combat to get a fight malus.")]
        [SerializeField]
        private float criticalFightTime;

        [Tooltip("The malus for being in a fight for a certain time.")] [SerializeField]
        private float fightMalus;

        [Tooltip("The malus for being in a fight with friendly NPCs for a certain time.")] [SerializeField]
        private float fightNonAggressivesMalus;

        [Tooltip("The number of times to be hit to get a hit malus.")] [SerializeField]
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

        // the time the player has been in combat up until now
        private float _fightTime;
        // (if you attack a hostile NPC but change targets to a friendly NPC just before criticalFightTime 
        // is reached, you'll get the fightNonAgressivesTick)


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
            OnHealthUpdated(100);
            OnSaturationUpdated(100);
            OnHydrationUpdated(100);
        }

        private void OnEnable()
        {
            PlayerState.OnPlayerHealthUpdate += OnHealthUpdated;
            PlayerState.OnPlayerSaturationUpdate += OnSaturationUpdated;
            PlayerState.OnPlayerHydrationUpdate += OnHydrationUpdated;
            AttackLogic.OnAttackPerformed += CheckAttackPerformed;
            Damageable.OnPlayerHit += OnPlayerHit;
            PlayerState.OnPlayerHealed += OnPlayerHealed;
        }

        private void OnDisable()
        {
            PlayerState.OnPlayerHealthUpdate -= OnHealthUpdated;
            PlayerState.OnPlayerSaturationUpdate -= OnSaturationUpdated;
            PlayerState.OnPlayerHydrationUpdate -= OnHydrationUpdated;
            AttackLogic.OnAttackPerformed -= CheckAttackPerformed;
            Damageable.OnPlayerHit -= OnPlayerHit;
            PlayerState.OnPlayerHealed -= OnPlayerHealed;
        }

        /// <summary>
        /// Tick sanity. Influenced by character needs (health, saturation, hydration)
        /// and character events (like hit, fight, heal, etc.).
        /// </summary>
        private void Update()
        {
            // character needs
            SumUpNeedsTick();

            // character events
            if (_attackLogic.IsFighting)
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
            }
            else
            {
                _fightTime = 0;
                // reset hit counter as well
                _hitCounter = 0;
            }

            if (_healBonus)
                MightEndHealBonus();

            if (applyCampfireBonus)
                CheckCampfireRange();
        }

        private void OnHealthUpdated(int health) => sanityMath.InfluenceSanityByStat(StatType.Health, health);

        private void OnSaturationUpdated(int saturation) =>
            sanityMath.InfluenceSanityByStat(StatType.Saturation, saturation);

        private void OnHydrationUpdated(int hydration) =>
            sanityMath.InfluenceSanityByStat(StatType.Hydration, hydration);

        /// <summary>
        /// Depending on the player's needs (health, saturation, hydration), sum up a tick.
        /// Only sum up a positive tick if player doesn't have full sanity. Ticks are either
        /// positive or negative 1. If summed up tick, change player sanity.
        /// </summary>
        private void SumUpNeedsTick()
        {
            _needsTick += sanityMath.GetCurrentChange() * Time.deltaTime;
            if (_playerState.Sanity == 100 && _needsTick > 0)
            {
                // don't build up a positive tick value when sanity == 100
                _needsTick = 0;
                return;
            }

            if (_needsTick >= 1)
            {
                Tick(1);
                _needsTick = 0;
            }
            else if (_needsTick <= -1)
            {
                // do negative tick if not paused and decrease health if sanity == 0 no matter of pauseTick
                if (!_pauseTick || _playerState.Sanity == 0)
                    Tick(-1);
                _needsTick = 0;
            }
        }

        /// <summary>
        /// Tick sanity. If sanity == 0, decrease health.
        /// </summary>
        /// <param name="changeBy"></param>
        private void Tick(int changeBy)
        {
            // always apply changes if sanity > 0 or if positive change
            if (changeBy > 0 || _playerState.Sanity > 0)
            {
                _playerState.ChangePlayerSanity(changeBy);
                return;
            }

            // affect health if sanity == 0
            if (_playerState.Sanity == 0)
                _playerState.ChangePlayerHealth((int) (changeBy * insaneDamage));
        }

        /// <summary>
        /// Check if this entity performed the attack.
        /// </summary>
        /// <param name="attacker">The attacker that performed the attack</param>
        private void CheckAttackPerformed(AttackLogic attacker)
        {
            if (attacker.gameObject == gameObject)
                OnAttackPerformed();
        }

        /// <summary>
        /// Remember last attacked enemy.
        /// </summary>
        private void OnAttackPerformed()
        {
            if (_attackLogic.LastAttacked != null)
                _enemy = _attackLogic.LastAttacked;
        }

        /// <summary>
        /// Count up the hits performed on the player. If <see cref="criticalHitCount"/> is reached,
        /// apply a hit malus.
        /// </summary>
        /// <param name="attacker">The EnemyController of the attacker</param>
        private void OnPlayerHit(EnemyController attacker)
        {
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
                Tick(-changeBy);
                _eventTick -= changeBy;
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
                sanityMath.GainFactor *= healMultiplier;
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