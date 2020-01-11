using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using Utils;

namespace Crafting
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe", order = 0)]
    public class CraftingRecipe : ScriptableObject
    {
        [Header("Items")] [Tooltip("All needed items for crafting the new item(s) with this recipe")] [SerializeField]
        private List<ItemResourceData> neededItems;

        [Tooltip("The item that will be created with this recipe")] [SerializeField]
        private ItemResourceData createdItem;

        [Header("Recipe")] [Tooltip("The crafting station needed to craft this recipe")] [SerializeField]
        private CraftingManager.CraftingStation craftingStation;

        [Tooltip(
            "[CAN BE UNDEFINED]\nThe name to be displayed for this recipe. If not defined, will use the name of the created item")]
        [SerializeField]
        private string recipeName;

        /// <summary>
        /// The <see cref="recipeName"/> of this crafting recipe or - if not defined - the name of the first item in the <see cref="createdItem"/> list
        /// </summary>
        public string CreatedItemName => string.IsNullOrEmpty(recipeName) ? createdItem.item.name : recipeName;

        /// <summary>
        /// All needed items for crafting the new item(s) with this recipe
        /// </summary>
        public List<ItemResourceData> NeededItems => neededItems;

        /// <summary>
        /// The item that will be created with this recipe
        /// </summary>
        public ItemResourceData CreatedItem => createdItem;

        /// <summary>
        /// Checks if all ingredients for this recipe are present. If one item is missing or the amount of items does not match the needed amount, false will be returned.
        /// <para>Also checks if the needed crafting station is currently been used.</para>
        /// </summary>
        /// <returns>Whether all needed items are present</returns>
        public bool CanCraft()
        {
            return neededItems.All(neededItem => itemHandler.ContainsItem(neededItem.item, neededItem.amount)) &&
                   (CraftingManager.Instance.HasCraftingStation(craftingStation));
        }

        /// <summary>
        /// Crafts the new item(s) ith this recipe
        /// </summary>
        public void Craft()
        {
            if (!CanCraft())
                return;

            // Remove used item(s)
            foreach (ItemResourceData neededItem in neededItems)
                InventoryManager.Instance.RemoveItem(neededItem.item, neededItem.amount);

            // Add newly created item
            if (InventoryManager.Instance.AddItem(createdItem.item, createdItem.amount)) return;

            // Refund used item(s) if inventory is full
            foreach (ItemResourceData neededItem in neededItems)
                InventoryManager.Instance.AddItem(neededItem.item, neededItem.amount);
        }
    }
}