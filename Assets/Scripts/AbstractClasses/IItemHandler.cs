using System;
using Crafting;
using Inventory;

namespace Interfaces
{
    /// <summary>
    /// Everything that stores items. Especially used in <see cref="CraftingRecipe"/> for crafting.
    /// </summary>
    public interface IItemHandler
    {
        /// <summary>
        /// Event that will be triggered after an item is added/removed
        /// <para>Int will be negative when removing an item</para>
        /// </summary>
        event Action<Item, int> ItemsUpdated;

        /// <summary>
        /// Checks if the given item is present (in given quantity)
        /// </summary>
        /// <param name="item">The item to be checked</param>
        /// <param name="amount">The amount of this item to be present</param>
        /// <returns>Whether the item and the given amount is present</returns>
        bool ContainsItemAmount(Item item, int amount = 1);

        /// <summary>
        /// Adds an item with a specified quantity
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <param name="amount">The amount to be added</param>
        void AddItem(Item item, int amount = 1);

        /// <summary>
        /// Removes an item with a specified quantity
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <param name="amount">The amount to be removed</param>
        void RemoveItem(Item item, int amount = 1);
    }
}