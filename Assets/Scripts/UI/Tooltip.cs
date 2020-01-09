using UnityEngine;
using Constants;

namespace UI
{
    public class Tooltip : MonoBehaviour
    {
        [Tooltip("Highlight color when hover over object.")] [SerializeField]
        private Color highlightColor = new Color(0.38f, 0.97f, 0.44f);

        [Tooltip("Fade speed of the color change (slow -> quick)")] [SerializeField]
        private float highlightSpeed = 4f;

        [Tooltip("Show a text over the interacted object.")] [SerializeField]
        private bool hideTooltip;

        [Tooltip("Show a predefined UI Panel over the interacted object (intend to use for items in inventory)")]
        [SerializeField]
        private GameObject tooltipUIPanel;

        [Tooltip("Show the tooltip fixed the object instead of following the mouse.")] [SerializeField]
        private bool followingMouseCursor;

        [Tooltip("Position of the tooltip showed over the interacted object.")] [SerializeField]
        private Vector2 tooltipPosition = new Vector2(-50f, -80f);

        [Tooltip("Text to show over the interacted object.")] [SerializeField]
        private string tooltipText;

        [Tooltip("Color of the text showed over the interacted object.")] [SerializeField]
        private Color tooltipColor = Consts.Colors.White;

        [Tooltip("Size of the text showed over the interacted object.")] [SerializeField]
        private int tooltipSize = 20;

        [Tooltip("Resize the text, relative to the distance between the object and the camera.")] [SerializeField]
        private bool textResized;

        [Tooltip("Font of the text showed over the interacted object.")] [SerializeField]
        private Font tooltipFont;

        private enum TooltipAlignment { Center, Left, Right }

        [Tooltip("Alignment of the text showed over the interacted object.")] [SerializeField]
        private TooltipAlignment tooltipAlignment;

        [Tooltip("Texture for the icon beside text tooltip")] [SerializeField]
        private Texture image;

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
        private Vector3 _position;

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

            _position = transform.position;
        }

        /// <summary>
        /// Setting up the tooltip at start with color, shadow, text, size, font and alignment
        /// </summary>
        private void Start()
        {
            // Start settings of the tooltip
            if (!hideTooltip)
            {
                // Tooltip text style customization
                if (tooltipUIPanel != null)
                {
                    // Initialization of the UI Panel
                    tooltipUIPanel.SetActive(false);
                }

                _tooltipStyle.normal.textColor = tooltipColor; // Color of the tooltip text
                _tooltipStyleShadow.normal.textColor =
                    Consts.Tooltip.TOOLTIP_SHADOW_COLOR; // Color of the tooltip shadow
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
                if (textResized)
                {
                    _cameraDistance = Vector3.Distance(Camera.main.transform.position, _position);
                    _tooltipStyle.fontSize = _tooltipStyleShadow.fontSize =
                        Mathf.RoundToInt(tooltipSize - _cameraDistance / 3);
                }

                GUI.Label(
                    new Rect(Event.current.mousePosition.x + tooltipPosition.x - 40f,
                        Event.current.mousePosition.y + tooltipPosition.y, 100f, 20f), image, _tooltipStyle);
                GUI.Label(
                    new Rect(
                        Event.current.mousePosition.x + tooltipPosition.x - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.x,
                        Event.current.mousePosition.y + tooltipPosition.y - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.y,
                        100f, 20f), _currentText, _tooltipStyleShadow);
                GUI.Label(
                    new Rect(Event.current.mousePosition.x + tooltipPosition.x,
                        Event.current.mousePosition.y + tooltipPosition.y, 100f, 20f), _currentText, _tooltipStyle);
                if (tooltipUIPanel != null)
                {
                    tooltipUIPanel.transform.localPosition = new Vector3(
                        Event.current.mousePosition.x + tooltipPosition.x - Screen.width / 2f,
                        -Event.current.mousePosition.y + tooltipPosition.y + Screen.height / 2f, 0);
                }
            }
            // Display of text/tooltip that fixed to the object
            else if (!hideTooltip && !followingMouseCursor && _over)
            {
                _positionToScreen = Camera.main.WorldToScreenPoint(_position);
                _cameraDistance = Vector3.Distance(Camera.main.transform.position, _position);
                if (textResized)
                {
                    _tooltipStyle.fontSize = _tooltipStyleShadow.fontSize =
                        Mathf.RoundToInt(tooltipSize - _cameraDistance / 3);
                }

                //GUI.DrawTexture(new Rect((positionToScreen.x + tooltipPosition.x) - 40f, -positionToScreen.y + Screen.height + tooltipPosition.y / (cameraDistance / 10), 100f, 20f), image, ScaleMode.ScaleToFit);
                GUI.Label(
                    new Rect(_positionToScreen.x + tooltipPosition.x - 40f,
                        -_positionToScreen.y + Screen.height + tooltipPosition.y / (_cameraDistance / 10), 100f, 20f),
                    image, _tooltipStyle);
                GUI.Label(
                    new Rect(_positionToScreen.x + tooltipPosition.x - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.x,
                        -_positionToScreen.y + Screen.height + tooltipPosition.y / (_cameraDistance / 10) -
                        Consts.Tooltip.TOOLTIP_SHADOW_POSITION.y, 100f, 20f), _currentText, _tooltipStyleShadow);
                GUI.Label(
                    new Rect(_positionToScreen.x + tooltipPosition.x,
                        -_positionToScreen.y + Screen.height + tooltipPosition.y / (_cameraDistance / 10), 100f, 20f),
                    _currentText, _tooltipStyle);
                if (tooltipUIPanel != null)
                {
                    tooltipUIPanel.transform.localPosition = new Vector3(
                        _positionToScreen.x + tooltipPosition.x - Screen.width / 2f,
                        _positionToScreen.y - Screen.height / 2f + tooltipPosition.y / (_cameraDistance / 10),
                        _positionToScreen.z);
                }
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

            if (tooltipUIPanel != null)
            {
                Invoke(nameof(TooltipDelayOn), 0.05f);
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

            if (tooltipUIPanel != null)
            {
                Invoke(nameof(TooltipDelayOff), 0.05f);
            }
        }

        /// <summary>
        /// Delay for the tooltip display (on)
        /// </summary>
        private void TooltipDelayOn() => tooltipUIPanel.SetActive(true);

        /// <summary>
        /// Delay for the tooltip display (off)
        /// </summary>
        private void TooltipDelayOff() => tooltipUIPanel.SetActive(false);
    }
}