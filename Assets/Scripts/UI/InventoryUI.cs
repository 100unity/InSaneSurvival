using System.Collections.Generic;
using Inventory;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils.ElementInteraction;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Tooltip("The grid in which the ItemButtons will be spawned")] [SerializeField]
        private GridLayout itemGrid;

        [Tooltip("The ItemButtonPrefab for spawning")] [SerializeField]
        private ItemButton itemButtonPrefab;

        [Tooltip("Used for showing and hiding the inventory")] [SerializeField]
        private GameObject inventoryUIContent;

        /// <summary>
        /// All items and the corresponding ItemButtons
        /// </summary>
        private readonly List<ItemButton> _items = new List<ItemButton>();

        /// <summary>
        /// Whether the inventory is currently showing
        /// </summary>
        private bool _isActive;

        private void OnEnable()
        {
            InventoryManager.Instance.ItemHandler.ItemsUpdated += ItemsUpdated;
            Swappable.OnAfterSwap += SwapCompleted;
        }

        private void OnDisable()
        {
            InventoryManager.Instance.ItemHandler.ItemsUpdated -= ItemsUpdated;
            Swappable.OnAfterSwap -= SwapCompleted;
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
                ItemButton itemButtonInList = _items.Find(button => button.Item == item && button.CanAddOne());
                // Only full stacks or no stack yet
                if (itemButtonInList == default(ItemButton))
                {
                    ItemButton itemButton = Instantiate(itemButtonPrefab, itemGrid.transform);
                    itemButton.Init(item, amount);
                    _items.Add(itemButton);
                }
                else // Add to first one found
                    itemButtonInList.Count++;
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
                ItemButton itemButtonInList = _items.Find(button => button.Item == item);
                if (itemButtonInList == default(ItemButton)) return;
                itemButtonInList.Count--;
                if (itemButtonInList.Count > 0) continue;
                Destroy(itemButtonInList.gameObject);
                _items.Remove(itemButtonInList);
            }
        }

        /// <summary>
        /// Goes through all ItemButtons and deletes all buttons that have a Count of 0.
        /// </summary>
        private void UpdateItemButtons()
        {
            List<ItemButton> itemButtonInList = _items.FindAll(button => button.Count <= 0);
            foreach (ItemButton currentButton in itemButtonInList)
            {
                Destroy(currentButton.gameObject);
                _items.Remove(currentButton);
            }
        }

        /// <summary>
        /// Switches the elements in the Grid to make sure that they are ordered and displayed correctly
        /// </summary>
        /// <param name="swappable">The first swappable</param>
        /// <param name="otherSwappable">The second swappable</param>
        private void SwapCompleted(Swappable swappable, Swappable otherSwappable) =>
            itemGrid.SwitchElements(swappable.Parent, otherSwappable.Parent);
    }
}