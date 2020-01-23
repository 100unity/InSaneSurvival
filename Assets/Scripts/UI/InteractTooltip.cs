using UnityEngine;
using Constants;

namespace UI
{
    [RequireComponent(typeof(Collider))]
    public class InteractTooltip : MonoBehaviour
    {
        [Tooltip("Highlight color when hover over object.")]
        [SerializeField]
        private Color highlightColor = new Color(0.38f, 0.97f, 0.44f);

        [Tooltip("Fade speed of the color change (slow -> quick)")]
        [SerializeField]
        private float highlightSpeed = 4f;

        [Tooltip("Show a text over the interacted object.")]
        [SerializeField]
        private bool hideTooltip;

        [Tooltip("Show the tooltip fixed the object instead of following the mouse.")]
        [SerializeField]
        private bool followingMouseCursor;

        [Tooltip("Position of the tooltip showed over the interacted object.")]
        [SerializeField]
        private Vector2 tooltipPosition;

        [Tooltip("Text to show over the interacted object.")]
        [SerializeField]
        private string tooltipText;

        [Tooltip("Color of the text showed over the interacted object.")]
        [SerializeField]
        private Color tooltipColor = Consts.Colors.White;

        [Tooltip("Size of the text showed over the interacted object.")]
        [SerializeField]
        private int tooltipSize = 20;

        [Tooltip("Font of the text showed over the interacted object.")]
        [SerializeField]
        private Font tooltipFont;

        private enum TooltipAlignment { Center, Left, Right }

        [Tooltip("Alignment of the text showed over the interacted object.")]
        [SerializeField]
        private TooltipAlignment tooltipAlignment;

        [Tooltip("Texture for the icon beside text tooltip")]
        [SerializeField]
        private Texture interactIcon;

        [Tooltip("Icon size")]
        [SerializeField]
        private int iconSize = 25;

        [Tooltip("Offset for icon pos.x to be correctly displayed beside tooltip text")]
        [SerializeField]
        private int iconOffset;

        [Tooltip("Cursor sprite when interaction with the object (texture must be a Cursor in import settings)")]
        private Texture2D mouseCursor;

        // Reference Components
        private Renderer _renderer;
        private Material[] _allMaterials;
        private Color[] _baseColor;
        private float _t; // Time for fading the highlight color
        private bool _over; // Indicate when the mouse cursor is over the object
        private string _currentText;
        private GUIStyle _tooltipStyle = new GUIStyle();
        private GUIStyle _tooltipStyleShadow = new GUIStyle();
        private Vector3 _positionToScreen;
        private float _cameraDistance;

        private void Awake()
        {
            // Get all materials and all colors for supporting multi-materials object
            _renderer = GetComponent<Renderer>();
            _allMaterials = _renderer.materials;
            _baseColor = new Color[_allMaterials.Length];
            for (int i = 0; i < _baseColor.Length; i++)
            {
                _baseColor[i] = _allMaterials[i].color;
            }
        }

        /// <summary>
        /// Setting up the tooltip at start with color, shadow, text, size, font and alignment
        /// </summary>
        private void Start()
        {
            // Start settings of the tooltip
            if (!hideTooltip)
            {
                _tooltipStyle.normal.textColor = tooltipColor; // Color of the tooltip text
                _tooltipStyleShadow.normal.textColor = Consts.Tooltip.TOOLTIP_SHADOW_COLOR; // Color of the tooltip shadow
                _tooltipStyle.fontSize = _tooltipStyleShadow.fontSize = tooltipSize; // Size of the tooltip font
                _tooltipStyle.fontStyle = _tooltipStyleShadow.fontStyle = FontStyle.Bold; // Style of the tooltip font
                _tooltipStyle.font = _tooltipStyleShadow.font = tooltipFont;
                switch (tooltipAlignment)
                {
                    // Alignment of the tooltip text
                    case TooltipAlignment.Center:
                        _tooltipStyle.alignment = _tooltipStyleShadow.alignment = TextAnchor.UpperCenter;
                        break;
                    case TooltipAlignment.Left:
                        _tooltipStyle.alignment = _tooltipStyleShadow.alignment = TextAnchor.UpperLeft;
                        break;
                    case TooltipAlignment.Right:
                        _tooltipStyle.alignment = _tooltipStyleShadow.alignment = TextAnchor.UpperRight;
                        break;
                }
            }
        }

