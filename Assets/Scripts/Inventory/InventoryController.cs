using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Inventory
{

    public class InventoryController : MonoBehaviour, IItemHandler
    {
        [SerializeField] private List<Item> items = new List<Item>();
        
        public event Action<Item, int> ItemsUpdated;

        /// <summary>
        /// Adds an item to the player's inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="amount">The amount to be added</param>
        public void AddItem(Item item, int amount = 1)
        {
            items.Add(item);
            ItemsUpdated?.Invoke(item, amount);
        }

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <param name="amount">The amount to be removed</param>
        public void RemoveItem(Item item, int amount = 1)
        {
            items.Remove(item);
            ItemsUpdated?.Invoke(item, -amount);
        }

        /// <inheritdoc cref="IItemHandler.ContainsItemAmount"/>
        public bool ContainsItemAmount(Item item, int amount = 1)
        {
            int currentAmount = items.Count(currentItem => currentItem == item);
            return currentAmount >= amount;
        }
            
    }
}
