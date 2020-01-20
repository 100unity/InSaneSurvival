using System;
using System.Collections.Generic;
using Buildings;
using Entity.Player;
using GameTime;
using Interactables;
using Inventory;
using UnityEngine;
using UnityEngine.AI;
using Utils.Saves;

namespace Managers
{
    public class SaveManager : Singleton<SaveManager>
    {
        /// <summary>
        /// saves the current state of the game to JSON
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
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

                //get all harvestables
                Harvestable[] harvestables = FindObjectsOfType<Harvestable>();
                List<SavedHarvestable> sh = new List<SavedHarvestable>();
                foreach (Harvestable h in harvestables)
                {
                    sh.Add(new SavedHarvestable(h, h.IsRespawning, h.RespawnTimePassed));
                }

                // build a JSON-Object
                Save save = new Save();

                save.SetPlayerState(playerPosition, state.GetHealth(), state.GetSaturation(), state.GetHydration(),
                    state.GetSanity());
                save.SetWorldState(time, daynumber);
                save.SetInventory(inventoryData);
                save.buildVersion = Application.version;

                save.campsites = new List<SavedCampsite>();
                campsites.ForEach(cs => save.campsites.Add(new SavedCampsite(cs, cs.IsUnlocked)));

                save.harvestables = sh;

                Write(save, fileName);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        /// <summary>
        /// loads the savegame from the file & sets the states of gameobjects sequentially afterwards...
        /// </summary>
        /// <param name="fileName"></param>
        public void Load(string fileName)
        {
            try
            {
                // get save object
                Save save = Read(fileName);

                // get game components
                GameObject player = PlayerManager.Instance.GetPlayer();
                InventoryController inventoryController = InventoryManager.Instance.GetInvController();
                DayNightManager timeManager = DayNightManager.Instance;

                // set player position
                player.GetComponent<NavMeshAgent>().Warp(save.playerPosition);

                // set player state
                PlayerState state = player.GetComponent<PlayerState>();
                state.SetHealth(save.playerHealth);
                state.SetHydration(save.playerHydration);
                state.SetSanity(save.playerSanity);
                state.SetSaturation(save.playerSaturation);

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
                            sbp.blueprint.gameObject.GetComponentInChildren<BlueprintTooltip>().gameObject
                                .SetActive(false);
                            //sbp.blueprint.ShowBlueprint();
                            //sbp.blueprint.ShowBuilding();
                        }
                        else if (sbp.blueprintActive && !sbp.buildingActive)
                        {
                            sbp.building.IsBuilt = false;
                            //sbp.blueprint.ShowBlueprint();
                        }
                    });
                    cs.SetState();
                });

                //set harvestables
                save.harvestables.ForEach(h => { h.SetState(); });

                // set inventory
                inventoryController.SetItems(save.items);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }


        //JSON - Utility functions - interaction with File system...

        private void Write(Save save, string fileName)
        {
            string path = Application.persistentDataPath;
            string json = JsonUtility.ToJson(save);
            if (fileName == "") System.IO.File.WriteAllText(@"" + path + "/save.json", json);
            else System.IO.File.WriteAllText(@"" + path + "/" + fileName + ".json", json);
        }

        private Save Read(string fileName)
        {
            string path = Application.persistentDataPath;
            string json = fileName == ""
                ? System.IO.File.ReadAllText(@"" + path + "/save.json")
                : System.IO.File.ReadAllText(@"" + path + "/" + fileName + ".json");
            return JsonUtility.FromJson<Save>(json);
        }
    }
}