using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{

    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private List<Item> items = new List<Item>();

        public delegate void InventoryUpdate(Item item, int amount);

        public event InventoryUpdate OnItemAdded;
        public event InventoryUpdate OnItemRemoved;

        /// <summary>
        /// Adds an item to the player's inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="amount">The amount to be added</param>
        public void Add(Item item, int amount = 1)
        {
            items.Add(item);
            OnItemAdded?.Invoke(item, amount);
        }

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <param name="amount">The amount to be removed</param>
        public void Remove(Item item, int amount = 1)
        {
            items.Remove(item);
            OnItemRemoved?.Invoke(item, amount);
        }
    }
}
