﻿using UnityEngine;
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

        [Tooltip("Disables the dragging behaviour. This does not influence the swapping")] [SerializeField]
        private bool disableDragging;

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

            if (disableDragging)
                _draggable.enabled = false;
        }

        private void OnEnable() => _draggable.OnEndDragging += Swap;

        private void OnDisable() => _draggable.OnEndDragging -= Swap;

        /// <summary>
        /// Reset the elements position to the old position
        /// </summary>
        public void ResetPosition() => _draggable.ResetPosition();

        /// <summary>
        /// Swap with another Swappable. If no swappable found, snap to old position
        /// </summary>
        /// <param name="eventData"></param>
        private void Swap(PointerEventData eventData)
        {
            Swappable otherSwappable = _draggable.HighestGraphicRaycaster.FindUIElement(eventData, this);
            // No swappable, nothing to do
            if (otherSwappable == null)
            {
                ResetPosition();
                return;
            }

            // Invoke OnSwap event before swapping
            if (OnBeforeSwap != null && !OnBeforeSwap.Invoke(otherSwappable))
                return;

            _draggable.RefreshPosition();
            OnAfterSwap?.Invoke(this, otherSwappable);
        }
    }
}