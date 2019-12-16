using System;
using System.Collections.Generic;
using Entity.Player;
using Inventory;
using UnityEngine;
using UnityEngine.AI;
using Utils.Saves;

namespace Managers
{
    public class SaveManager: Singleton<SaveManager>
    {
        public void Save()
        {
            Debug.Log("save initiated");
            try
            {
                // grab required components to extract data from
                GameObject player = PlayerManager.Instance.GetPlayer();
                InventoryController inventoryController = player.GetComponentInChildren<InventoryController>();

                // this is the player's current position in the map
                Vector3 playerPosition = player.transform.localPosition;

                // get health, hunger, thirst, sanity, ... from here
                PlayerState state = player.GetComponent<PlayerState>();

                //get a json-able list of items in the player's inventory
                List<Item> inventoryData = inventoryController.GetItems();

                // build a JSON-Object
                Save save = new Save();

                save.SetPlayerState(playerPosition, state.GetHealth(), state.GetSaturation(), state.GetHydration(), state.GetSanity());
                save.SetInventory(inventoryData);

                Write(save);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }

        public void Load()
        {
            Debug.Log("loading initiated");
            try
            {
                // get save object
                Save save = Read();
                
                // get game components
                GameObject player = PlayerManager.Instance.GetPlayer();
                InventoryController inventoryController = player.GetComponentInChildren<InventoryController>();

                // set player position
                player.GetComponent<NavMeshAgent>().Warp(save.playerPosition);
                
                // set player state
                PlayerState state = player.GetComponent<PlayerState>();
                state.SetHealth(save.playerHealth);
                state.SetHydration(save.playerHydration);
                state.SetSanity(save.playerSanity);
                state.SetSaturation(save.playerSaturation);
                
                inventoryController.SetItems(save.items);
                
                Debug.Log("save recreated");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
            
        }
        
        
        //JSON - Utility functions
        
        private void Write(Save save)
        {
            string json = JsonUtility.ToJson(save);
            System.IO.File.WriteAllText(@"C:\Users\Public\save.json", json);
            Debug.Log("save written to file");
        }

        private Save Read()
        {
            string json = System.IO.File.ReadAllText(@"C:\Users\Public\save.json");
            return JsonUtility.FromJson<Save>(json);
        }
    }
}