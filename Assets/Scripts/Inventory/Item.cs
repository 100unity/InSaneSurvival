using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStackSize;
        public Sprite Icon => icon;
        public int MaxStackSize => maxStackSize;

        public abstract bool Use();
    }
}


