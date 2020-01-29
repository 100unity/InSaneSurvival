using System.Collections.Generic;
using Interfaces;
using Inventory;
using Inventory.UI;
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
        /// The damage boost of the currently equipped weapon. Returns 0 if no item is equipped.
        /// </summary>
        public int DamageBoostFromEquipable => CurrentlyEquippedItem ? CurrentlyEquippedItem.DamageBoost : 0;

        /// <summary>
        /// Whether the inventory still has empty slots. Used for determining if an item can be added.
        /// </summary>
        public bool HasEmptySlots { get; set; }

        /// <summary>
        /// All items that still have an not-full stack. Used for determining if an item can be added.
        /// </summary>
        public HashSet<Item> NotFullItemStacks { get; set; }

        /// <summary>
        /// The currently equipped ItemButton (Visually equipped item).
        /// </summary>
        private ItemButton _currentlyEquippedItemButton;

        /// <summary>
        /// The currently equipped item.
        /// </summary>
        public Equipable CurrentlyEquippedItem { get; set; }

        /// <summary>
        /// The ItemButton of the currently equipped item.
        /// </summary>
        public ItemButton CurrentlyEquippedItemButton => _currentlyEquippedItemButton;

        /// <summary>
        /// Event for when an item button is deleted.
        /// </summary>
        public event ItemButtonRemove OnItemButtonRemove;

        public delegate void ItemButtonRemove(ItemButton itemButton);

        /// <summary>
        /// Adds an item to the player's inventory.
        /// </summary>
        /// <param name="item">The item to be added</param>
        public bool AddItem(Item item)
        {
            if (!HasEmptySlots && !NotFullItemStacks.Contains(item)) return false;

            playerInventory.AddItem(item);
            return true;
        }

        public InventoryController GetInvController() => playerInventory;

        /// <summary>
        /// Removes an item from the player's inventory.
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <param name="amount">The amount</param>
        public void RemoveItem(Item item, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
                playerInventory.RemoveItem(item);
        }

        /// <summary>
        /// Removes an entire item button from the player's inventory.
        /// </summary>
        /// <param name="itemButton">The item to be removed</param>
        public void RemoveItemButton(ItemButton itemButton)
        {
            for (int i = 0; i < itemButton.Count; i++)
                playerInventory.RemoveItemSilently(itemButton.Item);
            OnItemButtonRemove?.Invoke(itemButton);
            if(itemButton.Item is Equipable)
                SetCurrentlyEquipped(null);
        }

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
            UIManager.Instance.WearingIndicator.SetIcon(itemButton ? itemButton.Item.Icon : null);
            if (_currentlyEquippedItemButton != null)
                _currentlyEquippedItemButton.Unequip();
            _currentlyEquippedItemButton = itemButton;
        }
    }
}