using System;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
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

<<<<<<< Updated upstream
        [Tooltip("The inventory UI to toggle when pressing the inventory key")] [SerializeField]
        private InventoryUI inventoryUI;
        
        public delegate void PlayerPositionChanged(Vector3 newPosition);
=======
        [Tooltip("Showing the object player is focusing")] [SerializeField]
        public Interactable focus;

        [Tooltip("Showing the object target player is following")] [SerializeField]
        public Transform target;

>>>>>>> Stashed changes
        public static event PlayerPositionChanged OnPlayerPositionUpdated;
        
        //Component references
        private NavMeshAgent _navMeshAgent;
        private Camera _camera;
        private Controls _controls;

        //Interactables
        [Tooltip("Showing the object player is focusing")] [SerializeField]
        public Interactable focus;

        [Tooltip("Showing the object target player is following")] [SerializeField]
        public Transform target; //Target to follow

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
            _camera = Camera.main;
            if (_camera == null)
            {
                Debug.LogError("No main camera found");
                return;
            }

            _navMeshAgent = GetComponent<NavMeshAgent>();
            SetUpControls();
        }

        private void OnEnable()
        {
            UpdateCameraAngle();
            _controls.Enable();
        }

        private void OnDisable() => _controls.Disable();

        /// <summary>
        /// Moves the camera with the player
        /// </summary>
        private void Update()
        {
            // Set camera position and look angle
            Vector3 playerPosition = transform.position;
            _camera.transform.position = playerPosition + _cameraPosition;
            _camera.transform.LookAt(playerPosition);

            #region Interactable
            // Check and set destination of character to the currently focused Object
            if (target != null)
            {
                _navMeshAgent.SetDestination(target.position);
                FaceTarget();
            }

            // If player press right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                // Creates a ray
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If the ray hits
<<<<<<< Updated upstream
                if(Physics.Raycast(ray, out hit, 100))
=======
                if (Physics.Raycast(ray, out hit, 100))
>>>>>>> Stashed changes
                {
                    // Check if player hit an interactable
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
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
            #endregion
<<<<<<< Updated upstream
=======
        }

        /// <summary>
        /// These are functions for character to interact with all objects in the game (ex: moving, following, pick up items, open crate, etc..) using raycast and _NavMeshAgent
        /// (Won't conflict with character moving on the map)
        /// </summary>
        #region Interactables Functions
        public void FollowTarget(Interactable newTarget)
        {
            _navMeshAgent.stoppingDistance = newTarget.radius * .8f;
            _navMeshAgent.updateRotation = false;
            target = newTarget.transform;
        }

        public void StopFollowingTarget()
        {
            _navMeshAgent.stoppingDistance = 0f;
            _navMeshAgent.updateRotation = true;
            target = null;
        }

        public void SetFocus(Interactable newFocus)
        {
            if (newFocus != focus)
            {
                if (focus != null) focus.OnDefocused();
                focus = newFocus;
                FollowTarget(newFocus);
            }

            newFocus.OnFocused(transform);
        }

        public void RemoveFocus()
        {
            if (focus != null) focus.OnDefocused();
            focus = null;
            StopFollowingTarget();
        }

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
>>>>>>> Stashed changes
        }
        #endregion

        /// <summary>
        /// These are functions for character to interact with all objects in the game (ex: moving, following, pick up items, open crate, etc..) using raycast and _NavMeshAgent
        /// (Won't conflict with character moving on the map)
        /// </summary>
        #region Interactables Functions
        public void FollowTarget(Interactable newTarget)
        {
            _navMeshAgent.stoppingDistance = newTarget.radius * .8f;
            _navMeshAgent.updateRotation = false;
            target = newTarget.transform;
        }

        public void StopFollowingTarget()
        {
            _navMeshAgent.stoppingDistance = 0f;
            _navMeshAgent.updateRotation = true;
            target = null;
        }

        public void SetFocus(Interactable newFocus)
        {
            if(newFocus != focus)
            {
                if(focus != null) focus.OnDefocused();
                focus = newFocus;
                FollowTarget(newFocus);
            }

            newFocus.OnFocused(transform);
        }

        public void RemoveFocus()
        {
            if (focus != null) focus.OnDefocused();
            focus = null;
            StopFollowingTarget();
        }

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
            _controls.Game.Move.performed += Move;
            _controls.Game.RotateCamera.performed += RotateCamera;
            _controls.Game.Zoom.performed += Zoom;
            _controls.Game.Inventory.performed += ctx => inventoryUI.ToggleInventory();
        }

        /// <summary>
        /// When the right mouse button is pressed, the player moves to the pressed location using a raycast and the NavMeshAgent.
        /// </summary>
        private void Move(InputAction.CallbackContext obj)
        {
            Ray clickRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            // Get ground position from mouse click
            if (Physics.Raycast(clickRay, out RaycastHit hit, 10000, moveClickLayers))
            {
                // Set destination for nav mesh agent
                _navMeshAgent.SetDestination(hit.point);
                _navMeshAgent.isStopped = false;

                // Create click point effect
                if (focus == null)
                {
                    Instantiate(clickEffect, hit.point + Vector3.up * 5, Quaternion.identity);
                }

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

    }
}