using UnityEngine;
using UnityEngine.UI;

public class TooltipDisplay : MonoBehaviour
{
    [Tooltip("The text we want to show")]
    [SerializeField] public string myString;

    [Tooltip("Reference to tooltip UI element")]
    [SerializeField] public Canvas tooltip;

    [Tooltip("Reference to Text UI element")]
    [SerializeField] public Text myText;

    [Tooltip("Determine if we want to display the UI or not")]
    [SerializeField] public bool displayInfo;

    private void Start()
    {
        myText = myText.GetComponent<Text>();
        tooltip = tooltip.GetComponent<Canvas>();
        tooltip.enabled = false;
    }

    private void Update()
    {
        ShowTooltip();
    }

    private void OnMouseOver()
    {
        displayInfo = true;
    }

    private void OnMouseExit()
    {
        displayInfo = false;
    }

    /// <summary>
    /// This function will cause the Tooltip fade in and out
    /// </summary>
    void ShowTooltip()
    {
        if (displayInfo)
        {
            myText.text = myString;
            tooltip.enabled = true;
        }
        else
        {
            tooltip.enabled = false;
        }
    }
}

