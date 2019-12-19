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
        /// All recipes in the game
        /// </summary>
        [Tooltip("All crafting recipes the player can craft")] [SerializeField]
        private List<CraftingRecipe> recipes;

        /// <summary>
        /// All recipes in the game
        /// </summary>
        public List<CraftingRecipe> Recipes => recipes;
    }
}