        /// <summary>
        /// Checking if the mouse cursor is over the object and start fading the color in
        /// and fading out when the cursor exit the object.
        /// </summary>
        private void Update()
        {
            if (_over && _t < 1f)
            {
                // Fade in highlight color
                foreach (Material material in _allMaterials)
                {
                    material.color = Color.Lerp(material.color, highlightColor, _t);
                }

                _t += highlightSpeed * Time.deltaTime;
            }
            else if (!_over && _t < 1f)
            {
                // Fade out highlight color
                foreach (Material material in _allMaterials)
                {
                    material.color = Color.Lerp(material.color,
                        _baseColor[System.Array.IndexOf(_allMaterials, material)], _t);
                }

                _t += highlightSpeed * Time.deltaTime;
            }
        }

        // Called when mouse over this object
        private void OnMouseEnter()
        {
            ShowTooltip();
        }

        // Called when mouse exit this object
        private void OnMouseExit()
        {
            HideTooltip();
        }

        /// <summary>
        /// Tooltip creation. This will automatically draw a Tooltip on position depends on what style we want.
        /// </summary>
        private void OnGUI()
        {
            // Display of text/tooltip that follows the mouse
            if (!hideTooltip && followingMouseCursor && _over)
            {
                _cameraDistance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
                // Resized the text relative to the camera distance and size of the tooltip
                _tooltipStyle.fontSize = _tooltipStyleShadow.fontSize = Mathf.RoundToInt(tooltipSize - _cameraDistance / 3);

                if (interactIcon != null)
                {
                    GUI.DrawTexture(new Rect(Event.current.mousePosition.x + tooltipPosition.x - iconOffset, Event.current.mousePosition.y + tooltipPosition.y, 100f, iconSize), interactIcon, ScaleMode.ScaleToFit);
                }
                GUI.Label(
                    new Rect(
                        Event.current.mousePosition.x + tooltipPosition.x - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.x,
                        Event.current.mousePosition.y + tooltipPosition.y - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.y,
                        100f, 20f), _currentText, _tooltipStyleShadow);
                GUI.Label(
                    new Rect(Event.current.mousePosition.x + tooltipPosition.x,
                        Event.current.mousePosition.y + tooltipPosition.y, 100f, 20f), _currentText, _tooltipStyle);
            }
            // Display of text/tooltip that fixed to the object
            else if (!hideTooltip && !followingMouseCursor && _over)
            {
                _positionToScreen = Camera.main.WorldToScreenPoint(transform.position);
                _cameraDistance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
                // Resized the text relative to the camera distance and size of the tooltip
                _tooltipStyle.fontSize = _tooltipStyleShadow.fontSize = Mathf.RoundToInt(tooltipSize - _cameraDistance / 3);

                if (interactIcon != null)
                {
                    GUI.DrawTexture(new Rect(_positionToScreen.x + tooltipPosition.x - iconOffset, -_positionToScreen.y + Screen.height + tooltipPosition.y / (_cameraDistance / 10), 100f, iconSize), interactIcon, ScaleMode.ScaleToFit);
                }
                GUI.Label(
                    new Rect(_positionToScreen.x + tooltipPosition.x - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.x,
                        -_positionToScreen.y + Screen.height + tooltipPosition.y / (_cameraDistance / 10) -
                        Consts.Tooltip.TOOLTIP_SHADOW_POSITION.y, 100f, 20f), _currentText, _tooltipStyleShadow);
                GUI.Label(
                    new Rect(_positionToScreen.x + tooltipPosition.x,
                        -_positionToScreen.y + Screen.height + tooltipPosition.y / (_cameraDistance / 10), 100f, 20f),
                    _currentText, _tooltipStyle);
            }
        }

        /// <summary>
        /// Show tooltip and focus color of this object
        /// </summary>
        public void ShowTooltip()
        {
            _t = 0f;
            _over = true;
            _currentText = tooltipText;

            //Change back the cursor texture when hovering over the object
            if (mouseCursor != null)
            {
                Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);
            }
        }

        /// <summary>
        /// Hide tooltip and focus color of this object
        /// </summary>
        public void HideTooltip()
        {
            _t = 0f;
            _over = false;
            _currentText = "";

            //Change back the cursor texture when cursor exit the object
            if (mouseCursor != null)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}