﻿using System;
using Interfaces;
using Managers;
using UI.Menus;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour, IMovable
    {
        [Tooltip("The clickable layer. Defines where the player can click/move")] [SerializeField]
        private LayerMask moveClickLayers;

        [Tooltip("An effect that will be displayed whenever the player clicks to move")]
        [SerializeField]
        private GameObject clickEffect;

        [Tooltip("The speed with which the player can rotate the camera around the character")]
        [SerializeField]
        private float cameraRotationSpeed;

        [Tooltip("Whether the rotation of the camera with the mouse should be flipped")]
        [SerializeField]
        private bool invertRotation;

        [Tooltip("The distance of the camera to the user")]
        [SerializeField]
        private Vector2 cameraDistance;

        [Tooltip("The min- and max-distance of the camera")]
        [SerializeField]
        private Range cameraDistanceRange;

        [Tooltip("The speed the player rotates with")]
        [SerializeField]
        private int rotationSpeed;

        [Tooltip("The maximum difference in degrees for the player between look direction and target direction in order to be facing the target.")]
        [SerializeField]
        private int rotationTolerance;
        
        [Tooltip("The inventory UI to toggle when pressing the inventory key")]
        [SerializeField]
        private InventoryUI inventoryUI;

        [Tooltip("Showing the object player is focusing")] [SerializeField]
        public Interactable focus;

        [Tooltip("Showing the object target player is following")] [SerializeField]
        public Transform target;

        public delegate void PlayerPositionChanged(Vector3 newPosition);

        public static event PlayerPositionChanged OnPlayerPositionUpdated;
        
        //Component references
        private NavMeshAgent _navMeshAgent;
        private Camera _camera;
        private Controls _controls;
        private AttackLogic _attackLogic;

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
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _attackLogic = GetComponent<AttackLogic>();
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
        /// Moves the camera with the player.
        /// </summary>
        private void Update()
        {
            // Set camera position and look angle
            Vector3 playerPosition = transform.position;
            _camera.transform.position = playerPosition + _cameraPosition;
            _camera.transform.LookAt(playerPosition);

            // Check and makes character facing the target object when moving toward it
            if (target != null) FaceTarget();
        }

        /// <summary>
        /// These are functions for character to interact with all objects in the game (ex: moving, following, pick up items, open crate, etc..) using raycast and _NavMeshAgent
        /// (Won't conflict with character attacking enemy or moving on the map)
        /// </summary>
        #region Interactables Functions
        
        // This will make character moving to the target object
        public void MovingToTarget(Interactable newTarget)
        {
            _navMeshAgent.stoppingDistance = newTarget.radius * .8f;
            _navMeshAgent.updateRotation = false;
            target = newTarget.transform;
        }

        // Makes character stop moving to target object
        public void StopMovingToTarget()
        {
            _navMeshAgent.stoppingDistance = 0f;
            _navMeshAgent.updateRotation = true;
            target = null;
        }

        // Set focus when interact
        public void SetFocus(Interactable newFocus)
        {
            if(newFocus != focus)
            {
                if(focus != null) focus.OnDefocused();
                focus = newFocus;
                MovingToTarget(newFocus);
            }

            newFocus.OnFocused(transform);
        }

        // Remove focus when not interact
        public void RemoveFocus()
        {
            if (focus != null) focus.OnDefocused();
            focus = null;
            StopMovingToTarget();
        }

        // Makes character facing the target when moving toward it
        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        #endregion

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
            Ray clickRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            // only change target / move, if not performing a hit
            if (Physics.Raycast(clickRay, out RaycastHit hit, 10000) && _attackLogic.Status == AttackLogic.AttackStatus.None)
            {
                GameObject objectHit = hit.collider.gameObject;
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                IDamageable damageable = objectHit.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    // implementation NOT capable of area damage
                    _attackLogic.StartAttack(objectHit);
                }
                // Get ground position from mouse click
                else if (Physics.Raycast(clickRay, out RaycastHit groundHit, 10000, moveClickLayers))
                {
                    // cancel possible ongoing attacks
                    _attackLogic.StopAttack();
                    Move(groundHit);
                }
        
                if (interactable != null)
                {
                    //Set as focus
                    SetFocus(interactable);
                }

                else
                {
                    RemoveFocus();
                }          
            }
        }

        /// <summary>
        /// Player moves to the pressed location using the NavMeshAgent.
        /// <param name="hit">The point on the ground the player should move to.</param>
        /// </summary>
        private void Move(RaycastHit hit)
        {
            // Set destination for nav mesh agent
            _navMeshAgent.SetDestination(hit.point);
            _navMeshAgent.isStopped = false;

            // Create click point effect
            Instantiate(clickEffect, hit.point + Vector3.up * 5, Quaternion.identity);

			OnPlayerPositionUpdated?.Invoke(transform.position);
        }

        // ----- Note: same in EnemyController --> make IMovable abstract class and inherit? ------

        /// <summary>
        /// Faces the target. Returns true if facing the target.
        /// </summary>
        /// <param name="target">The target to face</param>
        /// <param name="shouldTurn">Whether the object should turn to the target or not</param>
        /// <param name="difference">The difference between object and target in degrees</param>
        /// <returns>Whether the object is facing the target</returns>
        public bool FaceTarget(GameObject target, bool shouldTurn, out float difference)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            difference = Mathf.Abs(lookRotation.eulerAngles.magnitude - transform.rotation.eulerAngles.magnitude);
            bool facesTarget = difference < rotationTolerance;
            if (!facesTarget && shouldTurn)
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            return facesTarget;
        }

        /// <summary>
        /// Move to a certain position.
        /// </summary>
        /// <param name="position">The position to move to</param>
        public void Move(Vector3 position)
        {
            // Set destination for nav mesh agent
            _navMeshAgent.SetDestination(position);
            _navMeshAgent.isStopped = false;
        }

        /// <summary>
        /// Stops moving.
        /// </summary>
        public void StopMoving()
        {
            _navMeshAgent.isStopped = true;
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
            float radian = (float)Math.PI * _cameraAngleX / 180;
            cameraDistance.y = Mathf.Clamp(cameraDistance.y + cameraDistanceChange, cameraDistanceRange.min,
                cameraDistanceRange.max);
            _cameraPosition = new Vector3((float)Math.Sin(radian) * cameraDistance.x, cameraDistance.y,
                (float)-Math.Cos(radian) * cameraDistance.x);
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