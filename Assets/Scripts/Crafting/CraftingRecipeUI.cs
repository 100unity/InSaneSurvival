using Constants;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
    public class CraftingRecipeUI : MonoBehaviour
    {
        [Tooltip("The name of this recipe")] [SerializeField]
        private TextMeshProUGUI txtTitle;

        [Tooltip("The vertical layout group where the resources will be ordered in")] [SerializeField]
        private VerticalLayoutGroup recipeResourceList;

        [Tooltip("The background image to show if the recipe can be crafted")] [SerializeField]
        private Image imgBackground;

        [Tooltip("The button for crafting the item")] [SerializeField]
        private Button craftButton;

        [Tooltip("The crafting recipe resource prefab")] [SerializeField]
        private CraftingRecipeResource craftingRecipeResourcePrefab;

        /// <summary>
        /// The recipe of this CraftingRecipeUI element
        /// </summary>
        private CraftingRecipe _recipe;

        private void Start() => OnItemUpdate();

        /// <summary>
        /// Instantiates all crafting recipe resources for the given recipe
        /// </summary>
        /// <param name="recipe">The recipe of this crafting recipe UI</param>
        public void InitRecipe(CraftingRecipe recipe)
        {
            _recipe = recipe;

            txtTitle.SetText(_recipe.CreatedItemName);
            foreach (CraftingRecipe.ResourceData resourceData in _recipe.NeededItems)
                Instantiate(craftingRecipeResourcePrefab, recipeResourceList.transform)
                    .InitResource(resourceData.item.name, resourceData.amount, resourceData.item.Icon);
            craftButton.onClick.RemoveAllListeners();
            craftButton.onClick.AddListener(() => _recipe.Craft(InventoryManager.Instance.ItemHandler));

            InventoryManager.Instance.ItemHandler.ItemsUpdated += (item, amount) => OnItemUpdate();
        }

        /// <summary>
        /// Invokes <see cref="SetCanCraft"/>
        /// </summary>
        private void OnItemUpdate() =>
            SetCanCraft(_recipe.CanCraft(InventoryManager.Instance.ItemHandler));

        /// <summary>
        /// Sets the color of the image to visually show if this recipe can be crafted
        /// </summary>
        private void SetCanCraft(bool canCraft) =>
            imgBackground.color = canCraft ? Consts.Colors.White : Consts.Colors.Red;
    }
}