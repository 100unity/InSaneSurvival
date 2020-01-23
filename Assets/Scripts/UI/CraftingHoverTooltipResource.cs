using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class CraftingHoverTooltipResource : MonoBehaviour
    {
        [SerializeField] private Image imgResource;
        [SerializeField] private TextMeshProUGUI txtResource;

        public void Init(ItemResourceData itemResourceData)
        {
            imgResource.sprite = itemResourceData.item.Icon;
            txtResource.SetText($"{itemResourceData.amount} x {itemResourceData.item.ItemName}");
        }
    }
}