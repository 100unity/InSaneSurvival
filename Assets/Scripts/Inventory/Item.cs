using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Item(General)")][System.Serializable]
    public class Item : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStackSize;

        public Sprite Icon => icon;
        public int MaxStackSize => maxStackSize;

        public virtual bool Use()
        {
            Debug.Log("The item " + name + " can not be used.");
            return false;
        }
    }
}