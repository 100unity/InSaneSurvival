using System;
using System.Text.RegularExpressions;
using UnityEngine;
using Utils;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Item(General)")]
    [Serializable]
    public class Item : ScriptableObject
    {
        [Header("Item base")]
        [Tooltip("The sprite that will be shown everywhere this item is displayed")]
        [SerializeField]
        private Sprite icon;

        [Tooltip("Defines the maximum stack size of this item")] [SerializeField]
        private int maxStackSize;

        [Tooltip("[CAN BE UNDEFINED]\nUsed to define a different name for this item (than its object-name)")]
        [SerializeField]
        private string itemName;

        [Tooltip("Small/Medium/Long description of this item")] [TextArea] [SerializeField]
        private string description;

        public Sprite Icon => icon;
        public int MaxStackSize => maxStackSize;
        public string ItemName => itemName.IsNullOrEmpty() ? ItemNameWithSpaces : itemName;
        public string Description => description;

        /// <summary>
        /// See: https://stackoverflow.com/a/155340
        /// </summary>
        private string ItemNameWithSpaces => Regex.Replace(name, "(\\B[A-Z])", " $1");

        public virtual bool Use()
        {
            Debug.Log("The item " + name + " can not be used.");
            return false;
        }
    }
}