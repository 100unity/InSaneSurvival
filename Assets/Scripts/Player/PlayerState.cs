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

        [Tooltip("100 means no hunger, 0 means the player is in desperate need for food")] [SerializeField] [Range(0, 100)] 
        private int hunger;

        [Tooltip("100: not thirsty, 0: gazing for a sip of water")] [SerializeField] [Range(0, 100)] 
        private int thirst;
        
        [Header("Event that will throw when the player's health changes")]
        public UnityEventInt playerHeathUpdated;

        [Header("Event that will throw when the player's hunger changes")]
        public UnityEventInt playerHungerUpdated;
        
        [Header("Event that will throw when the player's thirst changes")]
        public UnityEventInt playerThirstUpdated;
        
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
            playerHeathUpdated.Invoke(updatedValue);
        }

        public void ChangePlayerHunger(int changeBy)
        {
            int updatedValue = hunger + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                ChangePlayerHealth(-1);
            }

            hunger = updatedValue;
            playerHungerUpdated.Invoke(updatedValue);
        }

        public void ChangePlayerThrist(int changeBy)
        {
            int updatedValue = thirst + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                ChangePlayerHealth(-1);
            }

            thirst = updatedValue;
            playerThirstUpdated.Invoke(updatedValue);
        }
    }
}