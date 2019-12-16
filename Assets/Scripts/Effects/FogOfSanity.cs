using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.WSA.Input;

public class FogOfSanity : MonoBehaviour
{
    [SerializeField][Tooltip("Reference to the fog generating mesh")]private Renderer fogOfSanityMesh;
    [SerializeField][Tooltip("Reference to the fog generating mesh")]private Camera mainCamera;
   
    private static readonly int PlayerPosition = Shader.PropertyToID("_PlayerPosition");
    private static readonly int FogRadius = Shader.PropertyToID("_FogRadius");

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector3 cameraPosition = mainCamera.WorldToScreenPoint(transform.position);
        Ray rayToPlayer = mainCamera.ScreenPointToRay(cameraPosition);

        if (Physics.Raycast(rayToPlayer, out RaycastHit hit, 1000))
        {
            fogOfSanityMesh.material.SetVector(PlayerPosition, hit.point);
        }
        
        Pulse(20);

    }

    private void Pulse(float intensity)
    {
        float baseRadius = fogOfSanityMesh.material.GetFloat(FogRadius);
        float currentRadius = fogOfSanityMesh.material.GetFloat(FogRadius);
        bool grow = true;

        while (true)
        {
            if (grow && currentRadius <= baseRadius + intensity)
            {
                fogOfSanityMesh.material.SetFloat(FogRadius, currentRadius);
                currentRadius += 0.1F;
            }
            else grow = false;

            if (!grow && currentRadius >= baseRadius)
            {
                fogOfSanityMesh.material.SetFloat(FogRadius, currentRadius);
                currentRadius-= 0.1F;
            }
            else grow = true;
        }
    }
}
