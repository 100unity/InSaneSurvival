using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Inventory;
using UnityEngine;

namespace Crafting
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe", order = 0)]
    public class ItemRecipe : ScriptableObject
    {
        [Tooltip("All needed items for crafting the new item(s) with this recipe")] [SerializeField]
        private List<RecipeItemData> neededItems;

        [Tooltip("The item(s) that will be created with this recipe")] [SerializeField]
        private List<RecipeItemData> createdItems;

        /// <summary>
        /// Checks if all ingredients for this recipe are present. If one item is missing or the amount of items does not match the needed amount, false will be returned.
        /// </summary>
        /// <param name="itemHandler">The object that holds the items</param>
        /// <returns>Whether all needed items are present</returns>
        public bool CanCraft(IItemHandler itemHandler) =>
            neededItems.All(neededItem => itemHandler.ContainsItem(neededItem.item, neededItem.amount));

        /// <summary>
        /// Crafts the new item(s) ith this recipe
        /// </summary>
        /// <param name="itemHandler">The object that holds the items</param>
        public void Craft(IItemHandler itemHandler)
        {
            if (!CanCraft(itemHandler))
                return;

            // Remove used item(s)
            foreach (RecipeItemData neededItem in neededItems)
                itemHandler.RemoveItem(neededItem.item, neededItem.amount);

            // Add newly created item(s)
            foreach (RecipeItemData createdItem in createdItems)
                itemHandler.AddItem(createdItem.item, createdItem.amount);
        }


        /// <summary>
        /// Small struct for storing the item and the amount of the item as an "ingredient" for the recipe.
        /// </summary>
        [Serializable]
        private struct RecipeItemData
        {
            public Item item;
            public int amount;
        }
    }
}