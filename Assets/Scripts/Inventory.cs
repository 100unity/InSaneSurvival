using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<AUsable> items = new List<AUsable>();
    
    public delegate void InventoryUpdate(AUsable item);
    public event InventoryUpdate OnItemAdded;
    public event InventoryUpdate OnItemRemoved;

    /// <summary>
    /// Adds an item to the player's inventory
    /// </summary>
    /// <param name="item">The item to add</param>
    public void Add(AUsable item)
    {
        items.Add(item);
        OnItemAdded?.Invoke(item);
    }

    /// <summary>
    /// Removes an item from the player's inventory
    /// </summary>
    /// <param name="item"></param>
    public void Remove(AUsable item)
    {
        items.Remove(item);
        OnItemRemoved?.Invoke(item);
    }
}
