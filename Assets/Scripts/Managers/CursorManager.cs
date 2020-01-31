using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Managers
{
    public class CursorManager : Singleton<CursorManager>
    {
        [SerializeField] private GraphicRaycaster raycaster;

        private readonly Vector2 _zeroVector2 = Vector2.zero;

        /// <summary>
        /// Sets the cursor to the given texture. The texture must be a Cursor in import settings (Read/Write).
        /// </summary>
        /// <param name="newCursorIcon">The new cursor texture.</param>
        public void SetCursorIcon(Texture2D newCursorIcon)
        {
            if (IsOverUI())
            {
                SetCursorToDefault();
                return;
            }

            Vector2 hotspot = new Vector2(newCursorIcon.width / 2f, newCursorIcon.height / 2f);
            Cursor.SetCursor(newCursorIcon, hotspot, CursorMode.Auto);
        }

        /// <summary>
        /// Reverts the cursor to the default one.
        /// </summary>
        public void SetCursorToDefault() => Cursor.SetCursor(null, _zeroVector2, CursorMode.Auto);

        /// <summary>
        /// Checks if the Cursor is on an UI-Element and if so, sets the cursor to the default one.
        /// BUG: The new EventSystem does not update the cursor-data correctly, causing this method to fail.
        /// </summary>
        public void UpdateCursor()
        {
            CoroutineManager.Instance.WaitForFrames(3, () =>
            {
                if (IsOverUI())
                    SetCursorToDefault();
            });
        }

        /// <summary>
        /// Checks if the Cursor is on an UI-Element.
        /// </summary>
        /// <returns></returns>
        private bool IsOverUI() => EventSystem.current.IsPointerOverGameObject();
    }
}