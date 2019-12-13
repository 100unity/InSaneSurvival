using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tooltip : MonoBehaviour
{
    [Tooltip("Color change of the interacted object.")]
    public Color interactionColor = new Color(0.38f, 0.97f, 0.44f);

    [Tooltip("Fade speed of the color change (slow -> quick)")]
    public float interactionSpeed = 4f;

    [Tooltip("Show a text over the interacted object.")]
    public bool showTooltip = true;

    [Tooltip("Show a predefined UI Panel over the interacted object.")]
    public GameObject tooltipUIPanel;

    [Tooltip("Show the tooltip over the object instead of over the mouse.")]
    public bool fixedToTheObject = true;

    [Tooltip("Position of the tooltip showed over the interacted object.")]
    public Vector2 tooltipPosition = new Vector2(-50f, 30f);

    [Tooltip("Text to show over the interacted object.")]
    public string tooltipText = "";

    [Tooltip("Color of the text showed over the interacted object.")]
    public Color tooltipColor = new Color(0.9f, 0.9f, 0.9f);

    [Tooltip("Size of the text showed over the interacted object.")]
    public int tooltipSize = 20;

    [Tooltip("Resize the text, relative to the distance between the object and the camera.")]
    public bool textResized = false;

    [Tooltip("Font of the text showed over the interacted object.")]
    public Font tooltipFont;

    public enum TooltipAlignment { Center, Left, Right }

    [Tooltip("Alignment of the text showed over the interacted object.")]
    public TooltipAlignment tooltipAlignment;

    [Tooltip("Color of the text shadow showed over the interacted object.")]
    public Color tooltipShadowColor = new Color(0.1f, 0.1f, 0.1f);

    [Tooltip("Position of the text shadow showed over the interacted object.")]
    public Vector2 tooltipShadowPosition = new Vector2(-2f, -2f);

    [Tooltip("Texture for the icon beside tooltip")]
    public Texture image;

    // Reference Components
    private Renderer render;
    private Material[] allMaterials;
    private Color[] baseColor;
    private float t = 0f;
    private bool over = false;
    private string currentText = "";
    private GUIStyle tooltipStyle = new GUIStyle();
    private GUIStyle tooltipStyleShadow = new GUIStyle();
    private Vector3 positionToScreen;
    private float cameraDistance;


    // Initialization
    private void Start()
    {
        // Get all materials and all colors for supporting multi-materials object
        render = GetComponent<Renderer>();
        allMaterials = render.materials;
        baseColor = new Color[allMaterials.Length];
        int temp_length = baseColor.Length;
        for (int i = 0; i < temp_length; i++)
        {
            baseColor[i] = allMaterials[i].color;
        }

        // Start settings of the tooltip
        if (showTooltip)
        { // Tooltip text style customization
            if (tooltipUIPanel != null)
            { // Initialization of the UI Panel
                tooltipUIPanel.SetActive(false);
            }
            tooltipStyle.normal.textColor = tooltipColor; // Color of the tooltip text
            tooltipStyleShadow.normal.textColor = tooltipShadowColor; // Color of the tooltip shadow
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
        if (over && t < 1)
        { // Fade of the interaction enter color
            foreach (Material material in allMaterials)
            {
                material.color = Color.Lerp(material.color, interactionColor, t);
            }
            t += interactionSpeed * Time.deltaTime;
        }
        else if (!over && t < 1f)
        { // Fade of the interaction exit color
            foreach (Material material in allMaterials)
            {
                material.color = Color.Lerp(material.color, baseColor[System.Array.IndexOf(allMaterials, material)], t);
            }
            t += interactionSpeed * Time.deltaTime;
        }
    }

    // Called when mouse over this object
    private void OnMouseEnter()
    {
        interaction_enter();
    }

    // Called when mouse exit this object
    private void OnMouseExit()
    {
        interaction_exit();
    }

    // Tooltip creation
    private void OnGUI()
    {
        // Display of text/tooltip that follows the mouse
        if (showTooltip && !fixedToTheObject && over)
        {
            if (textResized)
            {
                cameraDistance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
                tooltipStyle.fontSize = tooltipStyleShadow.fontSize = Mathf.RoundToInt(tooltipSize - (cameraDistance / 3));
            }
            GUI.Label(new Rect(Event.current.mousePosition.x + tooltipPosition.x - 40, Event.current.mousePosition.y + tooltipPosition.y, 100f, 20f), image, tooltipStyle);
            GUI.Label(new Rect(Event.current.mousePosition.x + tooltipPosition.x - tooltipShadowPosition.x, Event.current.mousePosition.y + tooltipPosition.y - tooltipShadowPosition.y, 100f, 20f), currentText, tooltipStyleShadow);
            GUI.Label(new Rect(Event.current.mousePosition.x + tooltipPosition.x, Event.current.mousePosition.y + tooltipPosition.y, 100f, 20f), currentText, tooltipStyle);
            if (tooltipUIPanel != null)
            {
                tooltipUIPanel.transform.localPosition = new Vector3(Event.current.mousePosition.x + tooltipPosition.x - Screen.width / 2f, -Event.current.mousePosition.y + tooltipPosition.y + Screen.height / 2f, 0);
            }

            // Display of text/tooltip that fixed to the object
        }
        else if (showTooltip && fixedToTheObject && over)
        {
            positionToScreen = Camera.main.WorldToScreenPoint(transform.position);
            cameraDistance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
            if (textResized)
            {
                tooltipStyle.fontSize = tooltipStyleShadow.fontSize = Mathf.RoundToInt(tooltipSize - (cameraDistance / 3));
            }
            //GUI.DrawTexture(new Rect((positionToScreen.x + tooltipPosition.x) - 40f, -positionToScreen.y + Screen.height + tooltipPosition.y / (cameraDistance / 10), 100f, 20f), image, ScaleMode.ScaleToFit);
            GUI.Label(new Rect(positionToScreen.x + tooltipPosition.x - 40, -positionToScreen.y + Screen.height + tooltipPosition.y / (cameraDistance / 10), 100f, 20f), image, tooltipStyle);
            GUI.Label(new Rect(positionToScreen.x + tooltipPosition.x - tooltipShadowPosition.x, -positionToScreen.y + Screen.height + tooltipPosition.y / (cameraDistance / 10) - tooltipShadowPosition.y, 100f, 20f), currentText, tooltipStyleShadow);
            GUI.Label(new Rect(positionToScreen.x + tooltipPosition.x, -positionToScreen.y + Screen.height + tooltipPosition.y / (cameraDistance / 10), 100f, 20f), currentText, tooltipStyle);
            if (tooltipUIPanel != null)
            {
                tooltipUIPanel.transform.localPosition = new Vector3(positionToScreen.x + tooltipPosition.x - Screen.width / 2f, positionToScreen.y - Screen.height / 2f + tooltipPosition.y / (cameraDistance / 10), positionToScreen.z);
            }
        }
    }

    // Begin the interaction system (show tooltip and focus color of this object)
    public void interaction_enter()
    {
        t = 0f;
        over = true;
        currentText = tooltipText;

        if (tooltipUIPanel != null)
        {
            Invoke("tooltipDelayOn", 0.05f);
        }
    }

    // End the interaction system (hide tooltip and focus color of this object)
    public void interaction_exit()
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
