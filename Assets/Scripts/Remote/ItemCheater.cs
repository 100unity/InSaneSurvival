using Inventory;
using Managers;
using UnityEngine;

namespace Remote
{
    public class ItemCheater : MonoBehaviour
    {
        [SerializeField] private Item wood;
        [SerializeField] private Item stone;
        [SerializeField] private Item iron;
        [SerializeField] private Item wool;

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
                case "IRON":
                    AddIron();
                    break;
                case "WOOL":
                    AddWool();
                    break;
                case "LOOTGOBLIN":
                    AddEverything();
                    break;
            }
        }

        private void AddEverything()
        {
            for (int i = 0; i < 20; i++)
            {
                _inventoryManager.AddItem(wool);
                _inventoryManager.AddItem(iron);
                _inventoryManager.AddItem(wood);
                _inventoryManager.AddItem(stone);
            }
        }
        
        private void AddWool()
        {
            for (int i = 0; i < 10; i++)
            {
                _inventoryManager.AddItem(wool);
            }
        }

        private void AddIron()
        {
            for (int i = 0; i < 10; i++)
            {
                _inventoryManager.AddItem(iron);
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