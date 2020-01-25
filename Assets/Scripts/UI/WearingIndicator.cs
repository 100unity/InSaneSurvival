using Constants;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WearingIndicator : MonoBehaviour
    {
        [Tooltip("The image component for the equipped item icon")] [SerializeField]
        private Image iconContainer;

        /// <summary>
        /// Sets an icon to display in the wearing indicator
        /// </summary>
        /// <param name="icon">The icon to display</param>
        public void SetIcon(Sprite icon)
        {
            iconContainer.color = icon ? Consts.Colors.White : Consts.Colors.Transparent;
            iconContainer.sprite = icon;
        }
    }
}