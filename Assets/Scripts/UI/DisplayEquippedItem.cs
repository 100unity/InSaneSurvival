using System;
using Managers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
   public class DisplayEquippedItem : MonoBehaviour
   {
      [SerializeField][Tooltip("Image component that holds the equipped item")]
      private Image equippedItemSlot;
      
      [SerializeField][Tooltip("Game object that displays the Item")]
      private GameObject equippedItemDispplay;
      
      private void Update()
      {
         UpdateEquippedSlot();
      }

      private void UpdateEquippedSlot()
      {
         Sprite currentlyEquipped = InventoryManager.Instance.GetCurrentlyEquipped().Icon;

         if (currentlyEquipped != null)
         {
            equippedItemDispplay.SetActive(true);
            equippedItemSlot.sprite = currentlyEquipped;
         }
         else
         {
            equippedItemDispplay.SetActive(false);
         }
         
      }
   }
}
