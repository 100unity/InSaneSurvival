using System;
using AbstractClasses;
using Remote;
using UnityEngine;

namespace Entity.Player
{
    
    public class PlayerState : Damageable
    {
        public delegate void PlayerStateChanged(int newValue);
        
        //Player State values
        [Tooltip("100 - full health, 0 - dead")] [SerializeField] [Range(0, 100)] 
        private int health;

        [Tooltip("100: no hunger, 0: the player is in desperate need for food")] [SerializeField] [Range(0, 100)] 
        private int saturation;

        [Tooltip("100: not thirsty, 0: gazing for a sip of water")] [SerializeField] [Range(0, 100)] 
        private int hydration;

        public static event PlayerStateChanged OnPlayerHealthUpdate;
        public static event PlayerStateChanged OnPlayerSaturationUpdate;
        public static event PlayerStateChanged OnPlayerHydrationUpdate;

        protected override void Awake()
        {
            base.Awake();
            OnPlayerHealthUpdate?.Invoke(health);
            OnPlayerSaturationUpdate?.Invoke(saturation);
            OnPlayerHydrationUpdate?.Invoke(hydration);
        }


        private void OnEnable()
        {
            RemoteStatusHandler.OnPlayerHealthRemoteUpdate += ChangePlayerHealth;
            RemoteStatusHandler.OnPlayerHydrationRemoteUpdate += ChangePlayerHydration;
            RemoteStatusHandler.OnPlayerSaturationRemoteUpdate += ChangePlayerSaturation;
        }

        private void OnDisable()
        {
            RemoteStatusHandler.OnPlayerHealthRemoteUpdate -= ChangePlayerHealth;
            RemoteStatusHandler.OnPlayerHydrationRemoteUpdate -= ChangePlayerHydration;
            RemoteStatusHandler.OnPlayerSaturationRemoteUpdate -= ChangePlayerSaturation;
        }

        private void ChangePlayerHealth(int changeBy)
        {
            int updatedValue = health + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                Die();
            }

            health = updatedValue;
            //throws an event with the new health value as a parameter
            OnPlayerHealthUpdate?.Invoke(updatedValue);
        }

        public void ChangePlayerSaturation(int changeBy)
        {
            int updatedValue = saturation + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                ChangePlayerHealth(-1);
            }

            saturation = updatedValue;
            OnPlayerSaturationUpdate?.Invoke(updatedValue);
        }

        public void ChangePlayerHydration(int changeBy)
        {
            int updatedValue = hydration + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                ChangePlayerHealth(-1);
            }

            hydration = updatedValue;
            OnPlayerHydrationUpdate?.Invoke(updatedValue);
        }

        /// <summary>
        /// Does damage to the player.
        /// </summary>
        /// <param name="damage">The damage dealt to player</param>
        public override void Hit(int damage)
        {
            base.Hit(damage);
            ChangePlayerHealth(-damage);
        }

        public override void Die()
        {
            Debug.Log("Player is dead");
        }
    }
}