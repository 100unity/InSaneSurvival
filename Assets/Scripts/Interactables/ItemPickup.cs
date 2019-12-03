using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] public Item item; //reference for items

    [SerializeField] public Inventory inventory; // reference for the inventory

<<<<<<< Updated upstream
=======
    // Unity Events
>>>>>>> Stashed changes
    private void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>(); //auto assign the Inventory to the component
    }

<<<<<<< Updated upstream
=======
    /// <summary>
    /// New function that override the Interact function from Interactable Class
    /// </summary>
>>>>>>> Stashed changes
    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

<<<<<<< Updated upstream
    void PickUp()
    {
        //Pick up item
        inventory.Add(item); //Add to inventory   
        Destroy(gameObject); //Destroy object
=======
    /// <summary>
    /// Function for picking up item, add to inventory and then make item disappeared
    /// </summary>
    public void PickUp()
    {
        //Add to inventory

        //inventory.Add(item); 

        //Destroy object
        Destroy(gameObject); 
>>>>>>> Stashed changes
    }
}
