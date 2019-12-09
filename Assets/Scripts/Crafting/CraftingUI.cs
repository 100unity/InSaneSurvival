using Interfaces;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
    public class CraftingUI : MonoBehaviour
    {
        [Tooltip("The content of the crafting UI. Used for hiding it")] [SerializeField]
        private GameObject craftingUIContent;

        [Tooltip("The grid for the recipes to be displayed in")] [SerializeField]
        private GridLayoutGroup craftingRecipeGrid;

        [Tooltip("The crafting recipe UI prefab")] [SerializeField]
        private CraftingRecipeUI craftingRecipeUIPrefab;

        [Tooltip("The inventory of the player representing it's item handler")] [SerializeField]
        private InventoryUI inventoryUI;

        private bool _isShowing;

        /// <summary>
        /// The item handler of this crafting UI
        /// </summary>
        public IItemHandler ItemHandler => inventoryUI;

        /// <summary>
        /// Adds a crafting recipe UI for each recipe and sets their data
        /// </summary>
        private void Awake()
        {
            foreach (CraftingRecipe recipe in CraftingManager.Instance.Recipes)
                Instantiate(craftingRecipeUIPrefab, craftingRecipeGrid.transform).InitRecipe(recipe, this);
        }

        /// <summary>
        /// Shows/hides the crafting menu
        /// </summary>
        public void Toggle()
        {
            _isShowing = !_isShowing;
            craftingUIContent.SetActive(_isShowing);
        }
    }
}