using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Player;
using Inventory;
using UnityEngine;
using UnityEngine.AI;
using Utils.Saves;

namespace Managers
{
    public class SaveManager: Singleton<SaveManager>
    {
        public void Save(string fileName)
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
                
                /*List<Item> inventoryDataNew = new List<Item>();
                foreach (var item in inventoryData)
                {
                    inventoryDataNew.Add(JsonUtility.FromJson<Item>(JsonUtility.ToJson(item)));
                }*/

                // build a JSON-Object
                Save save = new Save();

                save.SetPlayerState(playerPosition, state.GetHealth(), state.GetSaturation(), state.GetHydration(), state.GetSanity());
                save.SetInventory(inventoryData);
                //save.SetInventoryNew(inventoryDataNew);

                Write(save, fileName);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }

        public void Load(string fileName)
        {
            Debug.Log("loading initiated");
            try
            {
                // get save object
                Save save = Read(fileName);
                
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
        
        private void Write(Save save, string fileName)
        {
            string json = JsonUtility.ToJson(save);
            
            if(fileName == "") System.IO.File.WriteAllText(@"C:\Users\Public\save.json", json);
            else System.IO.File.WriteAllText(@"C:\Users\Public\"+fileName+".json", json);
            
            Debug.Log("save written to file");
        }

        private Save Read(string fileName)
        {
            string json = fileName == "" ? System.IO.File.ReadAllText(@"C:\Users\Public\save.json") : System.IO.File.ReadAllText(@"C:\Users\Public\"+fileName+".json");
            return JsonUtility.FromJson<Save>(json);
        }

        [System.Serializable]
        public class SavedItem
        {
            public SavedItem(int id, int instanceId, string itemName)
            {
                this.id = id;
                this.instanceId = instanceId;
                this.itemName = itemName;
            }

            public int id;
            public int instanceId;
            public string itemName;

            public Item ToItem()
            {
                var so = ScriptableObject.CreateInstance<Item>();
                so.id = id;
                so.name = itemName;

                return so;
            }
        }
    }
}