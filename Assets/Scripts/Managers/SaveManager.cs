using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Entity.Player;
using GameTime;
using Interactables;
using Inventory;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Utils.Saves;

namespace Managers
{
    public class SaveManager : Singleton<SaveManager>
    {
        /// <summary>
        /// Saves the current state of the game to a JSON file
        /// </summary>
        /// <param name="fileName">The file name to use for the JSON file</param>
        public static void Save(string fileName)
        {
            try
            {
                // grab required components to extract data from
                GameObject player = PlayerManager.Instance.GetPlayer();

                // this is the player's current position in the map
                Vector3 playerPosition = player.transform.localPosition;

                // get health, hunger, thirst, sanity, ... from here
                PlayerState state = player.GetComponent<PlayerState>();

                //get a json-able list of items in the player's inventory
                List<string> inventoryData = InventoryManager.Instance.GetInvController()
                    .GetItems().Select(i => i.name).ToList();

                //get all campsites
                List<Campsite> campsites = CampsiteManager.Instance.campsites;

                Clock clock = DayNightManager.Instance.GetComponent<Clock>();
                float time = clock.TimeOfDay;
                int dayNumber = clock.Days;

                //get all harvestables
                Harvestable[] harvestables = FindObjectsOfType<Harvestable>();
                List<SavedHarvestable> sh = harvestables.Select(h => new SavedHarvestable
                {
                    id = h.gameObject.GetId(),
                    isRespawning = h.IsRespawning,
                    respawnTimePassed = h.RespawnTimePassed
                }).ToList();

                // build a JSON-Object
                Save save = new Save();

                save.SetPlayerState(playerPosition, state.Health, state.Saturation, state.Hydration,
                    state.Sanity);
                save.SetWorldState(time, dayNumber);
                save.SetInventory(inventoryData);
                save.buildVersion = Application.version;

                save.campsites = new List<SavedCampsite>();
                campsites.ForEach(c =>
                {
                    List<SavedBlueprint> blueprints = c.buildingBlueprints.Select(b => new SavedBlueprint
                    {
                        blueprintId = b.gameObject.GetId(),
                        buildingId = b.Building.gameObject.GetId(),
                        blueprintActive = b.IsActive,
                        buildingActive = b.Building.IsBuilt
                    }).ToList();
                    save.campsites.Add(new SavedCampsite
                    {
                        id = c.gameObject.GetId(),
                        isUnlocked = c.IsUnlocked,
                        blueprints = blueprints
                    });
                });

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
        /// <param name="fileName">The file to load</param>
        public static void Load(string fileName)
        {
            try
            {
                Save save = Read(fileName);

                LoadPlayer(save);
                LoadDayTime(save);
                LoadCampsites(save);
                LoadHarvestables(save);
                LoadInventory(save);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        /// <summary>
        /// Loads the saved player state and restores it in the scene
        /// </summary>
        /// <param name="save">The save to load the player state from</param>
        private static void LoadPlayer(Save save)
        {
            // Get player instance
            GameObject player = PlayerManager.Instance.GetPlayer();

            // Set player position
            player.GetComponent<NavMeshAgent>().Warp(save.playerPosition);

            // Set player state
            PlayerState state = player.GetComponent<PlayerState>();
            state.Health = save.playerHealth;
            state.Hydration = save.playerHydration;
            state.Sanity = save.playerSanity;
            state.Saturation = save.playerSaturation;
        }

        /// <summary>
        /// Loads the saved daytime and restores it in the scene
        /// </summary>
        /// <param name="save">The save to load the daytime from</param>
        private static void LoadDayTime(Save save)
        {
            DayNightManager timeManager = DayNightManager.Instance;
            timeManager.SetTimeOfDay(save.timeOfDay);
            timeManager.SetDayNumber(save.dayNumber);
        }

        /// <summary>
        /// Loads the saved campsite states and restores them in the scene
        /// </summary>
        /// <param name="save">The save to load the campsites from</param>
        private static void LoadCampsites(Save save)
        {
            // Get identifiable references to objects in scene
            Dictionary<string, Campsite> campsiteMap = GetIdentifiableObjects<Campsite>();
            Dictionary<string, BuildingBlueprint> blueprintMap = GetIdentifiableObjects<BuildingBlueprint>();
            Dictionary<string, Building> buildingMap = GetIdentifiableObjects<Building>();

            // Load the saved state for each campsite and its buildings
            save.campsites.ForEach(savedCampsite =>
            {
                Campsite campsite = campsiteMap[savedCampsite.id];
                savedCampsite.blueprints.ForEach(savedBlueprint =>
                {
                    if (!blueprintMap.TryGetValue(savedBlueprint.blueprintId, out BuildingBlueprint blueprint))
                    {
                        print("Could not find blueprint");
                        return;
                    }

                    if (!buildingMap.TryGetValue(savedBlueprint.buildingId, out Building building))
                    {
                        print("Could not find building");
                        return;
                    }

                    blueprint.Building = building;
                    if (savedBlueprint.blueprintActive && savedBlueprint.buildingActive)
                    {
                        building.IsBuilt = true;
                        blueprint.gameObject.GetComponentInChildren<BlueprintTooltip>().gameObject.SetActive(false);
                    }
                    else if (savedBlueprint.blueprintActive && !savedBlueprint.buildingActive)
                    {
                        building.IsBuilt = false;
                    }
                });
                if (savedCampsite.isUnlocked && !campsite.IsUnlocked)
                    campsite.UnlockBuildingBlueprintsInstantly();
            });
        }

        /// <summary>
        /// Loads the saved harvestable states and restores them in the scene
        /// </summary>
        /// <param name="save">The save to load the harvestables from</param>
        private static void LoadHarvestables(Save save)
        {
            Dictionary<string, Harvestable> harvestableMap = GetIdentifiableObjects<Harvestable>();
            save.harvestables.ForEach(harvestable =>
                harvestableMap[harvestable.id]
                    .SetFromSave(harvestable.isRespawning, harvestable.respawnTimePassed));
        }

        /// <summary>
        /// Loads the saved inventory and restores them in the scene
        /// </summary>
        /// <param name="save">The save to load the inventory from</param>
        private static void LoadInventory(Save save)
        {
            Item[] items = Resources.FindObjectsOfTypeAll<Item>();
            Dictionary<string, Item> itemMap = items
                .Select(i => new KeyValuePair<string, Item>(i.name, i))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            InventoryManager.Instance.GetInvController().SetItems(save.items.Select(i => itemMap[i]).ToList());
        }

        /// <summary>
        /// Finds all objects of the specified type in the scene and maps them to a pseudo-unique ID
        /// </summary>
        /// <typeparam name="T">The type of objects to find</typeparam>
        /// <returns>A map of pseudo-unique object IDs and their respective objects</returns>
        private static Dictionary<string, T> GetIdentifiableObjects<T>() where T : MonoBehaviour
        {
            T[] objects = Resources.FindObjectsOfTypeAll<T>();
            return objects
                .Where(o => o.gameObject.scene.isLoaded)
                .Select(o => new KeyValuePair<string, T>(o.gameObject.GetId(), o))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        //JSON - Utility functions - interaction with File system...

        private static void Write(Save save, string fileName = "")
        {
            string path = Application.persistentDataPath;
            string json = JsonUtility.ToJson(save);
            if (fileName == "") System.IO.File.WriteAllText(@"" + path + "/save.json", json);
            else System.IO.File.WriteAllText(@"" + path + "/" + fileName + ".json", json);
        }

        private static Save Read(string fileName)
        {
            string path = Application.persistentDataPath;
            string json = fileName == ""
                ? System.IO.File.ReadAllText(@"" + path + "/save.json")
                : System.IO.File.ReadAllText(@"" + path + "/" + fileName + ".json");
            return JsonUtility.FromJson<Save>(json);
        }
    }
}