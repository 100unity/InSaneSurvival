using System;
using System.Linq;
using UnityEngine;
using Utils;
using static Utils.Enums;

namespace Entity.Player.Sanity
{
    /// <summary>
    /// Handles the math for stats influencing the player's sanity.
    /// </summary>
    [Serializable]
    public class SanityMath
    {
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


        // the total change rate
        private float _changeRate;

        /// <summary>
        /// Gets the current sanity change which should be added to the next tick.
        /// </summary>
        public float GetCurrentChange()
        {
            return _changeRate * severity;
        }

        /// <summary>
        /// Influences sanity by stat and its current value. It changes the stat's rate. It will get negative
        /// as the current stat value gets below the stat boundary. If it's positive it is weighted with a
        /// gain factor additionally.
        /// </summary>
        /// <param name="statType">Type of stat</param>
        /// <param name="value">Current value of stat</param>
        public void InfluenceSanityByStat(StatType statType, int value)
        {
            Stat stat = GetStatByType(statType);
            if (value < stat.Boundary)
                stat.Rate = (value - stat.Boundary) * stat.Severity;
            else
                // don't use - stat.Boundary because then if you have 100% of stat,
                // you get a higher positive rate, the lower the boundary
                stat.Rate = value * gainFactor * stat.Severity;

            UpdateChangeRate();
        }

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
                if (allPositive || stat.Rate < 0)
                    _changeRate += stat.Rate;
                else
                    // found a positive rate between negative rates
                    // --> use it, but with lower impact
                    _changeRate += stat.Rate * lowImpactSeverity;
            }
        }

        /// <summary>
        /// Gets the stat from <see cref="stats"/>
        /// </summary>
        private Stat GetStatByType(StatType statType)
        {
            return stats.Where(s => s.Type == statType).FirstOrDefault();
        }

        /// <summary>
        /// Checks if all stat rates are positive
        /// </summary>
        private bool AllRatesPositive()
        {
            return stats.All(s => s.Rate >= 0);
        }
    }
}
