using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utils.ElementInteraction
{
    /// <summary>
    /// This requires some sort of ray-blocking/triggering component on its or one of its parents gameObjects to work properly
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public UnityAction executeOnLateUpdate;

        public delegate void DragDelegate(PointerEventData eventData);

        public event DragDelegate OnEndDragging;

        public Vector2 OldPosition { get; private set; }

        /// <summary>
        /// Used for finding UI elements with rays
        /// </summary>
        public GraphicRaycaster GraphicRaycaster { get; private set; }

        private void Awake()
        {
            // Get the closest GraphicRaycaster (in hierarchy)
            GraphicRaycaster = GetComponentInParent<GraphicRaycaster>();
            if (GraphicRaycaster == null)
                Debug.LogError("There is no GraphicsRaycaster attached to the Canvas. Please add one!");
        }

        private void Start()
        {
            //Wait for the Grid-Layout calculations to be done
            CoroutineManager.Instance.WaitForOneFrame(() => OldPosition = transform.position);
        }

        private void LateUpdate()
        {
            executeOnLateUpdate?.Invoke();
            executeOnLateUpdate = null;
        }

        public void OnDrag(PointerEventData eventData) => transform.position = eventData.position;

        public void OnEndDrag(PointerEventData eventData) => OnEndDragging?.Invoke(eventData);

        public void ResetPosition() => transform.position = OldPosition;

        public void UpdatePosition(Vector2 position)
        {
            transform.position = position;
            OldPosition = position;
        }
    }
}