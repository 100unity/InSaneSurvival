using System;
using AbstractClasses;
using Interfaces;
using Remote;
using UnityEngine;

namespace Entity.Player
{
    
    public class PlayerState : Damageable
    {
        public delegate void PlayerStateChanged(int newValue);
        public delegate void PlayerIsDead();
        
        //Player State values
        [Tooltip("100 - full health, 0 - dead")] [SerializeField] [Range(0, 100)] 
        private int health;

        [Tooltip("100: no hunger, 0: the player is in desperate need for food")] [SerializeField] [Range(0, 100)] 
        private int saturation;

        [Tooltip("100: not thirsty, 0: gazing for a sip of water")] [SerializeField] [Range(0, 100)] 
        private int hydration;

        public static event PlayerStateChanged OnPlayerHealthUpdated;
        public static event PlayerStateChanged OnPlayerSaturationUpdated;
        public static event PlayerStateChanged OnPlayerHydrationUpdated;
        public static event PlayerIsDead OnPlayerDeath;
        

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

        public int GetHealth() => health;
        public int GetSaturation() => saturation;
        public int GetHydration() => hydration;

        private void ChangePlayerHealth(int changeBy)
        {
            int updatedValue = health + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue <= 0)
            {
                updatedValue = 0;
                Die();
            }

            health = updatedValue;
            
            //throws an event with the new health value as a parameter
            OnPlayerHealthUpdated?.Invoke(updatedValue);
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
            OnPlayerSaturationUpdated?.Invoke(updatedValue);
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
            OnPlayerHydrationUpdated?.Invoke(updatedValue);
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
            OnPlayerDeath?.Invoke();
            Debug.Log("Player is dead");
        }
    }
}