using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Inventory;
using UnityEngine;

namespace Crafting
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe", order = 0)]
    public class CraftingRecipe : ScriptableObject
    {
        [Tooltip("All needed items for crafting the new item(s) with this recipe")] [SerializeField]
        private List<ResourceData> neededItems;

        [Tooltip("The item(s) that will be created with this recipe")] [SerializeField]
        private List<ResourceData> createdItems;

        [Tooltip(
            "[CAN BE UNDEFINED]\nThe name to be displayed for this recipe. If not defined, will use the name of the first created item")]
        [SerializeField]
        private string recipeName;

        /// <summary>
        /// The <see cref="recipeName"/> of this crafting recipe or - if not defined - the name of the first item in the <see cref="createdItems"/> list
        /// </summary>
        public string CreatedItemName => string.IsNullOrEmpty(recipeName) ? createdItems[0].item.name : recipeName;

        /// <summary>
        /// All needed items for crafting the new item(s) with this recipe
        /// </summary>
        public List<ResourceData> NeededItems => neededItems;

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
            foreach (ResourceData neededItem in neededItems)
                itemHandler.RemoveItem(neededItem.item, neededItem.amount);

            // Add newly created item(s)
            foreach (ResourceData createdItem in createdItems)
                itemHandler.AddItem(createdItem.item, createdItem.amount);
        }


        /// <summary>
        /// Small struct for storing the item and the amount of the item as an "ingredient" for the recipe.
        /// </summary>
        [Serializable]
        public struct ResourceData
        {
            public Item item;
            public int amount;
        }
    }
}