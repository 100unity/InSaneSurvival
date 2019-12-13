using Remote;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Entity.Player.Sanity
{
    [RequireComponent(typeof(PlayerState))]
    public class SanityController : MonoBehaviour
    {
        public enum StatType { Health, Saturation, Hydration };
        public delegate void PlayerStateChanged(int newValue);

        [Tooltip("Handles the math and holds stats influencing the player's sanity.")]
        [SerializeField]
        private SanityMath sanityMath;
                
        // builds up to 1 or -1 for the next tick
        private float _nextTick;
        private PlayerState _playerState;

        private void Awake()
        {
            _playerState = GetComponent<PlayerState>();
        }
        
        private void OnEnable()
        {
            PlayerState.OnPlayerHealthUpdate += (int health) => sanityMath.InfluenceSanityByStat(StatType.Health, health);
            PlayerState.OnPlayerSaturationUpdate += (int saturation) => sanityMath.InfluenceSanityByStat(StatType.Saturation, saturation);
            PlayerState.OnPlayerHydrationUpdate += (int hydration) => sanityMath.InfluenceSanityByStat(StatType.Hydration, hydration);
        }

        private void OnDisable()
        {
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
                _playerState.ChangePlayerSanity(-1);
                _nextTick = 0;
            }
        }
    }
}
