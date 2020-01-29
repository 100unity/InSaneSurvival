using System.Collections.Generic;
using Crafting;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// ATM used for storing the recipes
    /// </summary>
    public class CraftingManager : Singleton<CraftingManager>
    {
        /// <summary>
        /// Crafting stations that limit the crafting of specific items to them.
        /// </summary>
        public enum CraftingStation { None, CraftingBench, Campfire, Anvil }

        /// <summary>
        /// Will be invoked when something related to crafting changes (e.g. <see cref="CurrentCraftingStation"/>).
        /// </summary>
        public event CraftingDelegate OnCraftingUpdate;

        public delegate void CraftingDelegate();

        /// <summary>
        /// All recipes in the game
        /// </summary>
        [Tooltip("All crafting recipes the player can craft")] [SerializeField]
        private List<CraftingRecipe> recipes;

        /// <summary>
        /// All recipes in the game
        /// </summary>
        public List<CraftingRecipe> Recipes => recipes;

        /// <summary>
        /// The currently used crafting station(s). <see cref="CraftingStation.None"/> is always included
        /// </summary>
        private readonly HashSet<CraftingStation> _craftingStations = new HashSet<CraftingStation>
            {CraftingStation.None};

        public bool HasCraftingStation(CraftingStation craftingStation) => _craftingStations.Contains(craftingStation);

        /// <summary>
        /// Adds a crafting station to the set. Invokes <see cref="OnCraftingUpdate"/>.
        /// </summary>
        /// <param name="craftingStation">The new crafting station</param>
        public void AddCraftingStation(CraftingStation craftingStation)
        {
            _craftingStations.Add(craftingStation);
            OnCraftingUpdate?.Invoke();
        }

        /// <summary>
        /// Removes a crafting station to the set. Invokes <see cref="OnCraftingUpdate"/>.
        /// </summary>
        /// <param name="craftingStation">The old crafting station (aka to be removed)</param>
        public void RemoveCraftingStation(CraftingStation craftingStation)
        {
            _craftingStations.Remove(craftingStation);
            OnCraftingUpdate?.Invoke();
        }
    }
}