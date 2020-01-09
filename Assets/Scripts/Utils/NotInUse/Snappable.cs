using UnityEngine;
using UnityEngine.EventSystems;
using Utils.ElementInteraction;

namespace Utils.NotInUse
{
    /// <summary>
    /// Allows snapping to a <see cref="SnapPoint"/>
    /// </summary>
    [RequireComponent(typeof(Draggable))]
    public class Snappable : MonoBehaviour
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
            _draggable.OnEndDragging += Snap;
        }

        /// <summary>
        /// Try to find a <see cref="SnapPoint"/>. If found, snap to it, if taken or nothing found, snap to old position
        /// </summary>
        /// <param name="eventData">The EventData from the <see cref="Draggable.OnEndDrag"/> event</param>
        private void Snap(PointerEventData eventData)
        {
            SnapPoint snapPoint = _draggable.GraphicRaycaster.FindUIElement<SnapPoint>(eventData);
            Draggable otherDraggable = _draggable.GraphicRaycaster.FindUIElement(eventData, _draggable);
            // If there is no snapping point or if there is already another snappable, don't do anything
            if (snapPoint == null || otherDraggable != null)
            {
                _draggable.ResetPosition();
                return;
            }

            // Get new position and set to current
            _draggable.UpdatePosition(snapPoint.transform.position);
        }
    }
}