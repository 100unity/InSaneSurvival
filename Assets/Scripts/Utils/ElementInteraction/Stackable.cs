using Inventory.UI;
using UnityEngine;

namespace Utils.ElementInteraction
{
    /// <summary>
    /// Small class for determining other stackable and getting data from it
    /// </summary>
    public class Stackable : MonoBehaviour
    {
        [Tooltip("The ItemButton for data")] [SerializeField]
        private ItemButton itemButton;

        /// <summary>
        /// ItemButton component reference
        /// </summary>
        public ItemButton ItemButton => itemButton;
    }
}