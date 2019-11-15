using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask moveClickLayers;
    [SerializeField] private GameObject clickEffect;
    [SerializeField] private float cameraOffset;
    [SerializeField] private float cameraRotationSpeed;
    
    private NavMeshAgent _navMeshAgent;
    private Camera _camera;
    private Vector3 _previousMousePosition;
    private float _angle;
    
    private void Start()
    {
        _camera = Camera.main;
        if (_camera == null)
        {
            Debug.LogError("No main camera found");
            return;
        }
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_previousMousePosition != Vector3.zero)
            {
                var moveDelta = Input.mousePosition.x - _previousMousePosition.x;
                _angle += moveDelta * cameraRotationSpeed;
            }
            _previousMousePosition = Input.mousePosition;
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
            _previousMousePosition = Vector3.zero;
        
        // Set camera position and look angle
        var playerPosition = transform.position;
        var radian = (float) Math.PI * _angle / 180;
        var offset = new Vector3((float) Math.Sin(radian) * 15, cameraOffset, (float) -Math.Cos(radian) * 15);
        _camera.transform.position = playerPosition + offset;
        _camera.transform.LookAt(playerPosition);
    }

    private void OnMove()
    {
        var clickLocation = Mouse.current.position.ReadValue();
        var clickRay = _camera.ScreenPointToRay(clickLocation);

        // Get ground position from mouse click
        if (Physics.Raycast(clickRay, out var hit, 10000, moveClickLayers))
        {
            // Set destination for nav mesh agent
            _navMeshAgent.SetDestination(hit.point);
            _navMeshAgent.isStopped = false;
            
            // Create click point effect
            Instantiate(clickEffect, hit.point + Vector3.up * 5, Quaternion.identity);
        }
    }
}
