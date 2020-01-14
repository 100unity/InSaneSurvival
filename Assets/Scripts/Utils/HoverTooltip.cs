using Managers;
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
        [Header("UITooltip base")]
        [Tooltip("The content of the tooltip. Will be used for movement and showing/hiding")]
        [SerializeField]
        private GameObject tooltipContent;

        [Tooltip("Defines the time that the player has to hover over the element before it will be shown")]
        [SerializeField]
        private float showDelay;

        [Tooltip("Defines the offset that will be added to the mouse position. Used for calculating the position")]
        [SerializeField]
        private Vector2 offset;

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
        /// Shows/Hides the tooltip and moves it to the top by using the <see cref="UIRenderManager"/>.
        /// </summary>
        protected bool IsShowing
        {
            get => _isShowing;
            set
            {
                _isShowing = value;
                tooltipContent.SetActive(value);
                UIRenderManager.Instance.SetCurrentGameObject(value ? tooltipContent : null);
            }
        }

        /// <summary>
        /// Deactivate tooltip by default.
        /// </summary>
        protected virtual void Awake() => tooltipContent.SetActive(false);

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
        public void OnPointerExit(PointerEventData eventData)
        {
            Time = float.MaxValue;
            IsShowing = false;
        }

        /// <summary>
        /// Moves the tooltip to the mouse position and applies the <see cref="offset"/>.
        /// Also flips the tooltip (moves it to the left of the mouse) if the tooltip would be out-of-bounds.
        /// </summary>
        private void MoveTooltip()
        {
            Vector2 newPosition = Mouse.current.position.ReadValue() + offset;
            bool outOfScreen = newPosition.x + ((RectTransform) tooltipContent.transform).rect.width >= Screen.width;

            if (!_isFlipped && outOfScreen) // If out of screen, flip
                FlipTooltip();
            else if (_isFlipped && !outOfScreen) // If inside of screen, flip back
                FlipTooltip();

            tooltipContent.transform.position = newPosition;
        }

        /// <summary>
        /// Flips the tooltip by moving it to the left or right of the mouse.
        /// </summary>
        private void FlipTooltip()
        {
            _isFlipped = !_isFlipped;
            ((RectTransform) tooltipContent.transform).pivot = _isFlipped ? new Vector2(1, 1) : new Vector2(0, 1);
        }
    }
}