using System;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Player
{
    
    [System.Serializable] public class UnityEventInt:UnityEvent<int> {}
    public class PlayerState : MonoBehaviour
    {
        private readonly Random _random = new Random();
        
        //Player State values
        [Tooltip("100 - full health, 0 - dead")] [SerializeField] [Range(0, 100)] 
        private int health;

        [Tooltip("100: no hunger, 0: the player is in desperate need for food")] [SerializeField] [Range(0, 100)] 
        private int saturation;

        [Tooltip("100: not thirsty, 0: gazing for a sip of water")] [SerializeField] [Range(0, 100)] 
        private int hydration;
        
        [Header("Event that will throw when the player's health changes")]
        public UnityEventInt playerHealthUpdated;

        [Header("Event that will throw when the player's saturation changes")]
        public UnityEventInt playerSaturationUpdated;
        
        [Header("Event that will throw when the player's hydration changes")]
        public UnityEventInt playerHydrationUpdated;
        
        //Interface
        public void ChangePlayerHealth(int changeBy)
        {
            int updatedValue = health + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                //TODO: invoke death
            }

            health = updatedValue;
            
            //throws an event with the new health value as a parameter
            playerHealthUpdated.Invoke(updatedValue);
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
            playerSaturationUpdated.Invoke(updatedValue);
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
            playerHydrationUpdated.Invoke(updatedValue);
        }
    }
}