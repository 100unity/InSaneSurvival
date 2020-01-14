using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
    public class CraftingUI : MonoBehaviour
    {
        [Tooltip("The content of the crafting UI. Used for hiding it")] [SerializeField]
        private GameObject craftingUIContent;

        [Tooltip("The grid for the recipes to be displayed in")] [SerializeField]
        private LayoutGroup craftingRecipeGrid;

        [Tooltip("The crafting recipe UI prefab")] [SerializeField]
        private CraftingRecipeUI craftingRecipeUIPrefab;

        /// <summary>
        /// Whether the crafting UI is currently visible to the player
        /// </summary>
        private bool _isShowing;

        /// <summary>
        /// A list of all instantiated CraftingRecipeUIs. Used for updating.
        /// </summary>
        private readonly List<CraftingRecipeUI> _recipeUIs = new List<CraftingRecipeUI>();

        /// <summary>
        /// Adds a crafting recipe UI for each recipe and sets their data.
        /// </summary>
        private void Awake()
        {
            foreach (CraftingRecipe recipe in CraftingManager.Instance.Recipes)
            {
                CraftingRecipeUI recipeUI = Instantiate(craftingRecipeUIPrefab, craftingRecipeGrid.transform);
                recipeUI.InitRecipe(recipe);
                _recipeUIs.Add(recipeUI);
            }
        }

        /// <summary>
        /// Listen for crafting update.
        /// </summary>
        private void OnEnable() => CraftingManager.Instance.OnCraftingUpdate += RefreshRecipes;

        private void OnDisable() => CraftingManager.Instance.OnCraftingUpdate -= RefreshRecipes;

        /// <summary>
        /// Shows/hides the crafting menu.
        /// </summary>
        public void Toggle()
        {
            _isShowing = !_isShowing;
            craftingUIContent.SetActive(_isShowing);
        }

        /// <summary>
        /// Refreshes all recipes by checking again if they can be crafted.
        /// </summary>
        private void RefreshRecipes() => _recipeUIs.ForEach(recipe => recipe.UpdateRecipe());
    }
}