using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject itemGrid;
        [SerializeField] private ItemButton itemButtonPrefab;
        [SerializeField] private Inventory inventory;

        private Dictionary<Item, ItemButton> _itemStacks;

        public bool IsActive { get; private set; }

        private void Awake()
        {
            _itemStacks = new Dictionary<Item, ItemButton>();
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
            gameObject.SetActive(IsActive);
        }

        /// <summary>
        /// Adds an item to the inventory UI or increases the stack size in case it already exists
        /// </summary>
        /// <param name="item">The item to add to the UI</param>
        private void AddItem(Item item)
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
        private void RemoveItem(Item item)
        {
            if (!_itemStacks.ContainsKey(item)) return;
            _itemStacks[item].Count -= 1;
            if (_itemStacks[item].Count <= 0)
                _itemStacks.Remove(item);
        }
    }
}
