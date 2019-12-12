using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.ElementInteraction
{
    /// <summary>
    /// Allows swapping with another Swappable. Snaps back to old position by default
    /// </summary>
    [RequireComponent(typeof(Draggable))]
    public class Swappable : MonoBehaviour
    {
        /// <summary>
        /// The draggable component used for the dragging events
        /// </summary>
        private Draggable _draggable;

        /// <summary>
        /// Subscribe to <see cref="Draggable.OnEndDrag"/> event
        /// </summary>
        private void Awake()
        {
            _draggable = GetComponent<Draggable>();
            _draggable.OnEndDragging += Swap;
        }

        /// <summary>
        /// Swap with another Swappable. If no swappable found, snap to old position
        /// </summary>
        /// <param name="eventData"></param>
        private void Swap(PointerEventData eventData)
        {
            // Reset position by default
            _draggable.executeOnLateUpdate = _draggable.ResetPosition;

            Swappable otherSwappable = _draggable.GraphicRaycaster.FindUIElement(eventData, this);
            // No swappable, nothing to do
            if (otherSwappable == null)
                return;

            //Switch positions
            Vector2 otherPosition = otherSwappable._draggable.OldPosition;
            otherSwappable._draggable.UpdatePosition(_draggable.OldPosition);
            _draggable.UpdatePosition(otherPosition);
            // Remove default position-reset
            _draggable.executeOnLateUpdate = null;
        }
    }
}