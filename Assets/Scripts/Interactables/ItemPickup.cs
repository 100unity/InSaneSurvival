using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace Inventory
{
    public class ItemPickup : Interactable
    {
        [SerializeField] private Item item; //reference for items
        [SerializeField] private InventoryController inventory; //reference for inventory - have to be manually assigned

        /// <summary>
        /// New function that override the Interact function from Interactable Class
        /// </summary>
        public override void Interact()
        {
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
}

