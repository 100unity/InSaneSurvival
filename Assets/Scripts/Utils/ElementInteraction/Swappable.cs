using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.ElementInteraction
{
    [RequireComponent(typeof(Draggable))]
    public class Swappable : MonoBehaviour
    {
        private Draggable _draggable;

        private void Awake()
        {
            _draggable = GetComponent<Draggable>();
            _draggable.OnEndDragging += Swap;
        }

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

            _draggable.executeOnLateUpdate = null;
        }
    }
}