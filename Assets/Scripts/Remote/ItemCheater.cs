using System;
using Inventory;
using Managers;
using UnityEngine;

namespace Remote
{
    public class ItemCheater : MonoBehaviour
    {
        [SerializeField] private Item wood;
        [SerializeField] private Item stone;

        private InventoryManager _inventoryManager;

        private void Start()
        {
            _inventoryManager = InventoryManager.Instance;
        }

        public void AddItems(string item)
        {
            switch (item)
            {
                case "WOOD":
                    AddWood();
                    break;
                case "STONE":
                    AddStone();
                    break;
            }
        }

        // adds 10 wood to the players inventory
        private void AddWood()
        {
            for (int i = 0; i < 10; i++)
            {
                _inventoryManager.AddItem(wood);
            }
        }

        // adds 10 stone to the players inventory
        private void AddStone()
        {
            for (int i = 0; i < 10; i++)
            {
                _inventoryManager.AddItem(stone);
            }
        }
    }
}