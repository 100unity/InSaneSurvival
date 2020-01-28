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

        public Item item1;
        public Item item2;
        public Item item3;
        public Item item42;
        public Item item323;
        public Item item423;
        private void Awake()
        {
            for(int i=0; i<15; i++)
            {
                AddItem(item1);
                AddItem(item2);
                AddItem(item3);
                AddItem(item42);
                AddItem(item323);
                AddItem(item423);
            }
        }

        /// <summary>
        /// Adds an item to the player's inventory and invokes <see cref="ItemsUpdated"/>.
        /// </summary>
        /// <param name="item">The item to add</param>
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
            foreach (var item in itemList)
            {
                AddItem(item);
            }
        }
    }
}