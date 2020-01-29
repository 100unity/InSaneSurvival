using System.Collections.Generic;
using Managers;
using UnityEngine;
using Utils.ElementInteraction;

namespace Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Tooltip("The grid in which the ItemButtons will be spawned")] [SerializeField]
        private InventoryGridLayout itemGrid;

        [Tooltip("The ItemButtonPrefab for spawning")] [SerializeField]
        private ItemButton itemButtonPrefab;

        [Tooltip("Used for showing and hiding the inventory")] [SerializeField]
        private GameObject inventoryUIContent;

        [Header("Inventory-Grid")]
        [Tooltip("The prefab for an inventory slot. Will be used to fill the inventory with empty slots.")]
        [SerializeField]
        private GameObject inventorySlotPrefab;

        [Tooltip("Defines the number of empty inventory slots the player has in his inventory")] [SerializeField]
        private int numberOfInventorySlots;

        /// <summary>
        /// All items and the corresponding ItemButtons.
        /// </summary>
        private readonly List<ItemButton> _items = new List<ItemButton>();

        /// <summary>
        /// Whether the inventory is currently showing.
        /// </summary>
        private bool _isActive;

        /// <summary>
        /// Fills the inventory with empty inventory slots.
        /// </summary>
        private void Awake() => itemGrid.FillInventoryWithEmptySlots(inventorySlotPrefab, numberOfInventorySlots);

        /// <summary>
        /// Subscribes to all item events.
        /// </summary>
        private void OnEnable()
        {
            InventoryManager.Instance.ItemHandler.ItemsUpdated += ItemsUpdated;
            InventoryManager.Instance.ItemHandler.ItemAdded += ItemAdded;
            InventoryManager.Instance.ItemHandler.ItemRemoved += ItemRemoved;
            InventoryManager.Instance.OnEquipableRemoved += RemoveItemButton;
            Swappable.OnAfterSwap += SwapCompleted;
        }

        /// <summary>
        /// Un-Subscribes to all item events.
        /// </summary>
        private void OnDisable()
        {
            InventoryManager.Instance.ItemHandler.ItemsUpdated += ItemsUpdated;
            InventoryManager.Instance.ItemHandler.ItemAdded -= ItemAdded;
            InventoryManager.Instance.ItemHandler.ItemRemoved -= ItemRemoved;
            InventoryManager.Instance.OnEquipableRemoved -= RemoveItemButton;
            Swappable.OnAfterSwap -= SwapCompleted;
        }

        /// <summary>
        /// Updated all item button and checks if ones amount is zero
        /// </summary>
        private void ItemsUpdated()
        {
            UpdateItemButtons();
            UpdateNotFullItemStacks();
        }

        /// <summary>
        /// Adds an item to the list and updates <see cref="InventoryManager.NotFullItemStacks"/>
        /// </summary>
        /// <param name="item">The new item to be added</param>
        private void ItemAdded(Item item)
        {
            AddItem(item);
            UpdateNotFullItemStacks();
        }

        /// <summary>
        /// Removes an item from the list and updates <see cref="InventoryManager.NotFullItemStacks"/>
        /// </summary>
        /// <param name="item">The item to be removed</param>
        private void ItemRemoved(Item item)
        {
            RemoveItem(item);
            UpdateNotFullItemStacks();
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
        private void AddItem(Item item)
        {
            ItemButton itemButtonInList = _items.Find(button => button.Item == item && button.CanAddOne());

            // Add to first one found
            if (itemButtonInList != default(ItemButton))
            {
                itemButtonInList.Count++;
                return;
            }

            // Only full stacks or no stack yet
            ItemButton itemButton = Instantiate(itemButtonPrefab, itemGrid.transform);
            itemGrid.SetPositionInGrid(itemButton.gameObject);
            itemButton.Init(item);
            _items.Add(itemButton);
        }

        /// <summary>
        /// Removes an item from the inventory UI or decreases the stack size by one.
        /// Deletes a item button if the count is 0.
        /// </summary>
        /// <param name="item">The item to remove from the UI</param>
        private void RemoveItem(Item item)
        {
            ItemButton itemButtonInList = _items.Find(button => button.Item == item);
            if (itemButtonInList == default(ItemButton)) return;
            itemButtonInList.Count--;

            // Skip deletion if amount is left
            if (itemButtonInList.Count > 0) return;

            RemoveItemButton(itemButtonInList);
        }

        /// <summary>
        /// Removes an ItemButton from the UI and adds an empty space.
        /// </summary>
        /// <param name="itemButton"></param>
        private void RemoveItemButton(ItemButton itemButton)
        {
            itemGrid.AddEmptySlotAt(itemButton.transform.GetSiblingIndex());
            DestroyImmediate(itemButton.gameObject);
            _items.Remove(itemButton);
        }

        /// <summary>
        /// Goes through all ItemButtons and deletes all buttons that have a Count of 0.
        /// </summary>
        private void UpdateItemButtons()
        {
            List<ItemButton> itemButtonList = _items.FindAll(button => button.Count <= 0);
            foreach (ItemButton currentButton in itemButtonList)
            {
                itemGrid.AddEmptySlotAt(currentButton.transform.GetSiblingIndex());
                DestroyImmediate(currentButton.gameObject);
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

        /// <summary>
        /// Updates <see cref="InventoryManager.NotFullItemStacks"/>.
        /// </summary>
        private void UpdateNotFullItemStacks()
        {
            HashSet<Item> notFullStacks = new HashSet<Item>();
            _items.FindAll(itemButton => itemButton.CanAddOne())
                .ForEach(itemButton => notFullStacks.Add(itemButton.Item));
            InventoryManager.Instance.NotFullItemStacks = notFullStacks;
        }
    }
}