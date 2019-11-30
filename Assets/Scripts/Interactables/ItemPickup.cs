using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public Inventory inventory;

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    void PickUp()
    {
        //Pick up item
        inventory.Add(item);
        //Add to inventory
        Destroy(gameObject);
    }
}
