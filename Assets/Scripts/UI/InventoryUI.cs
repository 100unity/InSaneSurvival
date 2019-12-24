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
        private readonly List<KeyValuePair<Item, ItemButton>> _items = new List<KeyValuePair<Item, ItemButton>>();

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
            {
                ItemButton itemButton = _items.Find(pair => pair.Key == oldItem).Value;
                if (itemButton != null)
                    itemButton.ToggleIsEquipped(false);
            }

            if (newItem != null)
            {
                ItemButton itemButton = _items.Find(pair => pair.Key == newItem).Value;
                if (itemButton != null)
                    itemButton.ToggleIsEquipped(true);
            }
        }

        /// <summary>
        /// Checks if the amount is negative or positive and removes or adds respectively.
        /// If the amount is 0, it will check all item buttons instead.
        /// </summary>
        /// <param name="item">The item to be added/removed</param>
        /// <param name="amount">The amount to be added/removed</param>
        private void ItemsUpdated(Item item, int amount)
        {
            if (amount == 0)
            {
                UpdateItemButtons();
                return;
            }

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
        /// Adds an item to the inventory UI or increases the stack size in case it already exists.
        /// Creates a new item button if the previous ones are full.
        /// </summary>
        /// <param name="item">The item to add to the UI</param>
        /// <param name="amount">The amount to be added</param>
        private void AddItem(Item item, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                KeyValuePair<Item, ItemButton> itemPair =
                    _items.Find(pair => pair.Key == item && pair.Value.CanAddOne());
                // Only full stacks or no stack yet
                if (itemPair.Equals(default(KeyValuePair<Item, ItemButton>)))
                {
                    ItemButton itemButton = Instantiate(itemButtonPrefab, itemGrid.transform);
                    itemButton.Init(item, amount);
                    _items.Add(new KeyValuePair<Item, ItemButton>(item, itemButton));
                }
                else // Add to first one found
                    itemPair.Value.Count++;
            }
        }

        /// <summary>
        /// Removes an item from the inventory UI or decreases the stack size by one.
        /// Deletes a item button if the count is 0.
        /// </summary>
        /// <param name="item">The item to remove from the UI</param>
        /// <param name="amount">The amount to be removed</param>
        private void RemoveItem(Item item, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                KeyValuePair<Item, ItemButton> itemPair = _items.Find(pair => pair.Key == item);
                if (itemPair.Equals(default(KeyValuePair<Item, ItemButton>))) return;
                itemPair.Value.Count--;
                if (itemPair.Value.Count > 0) continue;
                Destroy(itemPair.Value.gameObject);
                _items.Remove(itemPair);
            }
        }

        private void UpdateItemButtons()
        {
            List<KeyValuePair<Item,ItemButton>> itemPairs = _items.FindAll(pair => pair.Value.Count <= 0);
            foreach (KeyValuePair<Item,ItemButton> itemPair in itemPairs)
            {
                Destroy(itemPair.Value.gameObject);
                _items.Remove(itemPair);
            }
        }
    }
}