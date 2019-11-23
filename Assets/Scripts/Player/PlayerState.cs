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
        private void FixedUpdate()
        {
            //only for testing
            if (_random.NextDouble() < 0.002)
            {
                ChangePlayerHealth(-1);
            }
        }

        //Player State values
        [SerializeField] [Range(0, 100)] 
        private int health;

        [SerializeField] [Range(0, 100)] 
        private int hunger;

        [SerializeField] [Range(0, 100)] 
        private int thirst;
        
        [Header("Event that will throw when the player's state changes")]
        public UnityEventInt playerHeathUpdated;

        
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
    }
}