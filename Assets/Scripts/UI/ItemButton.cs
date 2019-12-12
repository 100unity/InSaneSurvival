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
        
        [HideInInspector]
        public Item _item;

        [HideInInspector]
        public InventoryController _inventory;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
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

        /// <summary>
        /// If the button is clicked, the item saved in the _item field is used. Consumables are going to be removed
        /// from the inventory.
        /// </summary>
        public void OnClick() 
        {
            if (_item is Consumable consumable)
            {
                if (consumable.Use())
                {
                    _inventory.Remove(_item);
                }
            }
            else if (_item is Equipable equipable)
            {
                equipable.Use();
            }
        }
    }
}