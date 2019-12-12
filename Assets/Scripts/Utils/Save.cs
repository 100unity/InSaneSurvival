using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class Save
    {
        //metadata
        private int timestamp;

        //player data
        public Vector3 playerPosition;
        public int playerHealth;
        public int playerSaturation;
        public int playerHydration;
        public int playerSanity;

        //inventory data
        public List<Item> items;
        
        //world data
        public int timeOfDay;
        public string weather;

        
    }
}