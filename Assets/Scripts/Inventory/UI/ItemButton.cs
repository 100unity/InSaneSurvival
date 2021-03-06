using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.ElementInteraction;

namespace Inventory.UI
{
    public class ItemButton : MonoBehaviour
    {
        [Tooltip("The button for using or dragging the item")] [SerializeField]
        private Button button;

        [Tooltip("The icon of item")] [SerializeField]
        private Image icon;

        [Tooltip("The current quantity of this item")] [SerializeField]
        private TextMeshProUGUI countLabel;

        [Tooltip("The image for showing the user which item is equipped")] [SerializeField]
        private Image imgIsEquipped;

        [Header("Stacking")] [Tooltip("The swappable component used for the stacking")] [SerializeField]
        private Swappable swappable;

        /// <summary>
        /// Event that will be triggered whenever the amount of uses is changed.
        /// </summary>
        public event UsesStatus OnUsesChange;

        public delegate void UsesStatus(int currentUses, int maxUses);

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
        /// The amount of this item
        /// </summary>
        private int _count;

        /// <summary>
        /// If this is an equipable, this is not null.
        /// </summary>
        private Equipable _equipable;

        /// <summary>
        /// The currently amount of uses.
        /// </summary>
        private int _currentUses;

        /// <summary>
        /// Checks if the item is visually equipped
        /// </summary>
        private bool IsEquipped => imgIsEquipped.gameObject.activeSelf;

        private void Awake() => button.onClick.AddListener(OnClick);

        private void OnEnable() => swappable.OnBeforeSwap += Stack;

        private void OnDisable() => swappable.OnBeforeSwap -= Stack;

        /// <summary>
        /// Sets all needed values for the UI element
        /// </summary>
        /// <param name="item">The item for this ItemButton</param>
        public void Init(Item item)
        {
            Item = item;
            icon.sprite = Item.Icon;
            Count = 1;
            if (item is Equipable equipable)
                _equipable = equipable;
        }

        /// <summary>
        /// Increases the amount of uses of this item.
        /// </summary>
        /// <param name="amount">The increase of uses. By default 1</param>
        public void IncreaseUses(int amount = 1)
        {
            if (_equipable == null)
                return;
            _currentUses += amount;
            if (_currentUses >= _equipable.MaxUses)
                InventoryManager.Instance.RemoveItemButton(this);
            else
                OnUsesChange?.Invoke(_currentUses, _equipable.MaxUses);
        }

        /// <summary>
        /// Checks if there is space left on the stack
        /// </summary>
        /// <returns>Whether this stack is full or not</returns>
        public bool CanAddOne() => GetFreeSpace() > 0;

        /// <summary>
        /// Visually unequips the item
        /// </summary>
        public void Unequip() => imgIsEquipped.gameObject.SetActive(false);

        /// <summary>
        /// Shows/Hides the "isEquipped" status and updates the equipable in the <see cref="InventoryManager"/>
        /// </summary>
        private void ToggleIsEquipped()
        {
            imgIsEquipped.gameObject.SetActive(!IsEquipped);
            InventoryManager.Instance.SetCurrentlyEquipped(IsEquipped ? this : null);
        }

        /// <summary>
        /// If the button is clicked, the item saved in the _item field is used.
        /// Consumables are going to be removed from the inventory and equipable items will be equipped un-equipped.
        /// <para>Also shows/hides the "isEquipped" image.</para>
        /// </summary>
        private void OnClick()
        {
            Item.Use();
            if (Item is Equipable)
                ToggleIsEquipped();
            else if (Item is Consumable consumable)
                AudioManager.Instance.Play(consumable.SoundOnUse);
        }

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
            if (otherSwappable.TryGetComponent(out Trash _))
            {
                InventoryManager.Instance.RemoveItemButton(this);
                return false;
            }

            // If full stack or not an itemButton, skip
            if (!otherSwappable.TryGetComponent(out Stackable otherStackable) ||
                !otherStackable.ItemButton.CanAddOne() || !CanAddOne())
                return true;
            // If not the same, skip
            if (Item != otherStackable.ItemButton.Item)
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
            swappable.ResetPosition();
            return false;
        }
    }
}