using System;
using Constants;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.ElementInteraction;

namespace Inventory
{
    /// <summary>
    /// Small script to define a trash for removing items from the inventory.
    /// </summary>
    [RequireComponent(typeof(Swappable))]
    public class Trash : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("Trash icon for coloring.")] [SerializeField]
        private Image imgTrashIcon;

        public void OnPointerEnter(PointerEventData eventData) => imgTrashIcon.color = Consts.Colors.Red;

        public void OnPointerExit(PointerEventData eventData) => imgTrashIcon.color = Consts.Colors.White;
    }
}