using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.WSA.Input;

public class FogOfSanity : MonoBehaviour
{
    [SerializeField][Tooltip("Reference to the fog generating mesh")]
    private Renderer fogOfSanityMesh;
    
    [SerializeField][Tooltip("Reference to the fog generating mesh")]
    private Camera mainCamera;

    [SerializeField] [Tooltip("Intensity of the pulse animation")]
    private float pulseIntensity;
    
    [SerializeField] [Tooltip("Determines how fast pulse animation is")]
    private float pulseFrequency;

    private float _currentRadius;
    private float _baseRadius;
    private bool _isGrowing;
   
    private static readonly int PlayerPosition = Shader.PropertyToID("_PlayerPosition");
    private static readonly int FogRadius = Shader.PropertyToID("_FogRadius");

   
    // Sets initial values for the fog animation 
    private void Awake()
    {
        _baseRadius = fogOfSanityMesh.material.GetFloat(FogRadius);
        _currentRadius = _baseRadius;
        _isGrowing = true;
    }

    // fog center sticks to player
    private void Update()
    {
        Vector3 cameraPosition = mainCamera.WorldToScreenPoint(transform.position);
        Ray rayToPlayer = mainCamera.ScreenPointToRay(cameraPosition);

        if (Physics.Raycast(rayToPlayer, out RaycastHit hit, 1000))
        {
            fogOfSanityMesh.material.SetVector(PlayerPosition, hit.point);
        }
        
        Pulse(pulseIntensity);
    }

    // animates the fog radius
    private void Pulse(float intensity)
    {
        fogOfSanityMesh.material.SetFloat(FogRadius, _currentRadius);
        
        if (_isGrowing)
        {
            _currentRadius += Time.deltaTime * pulseFrequency; 
            if (_currentRadius >= _baseRadius + intensity) _isGrowing = false;
        }
        else
        {
            _currentRadius -= Time.deltaTime * pulseFrequency;
            if (_currentRadius <= _baseRadius) _isGrowing = true;
        }
    }
}
