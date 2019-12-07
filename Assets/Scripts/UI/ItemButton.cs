using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameLabel;
        [SerializeField] private TextMeshProUGUI countLabel;

        private int _count;

        public Image Icon => icon;
        public TextMeshProUGUI NameLabel => nameLabel;
        
        
        private AUsable item;

        private InventoryController inventory;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        public AUsable Item
        {
            get => item;
            set
            {
                item = value;
            }
        }

        public InventoryController Inventory
        {
            get => inventory;
            set
            {
                inventory = value;
            }
        }

        /// <summary>
        /// The number of items on the item stack. Also updates the label in the inventory UI when changed.
        /// </summary>
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                countLabel.SetText(_count.ToString());
            }
        }

        public void OnClick() 
        {
            if (item.Use()) 
            {
                if (item is Consumable) inventory.Remove(item);
                //else if (item is Equipable) 
            } 
        }
    }
}