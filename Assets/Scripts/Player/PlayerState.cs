using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    
    [System.Serializable] public class UnityEventInt:UnityEvent<int> {}
    public class PlayerState : MonoBehaviour
    {
        //Player State values
        [SerializeField] [Range(0, 100)] 
        private int health;

        [SerializeField] [Range(0, 100)] 
        private int hunger;

        [SerializeField] [Range(0, 100)] 
        private int thirst;
        
        [Header("Event that will throw when the player's state changes")]
        public UnityEventInt playerHeathUpdated;

        
        public void SetPlayerHealth(int changeBy)
        {
            var updatedValue = health + changeBy;
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