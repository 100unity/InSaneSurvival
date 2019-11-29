using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private int maxStackSize;

    public Sprite Icon => icon;
    public int MaxStackSize => maxStackSize;

    public override bool Equals(object other)
    {
        return other is Item item && item.name == name;
    }
}
