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
        [Header("UIItemTooltip")] [SerializeField]
        private ItemButton itemButton;

        [SerializeField] private Draggable draggable;

        [SerializeField] protected TextMeshProUGUI txtTitle;

        [SerializeField] protected TextMeshProUGUI txtDescription;
        [SerializeField] protected TextMeshProUGUI txtEffect;

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

        private void Activate(PointerEventData eventData)
        {
            Time = float.MaxValue;
            IsDeactivated = false;
        }

        private void Deactivate(PointerEventData eventData)
        {
            IsDeactivated = true;
            IsShowing = false;
        }
    }
}