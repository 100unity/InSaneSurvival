using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utils.ElementInteraction
{
    /// <summary>
    /// This allows to drag elements with the mouse
    /// <para>This requires some sort of ray-blocking/triggering component on its or one of its parents gameObjects to work properly</para>
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [Tooltip("The highest parent element that will be moved")] [SerializeField] private Transform parent;

        /// <summary>
        /// Will be triggered when the user stops dragging
        /// </summary>
        public event DragDelegate OnEndDragging;

        public delegate void DragDelegate(PointerEventData eventData);

        /// <summary>
        /// The old position before the drag started. Used for resetting the position.
        /// </summary>
        public Vector2 OldPosition { get; private set; }

        /// <summary>
        /// Used for finding UI elements with rays
        /// </summary>
        public GraphicRaycaster GraphicRaycaster { get; private set; }

        /// <summary>
        /// An event to be executed on <see cref="LateUpdate"/>
        /// </summary>
        private UnityAction _executeOnLateUpdate;

        /// <summary>
        /// Gets the GraphicRayCaster from the canvas
        /// </summary>
        private void Awake()
        {
            // Get the closest GraphicRaycaster (in hierarchy)
            GraphicRaycaster = GetComponentInParent<GraphicRaycaster>();
            if (GraphicRaycaster == null)
                Debug.LogError("There is no GraphicsRaycaster attached to the Canvas. Please add one!");
        }

        /// <summary>
        /// Wait for the Grid-Layout calculations to be done
        /// </summary>
        private void Start() => CoroutineManager.Instance.WaitForOneFrame(() => OldPosition = parent.position);

        /// <summary>
        /// Execute the current <see cref="executeOnLateUpdate"/> action
        /// </summary>
        private void LateUpdate()
        {
            _executeOnLateUpdate?.Invoke();
            _executeOnLateUpdate = null;
        }

        /// <summary>
        /// Follows the mouse position
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData) => parent.position = eventData.position;

        /// <summary>
        /// Invokes the <see cref="OnEndDrag"/> event
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData) => OnEndDragging?.Invoke(eventData);

        public void SetExecuteOnLateUpdateAction(UnityAction onLateUpdate) => _executeOnLateUpdate = onLateUpdate;

        /// <summary>
        /// Reset the elements position to the old position
        /// </summary>
        public void ResetPosition() => parent.position = OldPosition;

        /// <summary>
        /// Updates the current position and the OldPosition
        /// </summary>
        /// <param name="position"></param>
        public void UpdatePosition(Vector2 position)
        {
            parent.position = position;
            OldPosition = position;
        }
    }
}