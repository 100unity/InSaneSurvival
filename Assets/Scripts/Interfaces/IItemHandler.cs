using Inventory;

namespace Interfaces
{
    /// <summary>
    /// Everything that stores items. Especially used in <see cref="Crafting.ItemRecipe"/> for crafting.
    /// </summary>
    public interface IItemHandler
    {
        /// <summary>
        /// Checks if the given item is present (in given quantity)
        /// </summary>
        /// <param name="item">The item to be checked</param>
        /// <param name="amount">The amount of this item to be present</param>
        /// <returns>Whether the item and the given amount is present</returns>
        bool ContainsItem(Item item, int amount = 1);

        /// <summary>
        /// Adds an item with a specified quantity
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <param name="amount">The amount to be added</param>
        void AddItem(Item item, int amount);

        /// <summary>
        /// Removes an item with a specified quantity
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <param name="amount">The amount to be removed</param>
        void RemoveItem(Item item, int amount = 1);
    }
}