using System;
using Interfaces;
using Remote;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Player
{
    
    public class PlayerState : MonoBehaviour, IDamageable
    {
        public delegate void PlayerStateChanged(int newValue);
        
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

        private void Awake()
        {
            RemoteStatusHandler.OnPlayerHealthRemoteUpdate += ChangePlayerHealth;
            RemoteStatusHandler.OnPlayerHydrationRemoteUpdate += ChangePlayerHydration;
            RemoteStatusHandler.OnPlayerSaturationRemoteUpdate += ChangePlayerSaturation;
        }

        //Interface
        public void ChangePlayerHealth(int changeBy)
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

        public void Hit(int damage)
        {
            ChangePlayerHealth(-damage);
        }

        public void Die()
        {
            Debug.Log("Player is dead");
        }
    }
}