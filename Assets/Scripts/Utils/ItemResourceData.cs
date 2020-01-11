using System;
using Inventory;

namespace Utils
{
    /// <summary>
    /// Small struct for storing the item and the amount of the item as an "ingredient" for the recipe.
    /// </summary>
    [Serializable]
    public struct ItemResourceData
    {
        public Item item;
        public int amount;
    }
}