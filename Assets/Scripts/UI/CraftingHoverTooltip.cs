using Crafting;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class CraftingHoverTooltip : HoverTooltip
    {
        [Header("CraftingTooltip")]
        [Tooltip("The CraftingRecipeUI reference used for getting the recipe")]
        [SerializeField]
        private CraftingRecipeUI craftingRecipe;

        [Tooltip("Title-Text used for setting it to the recipes name")] [SerializeField]
        private TextMeshProUGUI txtTitle;

        [Tooltip("CarftingStation-Text used for displaying the needed crafting station")] [SerializeField]
        private TextMeshProUGUI txtCraftingStation;

        [Tooltip("The layoutGroup used for setting the correct parent")] [SerializeField]
        private LayoutGroup resourceParent;

        [SerializeField] private CraftingHoverTooltipResource craftingHoverTooltipResourcePrefab;

        /// <summary>
        /// Waits for the recipe to be loaded, then invokes <see cref="Setup"/>.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            CoroutineManager.Instance.WaitUntil(() => craftingRecipe.Recipe != null, Setup);
        }

        /// <summary>
        /// Sets the texts and images.
        /// </summary>
        private void Setup()
        {
            txtTitle.SetText(craftingRecipe.Recipe.CreatedItemName);
            if (craftingRecipe.Recipe.CraftingStation == CraftingManager.CraftingStation.None)
                txtCraftingStation.gameObject.SetActive(false);
            else
                txtCraftingStation.SetText(
                    $"Crafting Station: <color=red>{craftingRecipe.Recipe.CraftingStation.ToString()}</color>");
            foreach (ItemResourceData resourceData in craftingRecipe.Recipe.NeededItems)
                Instantiate(craftingHoverTooltipResourcePrefab, resourceParent.transform).Init(resourceData);
        }
    }
}