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
        public enum CraftingStation { None, CraftingBench, Fire }

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
        /// The currently used crafting station. Invokes <see cref="OnCraftingUpdate"/>
        /// </summary>
        public CraftingStation CurrentCraftingStation
        {
            get => _craftingStation;
            set
            {
                OnCraftingUpdate?.Invoke();
                _craftingStation = value;
            }
        }

        /// <summary>
        /// The currently used crafting station.
        /// </summary>
        private CraftingStation _craftingStation;
    }
}