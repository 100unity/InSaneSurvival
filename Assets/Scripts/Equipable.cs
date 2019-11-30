using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipable", menuName = "Inventory/Equipable")]
public class Equipable : AUsable
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
