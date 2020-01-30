using Managers;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Collider))]
    public class InteractTooltip : MonoBehaviour
    {
        [Tooltip("Cursor sprite when interaction with the object (texture must be a Cursor in import settings)")]
        [SerializeField]
        private Texture2D mouseCursor;


        private void OnMouseEnter() => CursorManager.Instance.SetCursorIcon(mouseCursor);


        private void OnMouseExit() => CursorManager.Instance.SetCursorToDefault();
    }
}