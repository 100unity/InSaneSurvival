using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject itemGrid;
        [SerializeField] private ItemButton itemButtonPrefab;
        [SerializeField] private InventoryController inventory;

        [Tooltip("Used for showing and hiding the inventory")] [SerializeField]
        private GameObject inventoryUIContent;

        private readonly Dictionary<Item, ItemButton> _itemStacks = new Dictionary<Item, ItemButton>();
        public bool IsActive { get; private set; }

        private void Awake() => inventory.ItemsUpdated += ItemsUpdated;

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
            IsActive = !IsActive;
            inventoryUIContent.SetActive(IsActive);
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
                _itemStacks[item].Count += 1;
                return;
            }

            ItemButton itemButton = Instantiate(itemButtonPrefab, itemGrid.transform);
            itemButton.Icon.sprite = item.Icon;
            itemButton.NameLabel.text = item.name;
            itemButton.Count = 1;
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