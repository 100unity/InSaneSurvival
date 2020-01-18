using Constants;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Crafting
{
    public class CraftingRecipeUI : MonoBehaviour
    {
        [Tooltip("The image of this recipe. Represents the crafted item.")] [SerializeField]
        private Image imgCraftItem;

        [Tooltip("The vertical layout group where the resources will be ordered in")] [SerializeField]
        private LayoutGroup recipeResourceList;

        [Tooltip("The button for crafting the item")] [SerializeField]
        private Button craftButton;

        [Tooltip("The crafting recipe resource prefab")] [SerializeField]
        private CraftingRecipeResource craftingRecipeResourcePrefab;

        /// <summary>
        /// The recipe this CraftingRecipeUI represents.
        /// </summary>
        public CraftingRecipe Recipe => _recipe;

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

            if (_recipe.CreatedItem.item.Icon)
                imgCraftItem.sprite = _recipe.CreatedItem.item.Icon;

            foreach (ItemResourceData resourceData in _recipe.NeededItems)
                Instantiate(craftingRecipeResourcePrefab, recipeResourceList.transform)
                    .InitResource(resourceData.item.name, resourceData.amount, resourceData.item.Icon);
            craftButton.onClick.RemoveAllListeners();
            craftButton.onClick.AddListener(() => _recipe.Craft());

            InventoryManager.Instance.ItemHandler.ItemsUpdated += OnItemUpdate;
        }

        /// <summary>
        /// See <see cref="OnItemUpdate"/>
        /// </summary>
        public void UpdateRecipe() => OnItemUpdate();

        /// <summary>
        /// Checks if this recipe can be crafted. If so makes it white, else red.
        /// </summary>
        private void OnItemUpdate() =>
            SetCanCraft(_recipe.CanCraft());

        /// <summary>
        /// Sets the color of the image to visually show if this recipe can be crafted
        /// </summary>
        private void SetCanCraft(bool canCraft) =>
            imgCraftItem.color = canCraft ? Consts.Colors.White : Color.black;
    }
}