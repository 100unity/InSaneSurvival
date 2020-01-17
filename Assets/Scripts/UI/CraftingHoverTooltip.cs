using Crafting;
using Managers;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class CraftingHoverTooltip : HoverTooltip
    {
        [Header("CraftingTooltip")] [Tooltip("")] [SerializeField]
        private CraftingRecipeUI craftingRecipe;

        [Tooltip("")] [SerializeField] private TextMeshProUGUI txtTitle;

        [Tooltip("")] [SerializeField] private Transform resourceParent;

        [SerializeField] private CraftingHoverTooltipResource craftingHoverTooltipResourcePrefab;


        protected override void Awake()
        {
            base.Awake();
            CoroutineManager.Instance.WaitUntil(() => craftingRecipe.Recipe != null, Setup);
        }

        private void Setup()
        {
            txtTitle.SetText(craftingRecipe.Recipe.CreatedItemName);
            foreach (ItemResourceData resourceData in craftingRecipe.Recipe.NeededItems)
                Instantiate(craftingHoverTooltipResourcePrefab, resourceParent).Init(resourceData);
        }
    }
}