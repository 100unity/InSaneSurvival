using System.Collections.Generic;
using Inventory;
using Managers;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Tooltip("The grid in which the ItemButtons will be spawned")] [SerializeField]
        private GameObject itemGrid;

        [Tooltip("The ItemButtonPrefab for spawning")] [SerializeField]
        private ItemButton itemButtonPrefab;

        [Tooltip("Used for showing and hiding the inventory")] [SerializeField]
        private GameObject inventoryUIContent;

        /// <summary>
        /// All items and the corresponding ItemButtons
        /// </summary>
        private readonly Dictionary<Item, ItemButton> _itemStacks = new Dictionary<Item, ItemButton>();

        /// <summary>
        /// Whether the inventory is currently showing
        /// </summary>
        private bool _isActive;

        private void OnEnable()
        {
            InventoryManager.Instance.ItemHandler.ItemsUpdated += ItemsUpdated;
            InventoryManager.Instance.OnItemEquipped += ItemEquipped;
        }

        private void OnDisable()
        {
            InventoryManager.Instance.ItemHandler.ItemsUpdated -= ItemsUpdated;
            InventoryManager.Instance.OnItemEquipped -= ItemEquipped;
        }

        /// <summary>
        /// Enables/Disables the <see cref="ItemButton.imgIsEquipped"/> for the given items.
        /// </summary>
        /// <param name="oldItem">The item that WAS equipped</param>
        /// <param name="newItem">The item that IS equipped</param>
        private void ItemEquipped(Equipable oldItem, Equipable newItem)
        {
            if (oldItem != null)
                _itemStacks[oldItem]?.ToggleIsEquipped(false);
            if (newItem != null)
                _itemStacks[newItem]?.ToggleIsEquipped(true);
        }

        /// <summary>
        /// Checks if the amount is negative or positive and removes or adds respectively
        /// </summary>
        /// <param name="item">The item to be added/removed</param>
        /// <param name="amount">The amount to be added/removed</param>
        private void ItemsUpdated(Item item, int amount)
        {
            if (amount > 0)
                AddItem(item, amount);
            else
                RemoveItem(item, -amount);
        }

        /// <summary>
        /// Toggles the inventory's visibility
        /// </summary>
        public void ToggleInventory()
        {
            _isActive = !_isActive;
            inventoryUIContent.SetActive(_isActive);
        }

        /// <summary>
        /// Adds an item to the inventory UI or increases the stack size in case it already exists
        /// </summary>
        /// <param name="item">The item to add to the UI</param>
        /// <param name="amount">The amount to be added</param>
        private void AddItem(Item item, int amount)
        {
            if (_itemStacks.ContainsKey(item))
            {
                _itemStacks[item].Count += amount;
                return;
            }

            ItemButton itemButton = Instantiate(itemButtonPrefab, itemGrid.transform);
            itemButton.Init(item, amount);
            _itemStacks[item] = itemButton;
        }

        /// <summary>
        /// Removes an item from the inventory UI or decreases the stack size
        /// by one if there are at least two items of the same type
        /// </summary>
        /// <param name="item">The item to remove from the UI</param>
        /// <param name="amount">The amount to be removed</param>
        private void RemoveItem(Item item, int amount)
        {
            if (!_itemStacks.ContainsKey(item)) return;
            _itemStacks[item].Count -= amount;
            if (_itemStacks[item].Count > 0) return;
            Destroy(_itemStacks[item].gameObject);
            _itemStacks.Remove(item);
        }
    }
}