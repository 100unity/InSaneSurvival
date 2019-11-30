using System;
using Managers;
using UI.Menus;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public delegate void PlayerPositionChanged(Vector3 newPosition);
        
        [Tooltip("The clickable layer. Defines where the player can click/move")] [SerializeField]
        private LayerMask moveClickLayers;

        [Tooltip("An effect that will be displayed whenever the player clicks to move")] [SerializeField]
        private GameObject clickEffect;

        [Tooltip("The speed with which the player can rotate the camera around the character")] [SerializeField]
        private float cameraRotationSpeed;

        [Tooltip("Whether the rotation of the camera with the mouse should be flipped")] [SerializeField]
        private bool invertRotation;

        [Tooltip("The distance of the camera to the user")] [SerializeField]
        private Vector2 cameraDistance;

        [Tooltip("The min- and max-distance of the camera")] [SerializeField]
        private Range cameraDistanceRange;
        
        public static event PlayerPositionChanged OnPlayerPositionUpdated;
        
        //Component references
        private NavMeshAgent _navMeshAgent;
        private Camera _camera;
        private Controls _controls;

        /// <summary>
        /// The current horizontal angle of the camera, relative to the player
        /// </summary>
        private float _cameraAngleX;

        /// <summary>
        /// The current position of the camera, relative to the player
        /// </summary>
        private Vector3 _cameraPosition;

        /// <summary>
        /// Gets the camera and the NavMeshAgent component and sets up the controls
        /// </summary>
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            SetUpControls();
            PauseMenu.OnPause += OnPause;
        }

        /// <summary>
        /// Update camera once, and enable correct controls
        /// </summary>
        private void OnEnable()
        {
            _camera = Camera.main;
            if (_camera == null) Debug.LogError("No main camera found");
            
            UpdateCameraAngle();
            _controls.PlayerControls.Enable();
            _controls.PauseMenuControls.Disable();
        }
        
        /// <summary>
        /// Player disabled -> Disable all input
        /// </summary>
        private void OnDisable() => _controls.Disable();

        /// <summary>
        /// If the player gets destroyed we need to dispose the controls. Otherwise the events will stay and add up.
        /// </summary>
        private void OnDestroy() => _controls.Dispose();

        /// <summary>
        /// Moves the camera with the player
        /// </summary>
        private void Update()
        {
            // Set camera position and look angle
            Vector3 playerPosition = transform.position;
            _camera.transform.position = playerPosition + _cameraPosition;
            _camera.transform.LookAt(playerPosition);
        }

        /// <summary>
        /// This will set up all event for the player-controls
        /// </summary>
        private void SetUpControls()
        {
            _controls = new Controls();
            _controls.PlayerControls.Move.performed += Move;
            _controls.PlayerControls.RotateCamera.performed += RotateCamera;
            _controls.PlayerControls.Zoom.performed += Zoom;
            _controls.PlayerControls.Pause.performed += TogglePause;

            _controls.PauseMenuControls.ExitPause.performed += TogglePause;
        }

        /// <summary>
        /// Let's the GameManager know, that the player pressed pause.
        /// </summary>
        private void TogglePause(InputAction.CallbackContext obj) => GameManager.Instance.TogglePause();

        /// <summary>
        /// When the right mouse button is pressed, the player moves to the pressed location using a raycast and the NavMeshAgent.
        /// </summary>
        private void Move(InputAction.CallbackContext obj)
        {
            Ray clickRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            // Get ground position from mouse click
            if (Physics.Raycast(clickRay, out RaycastHit hit, 1000000, moveClickLayers))
            {
                // Set destination for nav mesh agent
                _navMeshAgent.SetDestination(hit.point);
                _navMeshAgent.isStopped = false;

                // Create click point effect
                Instantiate(clickEffect, hit.point + Vector3.up * 5, Quaternion.identity);
                
                OnPlayerPositionUpdated?.Invoke(transform.position);
            }
        }

        /// <summary>
        /// Using the delta-x of the mouse movement it will add to the <see cref="_cameraAngleX"/> when the left mouse button is pressed
        /// </summary>
        private void RotateCamera(InputAction.CallbackContext obj)
        {
            _cameraAngleX += obj.ReadValue<float>() * cameraRotationSpeed * (invertRotation ? -1 : 1);
            UpdateCameraAngle();
        }

        /// <summary>
        /// Allows the player to zoom in and out
        /// </summary>
        private void Zoom(InputAction.CallbackContext obj) => UpdateCameraAngle(obj.ReadValue<float>());

        /// <summary>
        /// Sets the distance of the camera to the player
        /// </summary>
        /// <param name="cameraDistanceChange">The increase/decrease of the camera distance</param>
        private void UpdateCameraAngle(float cameraDistanceChange = 0)
        {
            float radian = (float) Math.PI * _cameraAngleX / 180;
            cameraDistance.y = Mathf.Clamp(cameraDistance.y + cameraDistanceChange, cameraDistanceRange.min,
                cameraDistanceRange.max);
            _cameraPosition = new Vector3((float) Math.Sin(radian) * cameraDistance.x, cameraDistance.y,
                (float) -Math.Cos(radian) * cameraDistance.x);
        }

        /// <summary>
        /// Enables/Disables the correct controls if the game is paused/unpaused
        /// </summary>
        /// <param name="isPaused">Whether the game is paused</param>
        private void OnPause(bool isPaused)
        {
            if (isPaused)
            {
                _controls.PlayerControls.Disable();
                _controls.PauseMenuControls.Enable();
            }
            else
            {
                _controls.PlayerControls.Enable();
                _controls.PauseMenuControls.Disable();
            }
        }
    }
}