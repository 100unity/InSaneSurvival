using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject itemGrid;
        [SerializeField] private ItemButton itemButtonPrefab;
        [SerializeField] private Inventory inventory;

        private Dictionary<Item, ItemButton> _itemStacks;

        public bool IsActive { get; private set; }

        private void Awake()
        {
            _itemStacks = new Dictionary<Item, ItemButton>();
            
            inventory.OnItemAdded += AddItem;
            inventory.OnItemRemoved += RemoveItem;
        }

        public void ToggleInventory()
        {
            IsActive = !IsActive;
            gameObject.SetActive(IsActive);
        }

        private void AddItem(Item item)
        {
            if (_itemStacks.ContainsKey(item))
            {
                _itemStacks[item].Count += 1;
                return;
            }

            ItemButton itemButton = Instantiate(itemButtonPrefab, itemGrid.transform);
            itemButton.Icon.sprite = item.Icon;
            itemButton.NameLabel.text = item.name;
            itemButton.Count = 1;
            _itemStacks[item] = itemButton;
        }

        private void RemoveItem(Item item)
        {
            if (!_itemStacks.ContainsKey(item)) return;
            _itemStacks[item].Count -= 1;
            if (_itemStacks[item].Count <= 0)
                _itemStacks.Remove(item);
        }
    }
}
