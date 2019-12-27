using Interfaces;
using Inventory;
using UI;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Currently handles equipping items.
    /// </summary>
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Tooltip("Player's inventory")] [SerializeField]
        private InventoryController playerInventory;

        /// <summary>
        /// The current ItemHandler that holds all items.
        /// </summary>
        public IItemHandler ItemHandler => playerInventory;

        /// <summary>
        /// The currently equipped item
        /// </summary>
        public Equipable CurrentlyEquippedItem { get; private set; }

        /// <summary>
        /// The currently equipped ItemButton (Visually equipped item)
        /// </summary>
        private ItemButton _currentlyEquippedItemButton;

        /// <summary>
        /// Adds an item to the player's inventory.
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <param name="amount">The amount</param>
        public void AddItem(Item item, int amount = 1) => playerInventory.AddItem(item, amount);

        /// <summary>
        /// Removes an item from the player's inventory.
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <param name="amount">The amount</param>
        public void RemoveItem(Item item, int amount = 1) => playerInventory.RemoveItem(item, amount);

        /// <summary>
        /// Refreshes all items in the inventory.
        /// </summary>
        public void RefreshItems() => playerInventory.RefreshItems();

        /// <summary>
        /// Saves the given ItemButton as the equipped item, saves the item it holds and updates the old ItemButton
        /// </summary>
        /// <param name="itemButton">The new equipped itemButton (that holds the equipped item)</param>
        public void SetCurrentlyEquipped(ItemButton itemButton)
        {
            CurrentlyEquippedItem = itemButton ? (Equipable) itemButton.Item : null;
            if (_currentlyEquippedItemButton != null)
                _currentlyEquippedItemButton.Unequip();
            _currentlyEquippedItemButton = itemButton;
        }
    }
}