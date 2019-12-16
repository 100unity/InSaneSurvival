using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Inventory;
using UnityEngine;

namespace Utils.Saves
{
    [System.Serializable]
    public class Save
    {
        //metadata
        public string timestamp;

        //player state
        public Vector3 playerPosition;
        
        public int playerHealth;
        public int playerSaturation;
        public int playerHydration;
        public int playerSanity;

        //inventory data
        public List<Item> items;
        
        //world state data
        public int timeOfDay;
        public string weather;
        
        //world object data

        public Save()
        {
            timestamp = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
        }

        public void SetPlayerState(Vector3 pos, int health, int saturation, int hydration, int sanity)
        {
            playerHealth = health;
            playerHydration = hydration;
            playerSanity = sanity;
            playerSaturation = saturation;

            playerPosition = pos;
        }

        public void SetInventory(List<Item> itemList) => this.items = itemList;
    }
}