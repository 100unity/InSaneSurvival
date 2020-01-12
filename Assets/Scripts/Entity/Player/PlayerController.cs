using System;
using Crafting;
using AbstractClasses;
using Interactables;
using Managers;
using UI;
using UI.Menus;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using UnityEngine.EventSystems;

namespace Entity.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : Movable
    {
        [Tooltip("The clickable layers. Defines where the player can click")] [SerializeField]
        private LayerMask clickableLayers;

        [Tooltip("Defines where the player can move")] [SerializeField]
        private LayerMask groundLayer;

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

        [Tooltip("The inventory UI to toggle when pressing the inventory key")] [SerializeField]
        private InventoryUI inventoryUI;

        [Tooltip("The crafting UI to be toggled when pressing the crafting key")] [SerializeField]
        private CraftingUI craftingUI;

        public delegate void PlayerPositionChanged(Vector3 newPosition);
        public static event PlayerPositionChanged OnPlayerPositionUpdate;

        public delegate void CameraDistanceChanged(float newDistance);
        public static event CameraDistanceChanged OnCameraDistanceChange;

        public float CameraDistance => cameraDistance.y;
        public Range CameraDistanceRange => cameraDistanceRange;

        //Component references
        private Camera _camera;
        private Controls _controls;
        private AttackLogic _attackLogic;
        private InteractLogic _interactLogic;

        /// <summary>
        /// The current horizontal angle of the camera, relative to the player
        /// </summary>
        private float _cameraAngleX;

        /// <summary>
        /// The current position of the camera, relative to the player
        /// </summary>
        private Vector3 _cameraPosition;

        /// <summary>
        /// Gets references and sets up the controls.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _attackLogic = GetComponent<AttackLogic>();
            _interactLogic = GetComponent<InteractLogic>();
            SetUpControls();
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

            PauseMenu.OnPause += OnPause;
        }

        /// <summary>
        /// Player disabled -> Disable all input
        /// </summary>
        private void OnDisable()
        {
            _controls.Disable();
            PauseMenu.OnPause -= OnPause;
        }

        /// <summary>
        /// If the player gets destroyed we need to dispose the controls. Otherwise the events will stay and add up.
        /// </summary>
        private void OnDestroy() => _controls.Dispose();

        /// <summary>
        /// Moves the camera with the player.
        /// </summary>
        protected override void Update()
        {
            base.Update();
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
            _controls.PlayerControls.Click.performed += OnRightClick;
            _controls.PlayerControls.RotateCamera.performed += RotateCamera;
            _controls.PlayerControls.Zoom.performed += Zoom;
            _controls.PlayerControls.Pause.performed += TogglePause;
            _controls.PlayerControls.Inventory.performed += ctx => inventoryUI.ToggleInventory();
            _controls.PlayerControls.Crafting.performed += ToggleCrafting;

            _controls.PauseMenuControls.ExitPause.performed += TogglePause;
        }

        /// <summary>
        /// Let's the GameManager know, that the player pressed pause.
        /// </summary>
        private void TogglePause(InputAction.CallbackContext obj) => GameManager.Instance.TogglePause();

        /// <summary>
        /// Is called on mouse click.
        /// If right clicked, checks whether clicked on a damageable object or the ground.
        /// If clicked on a damageable object, attack it. If clicked on the ground, cancel attack
        /// and move to the clicked point. If clicked on a interactable object that is not a damageable object, interact with it.
        /// </summary>
        private void OnRightClick(InputAction.CallbackContext obj)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            Ray clickRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            // only change target / move, if not performing a hit
            if (Physics.Raycast(clickRay, out RaycastHit hit, 10000, clickableLayers) &&
                _attackLogic.Status == AttackLogic.AttackStatus.None)
            {
                GameObject objectHit = hit.collider.gameObject;

                if (objectHit.TryGetComponent(out Damageable damageable))
                {
                    _interactLogic.RemoveFocus();
                    // implementation NOT capable of area damage
                    _attackLogic.StartAttack(damageable);
                }

                else if (objectHit.TryGetComponent(out Interactable interactable))
                {
                    _attackLogic.StopAttack();
                    //Set as focus
                    _interactLogic.StartInteract(interactable);
                }

                // Get ground position from mouse click
                else if (Physics.Raycast(clickRay, out RaycastHit groundHit, 10000, groundLayer))
                {
                    // cancel possible ongoing attacks
                    _attackLogic.StopAttack();
                    _interactLogic.RemoveFocus();
                    Move(groundHit);
                }
            }
        }

        /// <summary>
        /// Player moves to the pressed location using the NavMeshAgent. Click effect is created.
        /// <param name="hit">The point on the ground the player should move to.</param>
        /// </summary>
        private void Move(RaycastHit hit)
        {
            base.Move(hit.point);
            // Create click point effect
            Instantiate(clickEffect, hit.point, Quaternion.identity);

			OnPlayerPositionUpdate?.Invoke(transform.position);
        }

        /// <summary>
        /// Using the delta-x of the mouse movement it will add to the <see cref="_cameraAngleX"/> when the left mouse button is pressed
        /// </summary>
        private void RotateCamera(InputAction.CallbackContext obj)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            _cameraAngleX += obj.ReadValue<float>() * cameraRotationSpeed * (invertRotation ? -1 : 1);
            UpdateCameraAngle();
        }

        /// <summary>
        /// Allows the player to zoom in and out
        /// </summary>
        private void Zoom(InputAction.CallbackContext obj) => UpdateCameraAngle(obj.ReadValue<float>());

        /// <summary>
        /// Shows/Hides the crafting menu
        /// </summary>
        private void ToggleCrafting(InputAction.CallbackContext obj) => craftingUI.Toggle();

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
            OnCameraDistanceChange?.Invoke(_cameraPosition.y);
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