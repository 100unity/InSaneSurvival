using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    
    public delegate void InventoryUpdate(Item item);
    public event InventoryUpdate OnItemAdded;
    public event InventoryUpdate OnItemRemoved;

    public void Add(Item item)
    {
        items.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        OnItemRemoved?.Invoke(item);
    }
}
