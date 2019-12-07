using Remote;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using static Utils.Enums;

namespace Entity.Player.Sanity
{
    public class SanityController : MonoBehaviour
    {
        public delegate void PlayerStateChanged(int newValue);

        [Tooltip("100: sane, 0: insane")]
        [Range(0, 100)]
        [SerializeField]
        private int sanity;

        [Tooltip("Handles the math and holds stats influencing the player's sanity.")]
        [SerializeField]
        private SanityMath sanityMath;
        
        public static event PlayerStateChanged OnPlayerSanityUpdate;
        
        // builds up to 1 or -1 for the next tick
        private float _nextTick;
        

        private void OnEnable()
        {
            RemoteStatusHandler.OnPlayerSanityRemoteUpdate += ChangePlayerSanity;

            PlayerState.OnPlayerHealthUpdate += (int health) => sanityMath.InfluenceSanityByStat(StatType.Health, health);
            PlayerState.OnPlayerSaturationUpdate += (int saturation) => sanityMath.InfluenceSanityByStat(StatType.Saturation, saturation);
            PlayerState.OnPlayerHydrationUpdate += (int hydration) => sanityMath.InfluenceSanityByStat(StatType.Hydration, hydration);
        }

        private void OnDisable()
        {
            RemoteStatusHandler.OnPlayerSanityRemoteUpdate -= ChangePlayerSanity;

            PlayerState.OnPlayerHealthUpdate -= (int health) => sanityMath.InfluenceSanityByStat(StatType.Health, health);
            PlayerState.OnPlayerSaturationUpdate -= (int saturation) => sanityMath.InfluenceSanityByStat(StatType.Saturation, saturation);
            PlayerState.OnPlayerHydrationUpdate -= (int hydration) => sanityMath.InfluenceSanityByStat(StatType.Hydration, hydration);
        }

        /// <summary>
        /// Tick sanity.
        /// </summary>
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
            _nextTick += sanityMath.GetCurrentChange();
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
        
        
    }
}
