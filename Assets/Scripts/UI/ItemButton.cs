using System;
using Interfaces;
using Inventory;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.ElementInteraction;

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

        [Header("Stacking")] [Tooltip("The swappable component used for the stacking")] [SerializeField]
        private Swappable swappable;

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

        private void OnEnable() => swappable.OnSwap += Stack;

        private void OnDisable() => swappable.OnSwap -= Stack;

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

        /// <summary>
        /// Checks if there is space left on the stack
        /// </summary>
        /// <returns>Whether this stack is full or not</returns>
        public bool CanAddOne() => GetFreeSpace() > 0;

        /// <summary>
        /// Shows/Hides the "isEquipped" status
        /// </summary>
        /// <param name="show">Whether to show or hide it</param>
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
        
        /// <summary>
        /// Gets the free space of the stack
        /// </summary>
        /// <returns>Free space as an integer</returns>
        private int GetFreeSpace() => Item.MaxStackSize - _count;
        
        /// <summary>
        /// Tries to stack the current item button into the other.
        /// If it is the wrong item or one of the two stacks is already full, it will do nothing
        /// </summary>
        /// <param name="otherSwappable">The other element the user dragged this element over</param>
        /// <returns>True: Swaps the elements, False: Stacks them</returns>
        private bool Stack(Swappable otherSwappable)
        {
            // If full stack or not a stackable, skip
            if (!otherSwappable.TryGetComponent(out Stackable otherStackable) ||
                !otherStackable.ItemButton.CanAddOne() || !CanAddOne())
                return true;
            // If not the same, skip
            if (!Item.Equals(otherStackable.ItemButton.Item))
                return true;

            int freeSpace = otherStackable.ItemButton.GetFreeSpace();

            // Move everything to the other stack
            if (freeSpace >= Count)
            {
                otherStackable.ItemButton.Count += Count;
                Count = 0;
                InventoryManager.Instance.RefreshItems();
                return false;
            }

            // Move as much as possible
            otherStackable.ItemButton.Count += freeSpace;
            Count -= freeSpace;
            return false;
        }
    }
}