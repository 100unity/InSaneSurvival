using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utils.ElementInteraction
{
    /// <summary>
    /// This allows to drag elements with the mouse
    /// <para>This requires some sort of ray-blocking/triggering component on its or one of its parents gameObjects to work properly</para>
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [Tooltip("Used for showing this on top of other UI-Elements")] [SerializeField]
        private Canvas canvas;

        [Tooltip("Used for deactivating when dragging")] [SerializeField]
        private GraphicRaycaster graphicRaycaster;

        [Tooltip("The highest parent element that will be moved")] [SerializeField]
        private Transform parent;

        /// <summary>
        /// Used for finding UI elements with rays.
        /// </summary>
        public GraphicRaycaster HighestGraphicRaycaster { get; private set; }

        /// <summary>
        /// Will be triggered when the user stops dragging.
        /// </summary>
        public event DragDelegate OnEndDragging;

        /// <summary>
        /// Will be triggered when the user starts dragging.
        /// </summary>
        public event DragDelegate OnBeginDragging;

        public delegate void DragDelegate(PointerEventData eventData);

        /// <summary>
        /// The old position before the drag started. Used for resetting the position.
        /// </summary>
        private Vector2 OldPosition { get; set; }

        private bool IsBeingMoved
        {
            set
            {
                graphicRaycaster.enabled = !value;
                canvas.overrideSorting = value;
                canvas.sortingOrder = value ? 1 : 0;
            }
        }

        /// <summary>
        /// Gets the GraphicRayCaster from the canvas
        /// </summary>
        private void Awake()
        {
            // Get the highest GraphicRaycaster (in hierarchy)
            HighestGraphicRaycaster = GetComponentsInParent<GraphicRaycaster>().Last();
            if (HighestGraphicRaycaster == null)
                Debug.LogError("There is no GraphicsRaycaster attached to the Canvas. Please add one!");
        }

        /// <summary>
        /// Save old position before the drag
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            OldPosition = parent.position;
            OnBeginDragging?.Invoke(eventData);
            IsBeingMoved = true;
        }

        /// <summary>
        /// Follows the mouse position
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            parent.position = eventData.position;
            Cursor.visible = false;
        }

        /// <summary>
        /// Invokes the <see cref="OnEndDrag"/> event
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragging?.Invoke(eventData);
            IsBeingMoved = false;
            Cursor.visible = true;
        }

        /// <summary>
        /// Reset the elements position to the old position
        /// </summary>
        public void ResetPosition() => parent.position = OldPosition;

        /// <summary>
        /// Saves the current position of the parent as <see cref="OldPosition"/>
        /// </summary>
        public void RefreshPosition() => OldPosition = parent.position;

        /// <summary>
        /// Updates the current position and the OldPosition
        /// </summary>
        /// <param name="position">The new postiion</param>
        public void UpdatePosition(Vector2 position)
        {
            parent.position = position;
            OldPosition = position;
        }
    }
}