using Inventory;
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
    public class ItemHoverTooltip : HoverTooltip
    {
        [Header("UIItemTooltip")] [Tooltip("Used for getting the item-information")] [SerializeField]
        private ItemButton itemButton;

        [Tooltip("Used for disabling the tooltip on drag")] [SerializeField]
        private Draggable draggable;

        [Tooltip("The title text component")] [SerializeField]
        protected TextMeshProUGUI txtTitle;

        [Tooltip("The description text component")] [SerializeField]
        protected TextMeshProUGUI txtDescription;

        [Tooltip("The condition text component. Only visible for equipable items")] [SerializeField]
        protected TextMeshProUGUI txtCondition;

        /// <summary>
        /// Defines the different states that will be shown respectively to the current uses of an equipable.
        /// </summary>
        private static readonly string[] ConditionTexts = new[] {"Great", "Good", "Used", "Breaking"};

        /// <summary>
        /// Wait until the item is set and set the texts.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            CoroutineManager.Instance.WaitUntil(() => itemButton.Item != null, () =>
            {
                txtTitle.SetText(itemButton.Item.ItemName);
                txtDescription.SetText(itemButton.Item.Description);
                // If the item is an equipable, update the condition.
                if (itemButton.Item is Equipable equipable)
                    equipable.OnUsesChange += UsesChanged;
                else
                    txtCondition.gameObject.SetActive(false);
            });
        }

        private void OnEnable()
        {
            draggable.OnBeginDragging += Deactivate;
            draggable.OnEndDragging += Activate;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
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

        /// <summary>
        /// Updates the condition using the texts from <see cref="ConditionTexts"/>.
        /// </summary>
        /// <param name="currentUses">The current amount of uses of the equipable</param>
        /// <param name="maxUses">The max amount of uses of the equipable</param>
        private void UsesChanged(int currentUses, int maxUses)
        {
            float ratio = currentUses / (float) maxUses;
            if (ratio >= 1)
                return;
            string newCondition = ConditionTexts[(int) (ratio * ConditionTexts.Length)];
            txtCondition.SetText($"Condition: {newCondition}");
        }
    }
}