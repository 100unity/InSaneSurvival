using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    /// <summary>
    /// Allows the usage of slots for an inventory.
    /// </summary>
    public class InventoryGridLayout : GridLayoutGroup
    {
        /// <summary>
        /// Used for instantiating new slots when an item gets removed and a slot is free.
        /// </summary>
        private GameObject _inventorySlotPrefab;

        /// <summary>
        /// All empty slots and their position in the hierarchy. Used to find the first empty one.
        /// </summary>
        private SortedDictionary<int, GameObject> _emptyItemIndexes = new SortedDictionary<int, GameObject>();

        /// <summary>
        /// Number of empty slots
        /// </summary>
        private int EmptySlots => _emptyItemIndexes.Count;

        /// <summary>
        /// Fills the inventory with empty slots
        /// </summary>
        /// <param name="inventorySlotPrefab">The prefab that represents an empty slot</param>
        /// <param name="numberOfInventorySlots">The number of empty slots the inventory has</param>
        public void FillInventoryWithEmptySlots(GameObject inventorySlotPrefab, int numberOfInventorySlots)
        {
            _inventorySlotPrefab = inventorySlotPrefab;
            float slotsToAdd = numberOfInventorySlots - transform.childCount;
            for (int i = 0; i < slotsToAdd; i++)
            {
                GameObject slot = Instantiate(inventorySlotPrefab, transform);
                _emptyItemIndexes.Add(transform.childCount - 1, slot);
            }

            UpdateEmptySlots();
        }

        /// <summary>
        /// Switches the given two elements in the hierarchy of the grid.
        /// </summary>
        /// <param name="first">The first element to be swapped with the second</param>
        /// <param name="second">The second element to be swapped with the first</param>
        public void SwitchElements(GameObject first, GameObject second)
        {
            int firstPos = first.transform.GetSiblingIndex();
            int secondPos = second.transform.GetSiblingIndex();

            first.transform.SetSiblingIndex(secondPos);
            second.transform.SetSiblingIndex(firstPos);

            UpdateIndexes();
        }

        /// <summary>
        /// Sets the position of the given object in the hierarchy by replacing the first empty slot with it.
        /// </summary>
        /// <param name="item">The newly added item</param>
        public void SetPositionInGrid(GameObject item)
        {
            int freeSlotIndex = _emptyItemIndexes.First().Value.transform.GetSiblingIndex();

            // Remove empty slot
            KeyValuePair<int, GameObject> toRemove = _emptyItemIndexes.First();
            _emptyItemIndexes.Remove(toRemove.Key);
            Destroy(toRemove.Value.gameObject);

            // Set position in grid
            item.transform.parent = transform;
            item.transform.SetSiblingIndex(freeSlotIndex);
            UpdateEmptySlots();
        }

        /// <summary>
        /// Adds an empty slot at the given index. Should be used when an item gets removed.
        /// </summary>
        /// <param name="index">The index of the removed item</param>
        public void AddEmptySlotAt(int index)
        {
            GameObject slot = Instantiate(_inventorySlotPrefab, transform);
            slot.transform.SetSiblingIndex(index);
            _emptyItemIndexes.Add(index, slot);
            UpdateEmptySlots();
        }

        /// <summary>
        /// Updates all indexes in the SortedDictionary.
        /// </summary>
        private void UpdateIndexes()
        {
            SortedDictionary<int, GameObject> newDictionary = new SortedDictionary<int, GameObject>();
            foreach (GameObject slot in _emptyItemIndexes.Values)
                newDictionary.Add(slot.transform.GetSiblingIndex(), slot);

            _emptyItemIndexes = newDictionary;
        }

        /// <summary>
        /// Updates whether there are empty slots in the <see cref="InventoryManager"/>.
        /// </summary>
        private void UpdateEmptySlots() => InventoryManager.Instance.HasEmptySlots = EmptySlots > 0;
    }
}