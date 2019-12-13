using System.Collections.Generic;
using System.Linq;
using Crafting;
using Utils;

namespace Managers
{
    /// <summary>
    /// Used for crafting new items with recipes.
    /// </summary>
    public class CraftingManager : Singleton<CraftingManager>
    {
        /// <summary>
        /// All recipes in the game
        /// </summary>
        public List<CraftingRecipe> Recipes => _recipes;

        private List<CraftingRecipe> _recipes;

        protected override void Awake()
        {
            base.Awake();
            GetRecipes();
        }

        /// <summary>
        /// Gets all recipes in the project
        /// </summary>
        private void GetRecipes() => _recipes = this.GetAllInstances<CraftingRecipe>().ToList();
    }
}