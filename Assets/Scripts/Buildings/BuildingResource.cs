using Constants;
using Inventory;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Buildings
{
    public class BuildingResource : MonoBehaviour
    {
        [SerializeField] private Image imgBackground;
        [SerializeField] private Image imgResourceIcon;
        [SerializeField] private TextMeshProUGUI txtResource;

        private ItemResourceData _resourceData;

        private void OnEnable() => InventoryManager.Instance.ItemHandler.ItemsUpdated += ItemsUpdated;

        private void OnDisable() => InventoryManager.Instance.ItemHandler.ItemsUpdated -= ItemsUpdated;

        public void Init(ItemResourceData resourceData)
        {
            imgResourceIcon.sprite = resourceData.item.Icon;
            txtResource.SetText($"{resourceData.item.name} x {resourceData.amount}");

            _resourceData = resourceData;
            Refresh();
        }

        private void ItemsUpdated(Item arg1, int arg2) => Refresh();

        private void Refresh()
        {
            imgBackground.color =
                InventoryManager.Instance.ItemHandler.ContainsItem(_resourceData.item, _resourceData.amount)
                    ? Consts.Colors.WhiteFaded
                    : Consts.Colors.RedFaded;
        }
    }
}