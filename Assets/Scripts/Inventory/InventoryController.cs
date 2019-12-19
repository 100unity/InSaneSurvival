using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour, IItemHandler
    {
        /// <summary>
        /// Every item the player currently has
        /// </summary>
        [SerializeField] private List<Item> items = new List<Item>();

        /// <inheritdoc cref="IItemHandler.ItemsUpdated"/>
        public event Action<Item, int> ItemsUpdated;

        /// <summary>
        /// Adds an item to the player's inventory and invokes <see cref="ItemsUpdated"/>.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="amount">The amount to be added</param>
        public void AddItem(Item item, int amount = 1)
        {
            for (int i = 0; i < amount; i++) items.Add(item);
            ItemsUpdated?.Invoke(item, amount);
        }

        /// <summary>
        /// Removes an item from the player's inventory and invokes <see cref="ItemsUpdated"/>.
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <param name="amount">The amount to be removed</param>
        public void RemoveItem(Item item, int amount = 1)
        {
            for (int i = 0; i < amount; i++) items.Remove(item);
            ItemsUpdated?.Invoke(item, -amount);
        }

        /// <inheritdoc cref="IItemHandler.ContainsItem"/>
        public bool ContainsItem(Item item, int amount = 1)
        {
            int currentAmount = items.Count(currentItem => currentItem == item);
            return currentAmount >= amount;
        }
    }
}