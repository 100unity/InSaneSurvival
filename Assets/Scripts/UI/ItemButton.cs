using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemButton : MonoBehaviour
    {
        [Tooltip("The icon of item")] [SerializeField]
        private Image icon;

        [Tooltip("The name of the item")] [SerializeField]
        private TextMeshProUGUI nameLabel;

        [Tooltip("The current quantity of this item")] [SerializeField]
        private TextMeshProUGUI countLabel;

        [Tooltip("The image for showing the user which item is equipped")] [SerializeField]
        private Image imgIsEquipped;

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
        /// The item which this ItemButton holds
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// The button used for the click-event
        /// </summary>
        private Button _button;

        /// <summary>
        /// The amount of this item
        /// </summary>
        private int _count;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        /// <summary>
        /// Sets all needed values for the UI element
        /// </summary>
        /// <param name="item">The item for this ItemButton</param>
        /// <param name="amount">The quantity to be displayed</param>
        public void Init(Item item, int amount)
        {
            Item = item;
            icon.sprite = Item.Icon;
            Count = amount;

            // TODO: Remove this if we have icons for all items
            nameLabel.SetText(Item.name);
        }

        public void ToggleIsEquipped(bool show)
        {
            imgIsEquipped.gameObject.SetActive(show);
        }

        /// <summary>
        /// If the button is clicked, the item saved in the _item field is used.
        /// Consumables are going to be removed from the inventory and equipable items will be equipped un-equipped.
        /// <para>Also shows/hides the "isEquipped" image.</para>
        /// </summary>
        private void OnClick() => Item.Use();
    }
}