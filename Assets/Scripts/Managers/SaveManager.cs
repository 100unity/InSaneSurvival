﻿using System;
using System.Collections.Generic;
using Buildings;
using Entity.Player;
using GameTime;
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
                InventoryController inventoryController = InventoryManager.Instance.GetInvController();

                // this is the player's current position in the map
                Vector3 playerPosition = player.transform.localPosition;

                // get health, hunger, thirst, sanity, ... from here
                PlayerState state = player.GetComponent<PlayerState>();

                //get a json-able list of items in the player's inventory
                List<Item> inventoryData = inventoryController.GetItems();
                
                //get all campsites
                List<Campsite> campsites = CampsiteManager.Instance.campsites;
                
                //get all buildings
                Building[] buildings = FindObjectsOfType<Building>();
                BuildingBlueprint[] blueprints = FindObjectsOfType<BuildingBlueprint>();

                Clock clock = DayNightManager.Instance.GetComponent<Clock>();
                float time = clock.TimeOfDay;
                int daynumber = clock.Days;
                
                // build a JSON-Object
                Save save = new Save();

                save.SetPlayerState(playerPosition, state.GetHealth(), state.GetSaturation(), state.GetHydration(), state.GetSanity());
                save.SetWorldState(time, daynumber);
                save.SetInventory(inventoryData);
                save.buildVersion = Application.version;
                
                save.campsites = new List<SavedCampsite>();
                campsites.ForEach(cs => save.campsites.Add(new SavedCampsite(cs, cs.IsUnlocked)));

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
                InventoryController inventoryController = InventoryManager.Instance.GetInvController();
                DayNightManager timeManager = DayNightManager.Instance;
                CampsiteManager campsiteManager = CampsiteManager.Instance;

                // set player position
                player.GetComponent<NavMeshAgent>().Warp(save.playerPosition);
                
                // set player state
                PlayerState state = player.GetComponent<PlayerState>();
                state.SetHealth(save.playerHealth);
                state.SetHydration(save.playerHydration);
                state.SetSanity(save.playerSanity);
                state.SetSaturation(save.playerSaturation);
                
                // set inventory
                inventoryController.SetItems(save.items);

                // set time
                timeManager.SetTimeOfDay(save.timeOfDay);
                timeManager.SetDayNumber(save.dayNumber);
                
                // set campsites and blueprints
                List<SavedCampsite> savedCampsites = save.campsites;
                savedCampsites.ForEach(cs =>
                {
                    cs.campsite.buildingBlueprints = new List<BuildingBlueprint>();
                    cs.blueprints.ForEach(sbp =>
                    {
                        cs.campsite.buildingBlueprints.Add(sbp.blueprint);
                        sbp.blueprint.Building = sbp.building;
                        if (sbp.blueprintActive && sbp.buildingActive)
                        {
                            sbp.building.IsBuilt = true;
                            sbp.blueprint.gameObject.GetComponentInChildren<BlueprintTooltip>().gameObject.SetActive(false);
                            //sbp.blueprint.ShowBlueprint();
                            //sbp.blueprint.ShowBuilding();
                        } else if (sbp.blueprintActive && !sbp.buildingActive)
                        {
                            sbp.building.IsBuilt = false;
                            //sbp.blueprint.ShowBlueprint();
                        }
                    });
                    cs.SetState();
                });
                
                
                
                Debug.Log("save recreated");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        
        //Buildings -  Utility functions
        
        
        
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
    }
}