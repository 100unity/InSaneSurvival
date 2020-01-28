using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Utils
{
    /// <summary>
    /// Simple class that shows some UI-Content on hover after a delay and moves it with the courser.
    /// Can be extended for more advanced usage.
    /// </summary>
    public abstract class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UITooltip base")] [Tooltip("Used for showing this on top of other UI-Elements")] [SerializeField]
        private Canvas canvas;

        [Tooltip("The whole tooltip gameObject. Will be used for movement and showing/hiding")] [SerializeField]
        private GameObject tooltipGameObject;

        [Tooltip("This gameObject will be flipped as well to counter-act the flipping of the normal gameObject." +
                 "Use this to display a child normally (e.g. texts)")]
        [SerializeField]
        private GameObject revertFlipGameObject;

        [Tooltip("Defines the time that the player has to hover over the element before it will be shown")]
        [SerializeField]
        private float showDelay;

        [Tooltip("Defines the offset that will be added to the mouse position. Used for calculating the position")]
        [SerializeField]
        private Vector2 offset;

        [Tooltip("Defines the flipped offset")] [SerializeField]
        private Vector2 flippedOffset;

        /// <summary>
        /// Whether this tooltip is currently deactivated. Will disable the activation/showing of the tooltip.
        /// </summary>
        protected bool IsDeactivated;

        /// <summary>
        /// Used for checking if the player has hovered long enough over the element.
        /// </summary>
        protected float Time = float.MaxValue;

        /// <summary>
        /// Whether this tooltip is flipped. Flipped hereby means moved to the left of the mouse
        /// </summary>
        private bool _isFlipped;

        /// <summary>
        /// Whether this tooltip is being shown.
        /// </summary>
        private bool _isShowing;


        /// <summary>
        /// Shows/Hides the tooltip, moves it to the render-top and hides the mouse.
        /// </summary>
        protected bool IsShowing
        {
            get => _isShowing;
            set
            {
                _isShowing = value;
                tooltipGameObject.SetActive(value);
                canvas.overrideSorting = value;
                canvas.sortingOrder = value ? 10 : 0;
                Cursor.visible = !value;
                Time = float.MaxValue;
            }
        }

        /// <summary>
        /// Deactivate tooltip by default.
        /// </summary>
        protected virtual void Awake() => tooltipGameObject.SetActive(false);

        /// <summary>
        /// Enable cursor if this element is deactivated.
        /// </summary>
        protected virtual void OnDisable() => IsShowing = false;

        /// <summary>
        /// Checks the restrictions, enables the tooltip and moves it.
        /// </summary>
        protected virtual void Update()
        {
            if (!IsDeactivated && !IsShowing && UnityEngine.Time.time >= Time + showDelay)
                IsShowing = true;

            if (!IsShowing) return;
            MoveTooltip();
        }

        /// <summary>
        /// Sets the time the player moved over the element. Used with <see cref="showDelay"/> to check if the tooltip
        /// should be shown
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData) => Time = UnityEngine.Time.time;

        /// <summary>
        /// Resets the time the player moved over the element and hides it.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData) => IsShowing = false;

        /// <summary>
        /// Moves the tooltip to the mouse position and applies the <see cref="offset"/>.
        /// Also flips the tooltip (moves it to the left of the mouse) if the tooltip would be out-of-bounds.
        /// </summary>
        private void MoveTooltip()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            bool outOfScreen = mousePos.x + offset.x + ((RectTransform) tooltipGameObject.transform).rect.width >=
                               Screen.width;

            if (!_isFlipped && outOfScreen) // If out of screen, flip
                FlipTooltip();
            else if (_isFlipped && !outOfScreen) // If inside of screen, flip back
                FlipTooltip();

            Vector2 newPosition = _isFlipped ? mousePos + flippedOffset : mousePos + offset;
            tooltipGameObject.transform.position = newPosition;
        }

        /// <summary>
        /// Flips the tooltip by setting it's scale to -1. 
        /// </summary>
        private void FlipTooltip()
        {
            _isFlipped = !_isFlipped;
            tooltipGameObject.transform.localScale = _isFlipped ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
            revertFlipGameObject.transform.localScale = _isFlipped ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        }
    }
}