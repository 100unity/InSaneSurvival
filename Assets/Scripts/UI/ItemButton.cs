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
        
        
        private Item _item;

        private InventoryController _inventory;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
            }
        }

        public InventoryController Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
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
            if (_item.Use()) 
            {
                if (_item is Consumable) _inventory.Remove(_item);
                //else if (item is Equipable) 
            } 
        }
    }
}