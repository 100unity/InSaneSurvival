using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Constants;

namespace UI
{
    public class Tooltip : MonoBehaviour
    {
        [Tooltip("Highlight color when hover over object.")]
        [SerializeField] private Color highlightColor = new Color(0.38f, 0.97f, 0.44f);

        [Tooltip("Fade speed of the color change (slow -> quick)")]
        [SerializeField] private float highlightSpeed = 4f;

        [Tooltip("Show a text over the interacted object.")]
        [SerializeField] private bool hideTooltip;

        [Tooltip("Show a predefined UI Panel over the interacted object (intend to use for items in inventory)")]
        [SerializeField] private GameObject tooltipUIPanel;

        [Tooltip("Show the tooltip fixed the object instead of following the mouse.")]
        [SerializeField] private bool fixedToTheObject = true;

        [Tooltip("Position of the tooltip showed over the interacted object.")]
        [SerializeField] private Vector2 tooltipPosition = new Vector2(-50f, -80f);

        [Tooltip("Text to show over the interacted object.")]
        [SerializeField] private string tooltipText;

        [Tooltip("Color of the text showed over the interacted object.")]
        [SerializeField] private Color tooltipColor = Consts.Colors.White;

        [Tooltip("Size of the text showed over the interacted object.")]
        [SerializeField] private int tooltipSize = 20;

        [Tooltip("Resize the text, relative to the distance between the object and the camera.")]
        [SerializeField] private bool textResized;

        [Tooltip("Font of the text showed over the interacted object.")]
        [SerializeField] private Font tooltipFont;

        [SerializeField] private enum TooltipAlignment { Center, Left, Right }

        [Tooltip("Alignment of the text showed over the interacted object.")]
        [SerializeField] private TooltipAlignment tooltipAlignment;

        [Tooltip("Texture for the icon beside text tooltip")]
        [SerializeField] private Texture image;

        // Reference Components
        private Renderer _renderer;
        private Material[] allMaterials;
        private Color[] baseColor;
        private float t; // Time for fading the highlight color
        private bool over; // Indicate when the mouse cursor is over the object
        private string currentText;
        private GUIStyle tooltipStyle = new GUIStyle();
        private GUIStyle tooltipStyleShadow = new GUIStyle();
        private Vector3 positionToScreen;
        private float cameraDistance;

        private void Awake()
        {
            // Get all materials and all colors for supporting multi-materials object
            _renderer = GetComponent<Renderer>();
            allMaterials = _renderer.materials;
            baseColor = new Color[allMaterials.Length];
            int temp_length = baseColor.Length;
            for (int i = 0; i < temp_length; i++)
            {
                baseColor[i] = allMaterials[i].color;
            }
        }

        // Initialization
        private void Start()
        {
            // Start settings of the tooltip
            if (!hideTooltip)
            { // Tooltip text style customization
                if (tooltipUIPanel != null)
                { // Initialization of the UI Panel
                    tooltipUIPanel.SetActive(false);
                }
                tooltipStyle.normal.textColor = tooltipColor; // Color of the tooltip text
                tooltipStyleShadow.normal.textColor = Consts.Tooltip.TOOLTIP_SHADOW_COLOR; // Color of the tooltip shadow
                tooltipStyle.fontSize = tooltipStyleShadow.fontSize = tooltipSize; // Size of the tooltip font
                tooltipStyle.fontStyle = tooltipStyleShadow.fontStyle = FontStyle.Bold; // Style of the tooltip font
                tooltipStyle.font = tooltipStyleShadow.font = tooltipFont;
                switch (tooltipAlignment)
                { // Alignment of the tooltip text
                    case TooltipAlignment.Center:
                        tooltipStyle.alignment = tooltipStyleShadow.alignment = TextAnchor.UpperCenter;
                        break;
                    case TooltipAlignment.Left:
                        tooltipStyle.alignment = tooltipStyleShadow.alignment = TextAnchor.UpperLeft;
                        break;
                    case TooltipAlignment.Right:
                        tooltipStyle.alignment = tooltipStyleShadow.alignment = TextAnchor.UpperRight;
                        break;
                    default:
                        break;
                }
            }
        }

