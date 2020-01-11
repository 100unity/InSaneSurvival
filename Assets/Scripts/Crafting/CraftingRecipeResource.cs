using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
    public class CraftingRecipeResource : MonoBehaviour
    {
        [Tooltip("The text to be displayed next to the resource sprite")] [SerializeField]
        private TextMeshProUGUI txtResource;

        [Tooltip("The image for this resource")] [SerializeField]
        private Image imgResource;

        /// <summary>
        /// Sets the name and sprite for this crafting recipe resource
        /// </summary>
        /// <param name="resourceName">The name for this resource</param>
        /// <param name="amount">The quantity</param>
        /// <param name="resourceSprite">The sprite for this resource</param>
        public void InitResource(string resourceName, int amount, Sprite resourceSprite)
        {
            string amountText = $" x{amount}";
            txtResource.SetText(amountText);
            imgResource.sprite = resourceSprite;
        }
    }
}