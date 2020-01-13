using UnityEngine;

namespace Managers
{
    public class UIRenderManager : Singleton<UIRenderManager>
    {
        [SerializeField] private Canvas canvas;

        private GameObject _currentGameObject;
        private Transform _oldParent;

        public void SetCurrentTooltip(GameObject newTooltip = null)
        {
            // Move old gameObject back
            if (_currentGameObject != null)
                _currentGameObject.transform.parent = _oldParent;

            // If null, reset all
            if (newTooltip == null)
            {
                _oldParent = null;
                _currentGameObject = null;
                return;
            }

            // Move new object
            _oldParent = newTooltip.transform.parent;
            newTooltip.transform.parent = canvas.transform;
            _currentGameObject = newTooltip;
        }
    }
}