        // Update once per frame
        private void Update()
        {
            if (over && t < 1f)
            { // Fade in hightlight color
                foreach (Material material in allMaterials)
                {
                    material.color = Color.Lerp(material.color, highlightColor, t);
                }
                t += highlightSpeed * Time.deltaTime;
            }
            else if (!over && t < 1f)
            { // Fade out highlight color
                foreach (Material material in allMaterials)
                {
                    material.color = Color.Lerp(material.color, baseColor[System.Array.IndexOf(allMaterials, material)], t);
                }
                t += highlightSpeed * Time.deltaTime;
            }
        }

        // Called when mouse over this object
        private void OnMouseEnter()
        {
            show_Tooltip();
        }

        // Called when mouse exit this object
        private void OnMouseExit()
        {
            hide_Tooltip();
        }

        // Tooltip creation
        private void OnGUI()
        {
            // Display of text/tooltip that follows the mouse
            if (!hideTooltip && !fixedToTheObject && over)
            {
                if (textResized)
                {
                    cameraDistance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
                    tooltipStyle.fontSize = tooltipStyleShadow.fontSize = Mathf.RoundToInt(tooltipSize - (cameraDistance / 3));
                }
                GUI.Label(new Rect(Event.current.mousePosition.x + tooltipPosition.x - 40f, Event.current.mousePosition.y + tooltipPosition.y, 100f, 20f), image, tooltipStyle);
                GUI.Label(new Rect(Event.current.mousePosition.x + tooltipPosition.x - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.x, Event.current.mousePosition.y + tooltipPosition.y - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.y, 100f, 20f), currentText, tooltipStyleShadow);
                GUI.Label(new Rect(Event.current.mousePosition.x + tooltipPosition.x, Event.current.mousePosition.y + tooltipPosition.y, 100f, 20f), currentText, tooltipStyle);
                if (tooltipUIPanel != null)
                {
                    tooltipUIPanel.transform.localPosition = new Vector3(Event.current.mousePosition.x + tooltipPosition.x - Screen.width / 2f, -Event.current.mousePosition.y + tooltipPosition.y + Screen.height / 2f, 0);
                }
            }
            // Display of text/tooltip that fixed to the object
            else if (!hideTooltip && fixedToTheObject && over)
            {
                positionToScreen = Camera.main.WorldToScreenPoint(transform.position);
                cameraDistance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
                if (textResized)
                {
                    tooltipStyle.fontSize = tooltipStyleShadow.fontSize = Mathf.RoundToInt(tooltipSize - (cameraDistance / 3));
                }
                //GUI.DrawTexture(new Rect((positionToScreen.x + tooltipPosition.x) - 40f, -positionToScreen.y + Screen.height + tooltipPosition.y / (cameraDistance / 10), 100f, 20f), image, ScaleMode.ScaleToFit);
                GUI.Label(new Rect(positionToScreen.x + tooltipPosition.x - 40f, -positionToScreen.y + Screen.height + tooltipPosition.y / (cameraDistance / 10), 100f, 20f), image, tooltipStyle);
                GUI.Label(new Rect(positionToScreen.x + tooltipPosition.x - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.x, -positionToScreen.y + Screen.height + tooltipPosition.y / (cameraDistance / 10) - Consts.Tooltip.TOOLTIP_SHADOW_POSITION.y, 100f, 20f), currentText, tooltipStyleShadow);
                GUI.Label(new Rect(positionToScreen.x + tooltipPosition.x, -positionToScreen.y + Screen.height + tooltipPosition.y / (cameraDistance / 10), 100f, 20f), currentText, tooltipStyle);
                if (tooltipUIPanel != null)
                {
                    tooltipUIPanel.transform.localPosition = new Vector3(positionToScreen.x + tooltipPosition.x - Screen.width / 2f, positionToScreen.y - Screen.height / 2f + tooltipPosition.y / (cameraDistance / 10), positionToScreen.z);
                }
            }
        }

        // Show tooltip and focus color of this object
        public void show_Tooltip()
        {
            t = 0f;
            over = true;
            currentText = tooltipText;

            if (tooltipUIPanel != null)
            {
                Invoke("tooltipDelayOn", 0.05f);
            }
        }

        // Hide tooltip and focus color of this object
        public void hide_Tooltip()
        {
            t = 0f;
            over = false;
            currentText = "";

            if (tooltipUIPanel != null)
            {
                Invoke("tooltipDelayOff", 0.05f);
            }
        }

        // Delay for the tooltip display (on)
        private void tooltipDelayOn()
        {
            tooltipUIPanel.SetActive(true);
        }

        // Delay for the tooltip display (off)
        void tooltipDelayOff()
        {
            tooltipUIPanel.SetActive(false);
        }
    }
}
