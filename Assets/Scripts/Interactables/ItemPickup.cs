using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] public Item item; //reference for items

    [SerializeField] public Inventory inventory; // reference for the inventory

    private void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>(); //auto assign the Inventory to the component
    }

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    void PickUp()
    {
        //Pick up item
        inventory.Add(item); //Add to inventory   
        Destroy(gameObject); //Destroy object
    }
}
