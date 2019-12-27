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
        [Tooltip("The highest parent element")] [SerializeField]
        private GameObject parent;

        /// <summary>
        /// The highest parent of this swappable. Should be used for positioning
        /// </summary>
        public GameObject Parent => parent;

        /// <summary>
        /// Event that will be executed when another swappable was found.
        /// If the returned boolean is false, the swap will be skipped.
        /// </summary>
        public event SwapDelegate OnBeforeSwap;

        /// <summary>
        /// Event that will be triggered after a swap is completed.
        /// </summary>
        public static event SwapCompletedDelegate OnAfterSwap;

        public delegate bool SwapDelegate(Swappable otherSwappable);

        public delegate void SwapCompletedDelegate(Swappable swappable, Swappable otherSwappable);

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
            _draggable.SetExecuteOnLateUpdateAction(_draggable.ResetPosition);

            Swappable otherSwappable = _draggable.GraphicRaycaster.FindUIElement(eventData, this);
            // No swappable, nothing to do
            if (otherSwappable == null)
                return;

            // Invoke OnSwap event before swapping
            if (OnBeforeSwap != null && !OnBeforeSwap.Invoke(otherSwappable))
                return;

            //Switch positions
            Vector2 otherPosition = otherSwappable._draggable.OldPosition;
            otherSwappable._draggable.UpdatePosition(_draggable.OldPosition);
            _draggable.UpdatePosition(otherPosition);
            // Remove default position-reset
            _draggable.SetExecuteOnLateUpdateAction(null);
            OnAfterSwap?.Invoke(this, otherSwappable);
        }
    }
}