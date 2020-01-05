using Constants;
using Crafting;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Buildings
{
    /// <summary>
    /// One of the needed resources for the building. Similar to <see cref="CraftingRecipeResource"/>
    /// </summary>
    public class BuildingResource : MonoBehaviour
    {
        [Tooltip("Background image. Used for showing the status")] [SerializeField]
        private Image imgBackground;

        [Tooltip("The icon of this resource")] [SerializeField]
        private Image imgResourceIcon;

        [Tooltip("The name and number of this resource")] [SerializeField]
        private TextMeshProUGUI txtResource;

        private ItemResourceData _resourceData;

        /// <summary>
        /// Sets all the needed data/texts/images.
        /// </summary>
        /// <param name="resourceData"></param>
        public void Init(ItemResourceData resourceData)
        {
            imgResourceIcon.sprite = resourceData.item.Icon;
            txtResource.SetText($"{resourceData.item.name} x {resourceData.amount}");

            _resourceData = resourceData;
            Refresh();
        }

        /// <summary>
        /// Checks if there are enough resources for this buildingResource. If so, changes the color.
        /// </summary>
        public void Refresh()
        {
            imgBackground.color =
                InventoryManager.Instance.ItemHandler.ContainsItem(_resourceData.item, _resourceData.amount)
                    ? Consts.Colors.WhiteFaded
                    : Consts.Colors.RedFaded;
        }
    }
}