using UnityEngine;

namespace Managers
{
    public class CursorManager : Singleton<CursorManager>
    {
        private readonly Vector2 _zeroVector2 = Vector2.zero;

        public void SetCursorIcon(Texture2D newCursorIcon)
        {
            Vector2 hotspot = new Vector2(newCursorIcon.width / 2f, newCursorIcon.height / 2f);
            Cursor.SetCursor(newCursorIcon, hotspot, CursorMode.Auto);
        }

        public void SetCursorToDefault() => Cursor.SetCursor(null, _zeroVector2, CursorMode.Auto);
    }
}