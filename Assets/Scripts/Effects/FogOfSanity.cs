using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfSanity : MonoBehaviour
{
    [SerializeField][Tooltip("Reference to the fog generating mesh")]private Transform fogOfSanityMesh;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(transform.position);
        Ray rayToPlayerPosition = Camera.main.ScreenPointToRay(cameraPosition);

        RaycastHit hit;
        if (Physics.Raycast(rayToPlayerPosition, out hit, 1000))
        {
            fogOfSanityMesh.GetComponent<Renderer>().material.SetVector("_PlayerPosition", hit.point);
        }

    }
}
