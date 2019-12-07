using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

[CreateAssetMenu(fileName = "New Equipable", menuName = "Inventory/Item/Equipable")]
public class Equipable : Item
{
    public override bool Use()
    {
        Debug.Log("Equipping the item " + name);
        return false;
    }
    public override bool Equals(object other)
    {
        return other is Equipable item && item.name == name;
    }
}
