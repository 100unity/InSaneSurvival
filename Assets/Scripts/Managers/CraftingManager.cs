using System.Collections.Generic;
using System.Linq;
using Crafting;
using Interfaces;
using Utils;

namespace Managers
{
    /// <summary>
    /// Used for crafting new items with recipes.
    /// </summary>
    public class CraftingManager : Singleton<CraftingManager>
    {
        private List<ItemRecipe> _recipes = new List<ItemRecipe>();

        protected override void Awake()
        {
            base.Awake();
            GetRecipes();
        }

        /// <summary>
        /// Checks if the given recipe can be crafted. See ItemRecipe.<see cref="ItemRecipe.CanCraft"/> for more details
        /// </summary>
        /// <param name="recipe">The recipe to be checked</param>
        /// <param name="itemHandler">The object that holds the items</param>
        /// <returns>Whether the recipe can be crafted</returns>
        public bool CanCraftRecipe(ItemRecipe recipe, IItemHandler itemHandler) => recipe.CanCraft(itemHandler);

        public void CraftRecipe(ItemRecipe recipe, IItemHandler itemHandler)
        {
            if (!CanCraftRecipe(recipe, itemHandler))
                return;
            recipe.Craft(itemHandler);
        }

        /// <summary>
        /// Gets all recipes in the project
        /// </summary>
        private void GetRecipes()
        {
            _recipes = this.GetAllInstances<ItemRecipe>().ToList();
        }
    }
}