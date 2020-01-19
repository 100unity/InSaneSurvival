using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Buildings;
using Inventory;
using Managers;
using UnityEngine;

namespace Utils.Saves
{
    [System.Serializable]
    public class Save
    {
        //metadata
        public string timestamp;
        public string buildVersion;

        //player state
        public Vector3 playerPosition;
        
        public int playerHealth;
        public int playerSaturation;
        public int playerHydration;
        public int playerSanity;

        //inventory data
        public List<Item> items;
        
        //world state data
        public float timeOfDay;
        public int dayNumber;
        
        //word buildings data
        public List<SavedCampsite> campsites;

        //world resource data
        public List<SavedHarvestable> harvestables;

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

        public void SetWorldState(float time, int day)
        {
            timeOfDay = time;
            dayNumber = day;
        }

        public void SetInventory(List<Item> itemList) => items = itemList;
    }
}