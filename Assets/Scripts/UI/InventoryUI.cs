using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Inventory;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour, IItemHandler
    {
        [SerializeField] private GameObject itemGrid;
        [SerializeField] private ItemButton itemButtonPrefab;
        [SerializeField] private InventoryController inventory;

        [Tooltip("Used for showing and hiding the inventory")] [SerializeField]
        private GameObject inventoryUIContent;

        private readonly Dictionary<Item, ItemButton> _itemStacks = new Dictionary<Item, ItemButton>();

        public event Action ItemsUpdated;
        public bool IsActive { get; private set; }

        private void Awake()
        {
            // Subscribe to inventory events
            inventory.OnItemAdded += AddItem;
            inventory.OnItemRemoved += RemoveItem;
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
        public void AddItem(Item item, int amount)
        {
            if (_itemStacks.ContainsKey(item))
            {
                _itemStacks[item].Count += 1;
                ItemsUpdated?.Invoke();
                return;
            }

            ItemButton itemButton = Instantiate(itemButtonPrefab, itemGrid.transform);
            itemButton.Icon.sprite = item.Icon;
            itemButton.NameLabel.text = item.name;
            itemButton.Count = 1;
            _itemStacks[item] = itemButton;
            ItemsUpdated?.Invoke();
        }

        /// <summary>
        /// Removes an item from the inventory UI or decreases the stack size
        /// by one if there are at least two items of the same type
        /// </summary>
        /// <param name="item">The item to remove from the UI</param>
        /// <param name="amount">The amount to be removed</param>
        public void RemoveItem(Item item, int amount)
        {
            if (!_itemStacks.ContainsKey(item)) return;
            _itemStacks[item].Count -= amount;
            if (_itemStacks[item].Count <= 0)
            {
                Destroy(_itemStacks[item].gameObject);
                _itemStacks.Remove(item);
            }

            ItemsUpdated?.Invoke();
        }

        /// <inheritdoc cref="IItemHandler.ContainsItem"/>
        public bool ContainsItem(Item item, int amount = 1) =>
            _itemStacks.Any(itemStack => item == itemStack.Key && amount <= itemStack.Value.Count);
    }
}