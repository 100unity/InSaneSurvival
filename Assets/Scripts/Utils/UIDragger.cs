using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utils
{
    /// <summary>
    /// Allows the dragging of an UI-Element.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class UIDragger : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [Tooltip("The UI-Content that can be dragged. Should be the up-most parent of the UI. Cannot drag Canvases!")]
        [SerializeField]
        private GameObject draggableUIContent;

        /// <summary>
        /// The mouse position relative to the center of the <see cref="draggableUIContent"/>. Will be added as offset.
        /// </summary>
        private Vector2 _mouseOffset;

        /// <summary>
        /// Calculates the offset of the click relative to the center of the <see cref="draggableUIContent"/>.
        /// This will make sure that the contents center does not snap to the mouse position but stays the way it was.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData) =>
            _mouseOffset = (Vector2) draggableUIContent.transform.position - eventData.position;

        /// <summary>
        /// Moves the position of <see cref="draggableUIContent"/> with the mouse position and the offset.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData) =>
            draggableUIContent.transform.position = eventData.position + _mouseOffset;
    }
}