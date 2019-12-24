using UI;
using UnityEngine;

namespace Utils.ElementInteraction
{
    /// <summary>
    /// Small class for determining other stackable and getting data from it
    /// </summary>
    public class Stackable : MonoBehaviour
    {
        [Tooltip("The item button for data")] [SerializeField]
        private ItemButton itemButton;

        public ItemButton ItemButton => itemButton;
    }
}