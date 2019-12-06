using Remote;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using static Utils.Enums;

namespace Entity.Player
{
    public class SanityController : MonoBehaviour
    {
        public delegate void PlayerStateChanged(int newValue);

        [Tooltip("100: sane, 0: insane")]
        [Range(0, 100)]
        [SerializeField]
        private int sanity;

        [Tooltip("The total severity at which sanity changes. Scales the change rate")]
        [SerializeField]
        private float severity;

        [Tooltip("If one or more stats have a negative impact, weight other stats with this percentage.")]
        [SerializeField]
        private float lowImpactSeverity;

        [Tooltip("Determines how fast player gains sanity. Scales current stat value * stat severity")]
        [SerializeField]
        private float gainFactor;

        [Tooltip("Stats that have an impact on sanity.")]
        [SerializeField]
        private Stat[] stats;

        // I considered putting the sanity in player state, but it's just an unnecessary reference between playerstate and sanitycontroller
        public static event PlayerStateChanged OnPlayerSanityUpdate;
        
        
        // the total change rate
        private float _changeRate;
        // builds up to 1 or -1 for the next tick
        private float _nextTick;
        

        private void OnEnable()
        {
            RemoteStatusHandler.OnPlayerSanityRemoteUpdate += ChangePlayerSanity;

            PlayerState.OnPlayerHealthUpdate += (int health) => InfluenceSanityByStat(StatType.Health, health);
            PlayerState.OnPlayerSaturationUpdate += (int saturation) => InfluenceSanityByStat(StatType.Saturation, saturation);
            PlayerState.OnPlayerHydrationUpdate += (int hydration) => InfluenceSanityByStat(StatType.Hydration, hydration);
        }

        private void OnDisable()
        {
            RemoteStatusHandler.OnPlayerSanityRemoteUpdate -= ChangePlayerSanity;

            PlayerState.OnPlayerHealthUpdate -= (int health) => InfluenceSanityByStat(StatType.Health, health);
            PlayerState.OnPlayerSaturationUpdate -= (int saturation) => InfluenceSanityByStat(StatType.Saturation, saturation);
            PlayerState.OnPlayerHydrationUpdate -= (int hydration) => InfluenceSanityByStat(StatType.Hydration, hydration);
        }

        private void Update()
        {
            TickSanity();
        }

        private void ChangePlayerSanity(int changeBy)
        {
            sanity = Mathf.Clamp(sanity + changeBy, 0, 100);
            OnPlayerSanityUpdate?.Invoke(sanity);
        }

        /// <summary>
        /// Sums up a tick weighting the total change rate with <see cref="severity"/>. 
        /// Only sums up a positive tick if player doesn't have full sanity. Ticks are either
        /// positive or negative 1. If summed up tick, changes player sanity.
        /// </summary>
        private void TickSanity()
        {
            float change = _changeRate * severity;
            _nextTick += change;
            if (sanity == 100 && _nextTick > 0)
            {
                // don't build up a positive tick value when sanity == 100
                _nextTick = 0;
                return;
            }
            if (_nextTick >= 1)
            {
                ChangePlayerSanity(1);
                _nextTick = 0;
            }
            else if (_nextTick <= -1)
            {
                ChangePlayerSanity(-1);
                _nextTick = 0;
            }
        }
        
        /// <summary>
        /// Influences sanity by stat and its current value. It changes the stat's rate. It will get negative
        /// as the current stat value gets below the stat boundary. If it's positive it is weighted with a
        /// gain factor additionally.
        /// </summary>
        /// <param name="statType">Type of stat</param>
        /// <param name="value">Current value of stat</param>
        private void InfluenceSanityByStat(StatType statType, int value)
        {
            Stat stat = GetStatByType(statType);
            if (value < stat.Boundary) stat.Rate = (value - stat.Boundary) * stat.Severity;
            else
                // don't use - stat.Boundary because then if you have 100% of stat,
                // you get a higher positive rate, the lower the boundary
                stat.Rate = value * gainFactor * stat.Severity;

            UpdateChangeRate();
        }

        // je länger changeRate negativ, desto höher/niedriger wird sie?

        /// <summary>
        /// Updates the total change rate which influences sanity. If not all stat rates are positive, weights the positive
        /// ones with a lower severity. 
        /// </summary>
        private void UpdateChangeRate()
        {
            bool allPositive = AllRatesPositive();
            _changeRate = 0;
            foreach (Stat stat in stats)
            {
                // only use negative rates or all if no stat is negative
                if (!allPositive && stat.Rate < 0 || allPositive)
                    _changeRate += stat.Rate;
                else
                    // found a positive rate between negative rates
                    // --> use it, but with lower impact
                    _changeRate += stat.Rate * lowImpactSeverity;
            }
        }

        private void AffectGameplay(int sanity)
        {
            // implement impact on gameplay
        }
        
        /// <summary>
        /// Gets the stat from <see cref="stats"/>
        /// </summary>
        private Stat GetStatByType(StatType statType)
        {
            foreach (Stat stat in stats)
            {
                if (stat.Type == statType)
                    return stat;
            }
            throw new Exception("Stat not found");
        }

        /// <summary>
        /// Checks if all stat rates are positive
        /// </summary>
        private bool AllRatesPositive()
        {
            foreach (Stat stat in stats)
            {
                if (stat.Rate < 0)
                    return false;
            }
            return true;
        }
    }
}
