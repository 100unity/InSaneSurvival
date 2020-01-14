using UnityEngine;
using Utils;

namespace Managers
{
    /// <summary>
    /// Can be used to show a UI-Element on top of others. Uses a different canvas with a higher "Sort Order".
    /// Currently used by <see cref="HoverTooltip"/>.
    /// Can only hold one element at a time.
    /// </summary>
    public class UIRenderManager : Singleton<UIRenderManager>
    {
        [Tooltip("The canvas that has a higher sort-order. Will be the new parent of the element")] [SerializeField]
        private Canvas canvas;

        /// <summary>
        /// Used for reverting everything.
        /// </summary>
        private GameObject _currentGameObject;

        /// <summary>
        /// Used for moving it back.
        /// </summary>
        private Transform _oldParent;

        /// <summary>
        /// Moves the given gameObject to the higher canvas and saves it. Also moves the old one back, if there was one.
        /// </summary>
        /// <param name="newGameObject">The new gameObject to be moved on top.</param>
        public void SetCurrentGameObject(GameObject newGameObject = null)
        {
            // Move old gameObject back
            if (_currentGameObject != null)
                _currentGameObject.transform.parent = _oldParent;

            // If null, reset all
            if (newGameObject == null)
            {
                _oldParent = null;
                _currentGameObject = null;
                return;
            }

            // Move new object
            _oldParent = newGameObject.transform.parent;
            newGameObject.transform.parent = canvas.transform;
            _currentGameObject = newGameObject;
        }
    }
}