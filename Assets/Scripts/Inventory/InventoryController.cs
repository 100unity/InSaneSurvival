using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{

    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private List<Item> items = new List<Item>();

        public delegate void InventoryUpdate(Item item);
        public event InventoryUpdate OnItemAdded;
        public event InventoryUpdate OnItemRemoved;

        /// <summary>
        /// Adds an item to the player's inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        public void Add(Item item)
        {
            items.Add(item);
            OnItemAdded?.Invoke(item);
        }

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Item item)
        {
            items.Remove(item);
            OnItemRemoved?.Invoke(item);
        }
    }
}
