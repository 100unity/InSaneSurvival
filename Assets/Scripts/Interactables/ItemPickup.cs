using Inventory;
using Managers;
using UnityEngine;

namespace Interactables
{
    public class ItemPickup : Interactable
    {
        [SerializeField] private Item item; //reference for items

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
        private void PickUp()
        {
            //Add to inventory
            if (InventoryManager.Instance.AddItem(item))
                Destroy(gameObject);
        }
    }
}