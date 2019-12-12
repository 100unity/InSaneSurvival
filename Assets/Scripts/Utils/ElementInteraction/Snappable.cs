using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.ElementInteraction
{
    [RequireComponent(typeof(Draggable))]
    public class Snappable : MonoBehaviour
    {
        private Draggable _draggable;

        private void Awake()
        {
            _draggable = GetComponent<Draggable>();
            _draggable.OnEndDragging += Snap;
        }

        private void Snap(PointerEventData eventData)
        {
            // Reset position by default
            _draggable.executeOnLateUpdate = _draggable.ResetPosition;

            SnapPoint snapPoint = _draggable.GraphicRaycaster.FindUIElement<SnapPoint>(eventData);
            Draggable otherDraggable = _draggable.GraphicRaycaster.FindUIElement(eventData, _draggable);
            // If there is no snapping point or if there is already another snappable, don't do anything
            if (snapPoint == null || otherDraggable != null)
                return;

            // Get new position and set to current
            _draggable.UpdatePosition(snapPoint.transform.position);

            _draggable.executeOnLateUpdate = null;
        }
    }
}