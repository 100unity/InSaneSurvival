using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] public Item item; //reference for items

    [SerializeField] public Inventory inventory; // reference for the inventory

    // Unity Events
    private void Start()
    {
        inventory = GameObject.Find("Player").GetComponentInChildren<Inventory>(); //auto assign the Inventory to the component
    }

    /// <summary>
    /// New function that override the Interact function from Interactable Class
    /// </summary>
    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    /// <summary>
    /// Function for picking up item, add to inventory and then make item disappeared
    /// </summary>
    public void PickUp()
    {
        //Add to inventory
        inventory.Add(item); 

        //Destroy object
        Destroy(gameObject); 
    }
}
