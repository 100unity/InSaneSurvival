using System;
using Interfaces;
using Inventory;
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

        public event Action<Equipable, Equipable> OnItemEquipped;

        /// <summary>
        /// The current ItemHandler that holds all items.
        /// </summary>
        public IItemHandler ItemHandler => playerInventory;

        /// <summary>
        /// The currently equipped item
        /// </summary>
        public Equipable CurrentlyEquippedItem { get; private set; }

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
        /// Equips/Unequips the provided item.
        /// </summary>
        /// <param name="equipable">Item to be equipped</param>
        public void ToggleEquipableItem(Equipable equipable)
        {
            Equipable old = CurrentlyEquippedItem;
            if (CurrentlyEquippedItem == null)
            {
                CurrentlyEquippedItem = equipable;
                OnItemEquipped?.Invoke(old, CurrentlyEquippedItem);
                return;
            }

            // If already equipped, unequip it or switch
            CurrentlyEquippedItem = CurrentlyEquippedItem != equipable ? equipable : null;
            OnItemEquipped?.Invoke(old, CurrentlyEquippedItem);
        }
    }
}