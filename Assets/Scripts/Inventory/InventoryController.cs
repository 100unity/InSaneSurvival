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

        /// <inheritdoc cref="IItemHandler.ItemAdded"/>
        public event Action<Item> ItemAdded;

        /// <inheritdoc cref="IItemHandler.ItemRemoved"/>
        public event Action<Item> ItemRemoved;

        /// <inheritdoc cref="IItemHandler.ItemsUpdated"/>
        public event Action ItemsUpdated;

        /// <summary>
        /// Adds an item to the player's inventory and invokes <see cref="ItemsUpdated"/>.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="amount">The amount to be added</param>
        public void AddItem(Item item)
        {
            items.Add(item);
            ItemAdded?.Invoke(item);
            ItemsUpdated?.Invoke();
        }

        /// <summary>
        /// Removes an item from the player's inventory and invokes <see cref="ItemsUpdated"/>.
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <param name="amount">The amount to be removed</param>
        public void RemoveItem(Item item)
        {
            items.Remove(item);
            ItemRemoved?.Invoke(item);
            ItemsUpdated?.Invoke();
        }

        /// <summary>
        /// Refreshes the items by invoking the <see cref="ItemsUpdated"/> event with default and 0.
        /// </summary>
        public void RefreshItems() => ItemsUpdated?.Invoke();

        /// <inheritdoc cref="IItemHandler.ContainsItem"/>
        public bool ContainsItem(Item item, int amount = 1)
        {
            int currentAmount = items.Count(currentItem => currentItem == item);
            return currentAmount >= amount;
        }

        public List<Item> GetItems() => items;

        public void SetItems(List<Item> itemList)
        {
            items = itemList;
            foreach (var item in items)
            {
                AddItem(item);
            }
        }
    }
}