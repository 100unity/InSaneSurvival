using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Item(General)")]
    public class Item : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStackSize;
        public Sprite Icon => icon;
        public int MaxStackSize => maxStackSize;

        public bool Use()
        {
            Debug.Log("The item " + name + " can not be used.");
            return false;
        }
    }
}


