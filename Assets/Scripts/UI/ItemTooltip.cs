using Inventory.UI;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Utils.ElementInteraction;

namespace UI
{
    /// <summary>
    /// Extends <see cref="HoverTooltip"/> by adding disabling-functionality on dragging and setting the texts of the
    /// tooltip.
    /// </summary>
    public class ItemTooltip : HoverTooltip
    {
        [Header("UIItemTooltip")] [Tooltip("Used for getting the item-information")] [SerializeField]
        private ItemButton itemButton;

        [Tooltip("Used for disabling the tooltip on drag")] [SerializeField]
        private Draggable draggable;

        [Tooltip("The title text component")] [SerializeField]
        protected TextMeshProUGUI txtTitle;

        [Tooltip("The description text component")] [SerializeField]
        protected TextMeshProUGUI txtDescription;

        [Tooltip("The effect text component")] [SerializeField]
        protected TextMeshProUGUI txtEffect;

        /// <summary>
        /// Wait until the item is set and set the texts.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            CoroutineManager.Instance.WaitUntil(() => itemButton.Item != null, () =>
            {
                txtTitle.SetText(itemButton.Item.name);
                txtDescription.SetText(itemButton.Item.Description);
                txtEffect.SetText(itemButton.Item.EffectText);
            });
        }

        private void OnEnable()
        {
            draggable.OnBeginDragging += Deactivate;
            draggable.OnEndDragging += Activate;
        }

        private void OnDisable()
        {
            draggable.OnBeginDragging -= Deactivate;
            draggable.OnEndDragging -= Activate;
        }

        /// <summary>
        /// Re-enables the tooltip.
        /// </summary>
        private void Activate(PointerEventData eventData)
        {
            Time = float.MaxValue;
            IsDeactivated = false;
        }

        /// <summary>
        /// Disables the tooltip.
        /// </summary>
        private void Deactivate(PointerEventData eventData)
        {
            IsDeactivated = true;
            IsShowing = false;
        }
    }
}