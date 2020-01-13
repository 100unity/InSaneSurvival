using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils.ElementInteraction;

namespace UI
{
    public class UITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Logic")] [SerializeField] private GameObject tooltipContent;
        [SerializeField] private float showDelay;
        [SerializeField] private Vector2 offset;
        [SerializeField] private Draggable draggable;

        private bool IsShowing
        {
            get => _isShowing;
            set
            {
                _isShowing = value;
                tooltipContent.SetActive(value);
                UIRenderManager.Instance.SetCurrentTooltip(value ? tooltipContent : null);
            }
        }

        private bool _isShowing;
        private bool _isDeactivated;
        private float _time = float.MaxValue;
        private bool _isFlipped;


        private void Awake() => tooltipContent.SetActive(false);

        private void OnEnable()
        {
            if (!draggable) return;
            draggable.OnBeginDragging += Deactivate;
            draggable.OnEndDragging += Activate;
        }

        private void OnDisable()
        {
            if (!draggable) return;
            draggable.OnBeginDragging -= Deactivate;
            draggable.OnEndDragging -= Activate;
        }

        private void Update()
        {
            if (!_isDeactivated && !IsShowing && Time.time >= _time + showDelay)
                IsShowing = true;

            if (!_isShowing) return;
            MoveTooltip();
        }

        public void OnPointerEnter(PointerEventData eventData) => _time = Time.time;

        public void OnPointerExit(PointerEventData eventData)
        {
            _time = float.MaxValue;
            IsShowing = false;
        }


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


        private void Activate(PointerEventData eventData)
        {
            _time = float.MaxValue;
            _isDeactivated = false;
        }

        private void Deactivate(PointerEventData eventData)
        {
            _isDeactivated = true;
            IsShowing = false;
        }

        private void FlipTooltip()
        {
            _isFlipped = !_isFlipped;
            ((RectTransform) tooltipContent.transform).pivot = _isFlipped ? new Vector2(1, 1) : new Vector2(0, 1);
        }
    }
}