using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Item(General)")]
    [System.Serializable] public class Item : ScriptableObject
    {
        [Header("Item base")]
        [Tooltip("The sprite that will be shown everywhere this item is displayed")]
        [SerializeField]
        private Sprite icon;

        [Tooltip("Defines the maximum stack size of this item")] [SerializeField]
        private int maxStackSize;

        [Tooltip("[CAN BE UNDEFINED]\nUse this to add spaces to the name or other special stuff")] [SerializeField]
        private string itemName;

        [Tooltip("Small/Medium/Long description of this item")] [TextArea] [SerializeField]
        private string description;

        [Tooltip("All effects that this item has on the player or what the player can do with it")]
        [TextArea]
        [SerializeField]
        private string effectText;

        public Sprite Icon => icon;
        public int MaxStackSize => maxStackSize;
        public string ItemName => itemName;
        public string Description => description;
        public string EffectText => effectText;

        public virtual bool Use()
        {
            Debug.Log("The item " + name + " can not be used.");
            return false;
        }
    }
}