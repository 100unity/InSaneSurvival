using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldElementTooltip : MonoBehaviour
{
    public bool hoverOverActive;
    public string currentText;
    [Tooltip("Position of the tooltip showed over the interacted object.")]
    public Vector2 tooltipPosition = new Vector2(-50f, 30f);
    private GUIStyle tooltipStyle = new GUIStyle();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast (ray, out hit, 10000))
        {
            if (hit.transform.tag == "Tree")
            {
                hoverOverActive = true;
            }
            else
            {
                hoverOverActive = false;
            }
            
        }
    }

    private void OnGUI()
    {
        if (hoverOverActive)
        {
            //GUI.Label(new Rect(Event.current.mousePosition.x + tooltipPosition.x - tooltipShadowPosition.x, Event.current.mousePosition.y + tooltipPosition.y - tooltipShadowPosition.y, 100f, 20f), currentText, tooltipStyle);
            GUI.Label(new Rect(Event.current.mousePosition.x + tooltipPosition.x, Event.current.mousePosition.y + tooltipPosition.y, 100f, 20f), currentText, tooltipStyle);
        }
    }



}
