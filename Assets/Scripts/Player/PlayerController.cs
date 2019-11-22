using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("The clickable layer. Defines where the player can click/move")]
        [SerializeField]
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

        [Tooltip("The base damage the player deals")]
        [SerializeField]
        private int damage;

        [Tooltip("The maximum distance between player and target in order to deal damage")]
        [SerializeField]
        private int attackRange;

        [Tooltip("The time needed for an attack in seconds")]
        [SerializeField]
        private double attackTime;

        [Tooltip("The maximum difference in degrees for the player between look direction and target direction in order to perform a successful hit")]
        [SerializeField]
        private double hitRotationTolerance;

        [Tooltip("The speed the player rotates with")]
        [SerializeField]
        private int rotationSpeed;

        [Tooltip("The maximum difference in degrees for the player between look direction and target direction in order to be facing the target.")]
        [SerializeField]
        private int rotationTolerance;


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

        private float _timer;
        // can only hold IDamageables
        private GameObject _target;
        private bool _performingHit;

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

            if (_target != null)
            {
                Attack();
            }
        }

        /// <summary>
        /// This will set up all event for the player-controls
        /// </summary>
        private void SetUpControls()
        {
            _controls = new Controls();
            _controls.Game.Click.performed += OnRightClick;
            _controls.Game.RotateCamera.performed += RotateCamera;
            _controls.Game.Zoom.performed += Zoom;
        }

        private void OnRightClick(InputAction.CallbackContext obj)
        {
            Ray clickRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            // only change target / move, if not performing a hit
            if (Physics.Raycast(clickRay, out RaycastHit hit, 10000) && !_performingHit)
            {
                Debug.Log("Click");
                GameObject objectHit = hit.collider.gameObject;
                Debug.Log(objectHit.name);
                IDamageable damageable = (IDamageable)objectHit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    // implementation NOT capable of area damage
                    Debug.Log("is damageable");
                    // object is damageable --> start the attack
                    Debug.Log("Attack");
                    _target = objectHit;
                    //Attack();
                }
                // Get ground position from mouse click
                else if (Physics.Raycast(clickRay, out RaycastHit groundHit, 10000, moveClickLayers))
                {
                    // cancel possible ongoing attacks
                    _target = null;
                    Debug.Log("Move");
                    Move(groundHit);
                }
            }
        }

        private void Attack()
        {
            // ----------
            MeshRenderer gameObjectRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            // ----------

            IDamageable damageable = (IDamageable)_target.GetComponent<IDamageable>();
            float distanceToTarget = Vector3.Distance(_target.transform.position, transform.position);
            if (distanceToTarget < attackRange && !_performingHit)
            {
                // is in range
                _navMeshAgent.isStopped = true;
                Debug.Log("target reached");
                // face target
                if (FaceTarget(true))
                {
                    // faces target
                    _performingHit = true;
                    // ----------
                    Material newMaterial = new Material(Shader.Find("Standard"));
                    newMaterial.color = Color.red;
                    gameObjectRenderer.material = newMaterial;
                    // ----------
                }
            }
            else if (_performingHit)
            {
                // perform hit
                bool? success = PerformHit();
                if (success != null)
                {
                    Debug.Log("hit performed");
                    if (success == true)
                    {
                        // hit performed --> deal damage
                        damageable.Hit(damage);
                    }

                    // ----------
                    Material newMaterial = new Material(Shader.Find("Standard"));
                    newMaterial.color = new Color(0.2784f, 1, 0.2784f);
                    gameObjectRenderer.material = newMaterial;
                    // ----------

                    _performingHit = false;
                    // reset aggro independent of (un-)successful hit
                    _target = null;
                }
            }
            else
            {
                // chase target until reached
                _navMeshAgent.SetDestination(_target.transform.position);
                _navMeshAgent.isStopped = false;
            }
        }

        private bool? PerformHit()
        {
            _timer += Time.deltaTime;

            if (_timer > attackTime)
            {
                // animation finished
                _timer = 0;
                // target still in range?
                float distanceToTarget = Vector3.Distance(_target.transform.position, transform.position);
                if (distanceToTarget < attackRange)
                {
                    // check rotation as well
                    float difference;
                    FaceTarget(false, out difference);
                    Debug.Log("rot diff: " + difference);
                    return difference < hitRotationTolerance;
                }
                Debug.Log("target out of range");
                return false;
            }
            return null;
        }

        private bool FaceTarget(bool shouldTurn)
        {
            float f;
            return FaceTarget(shouldTurn, out f);
        }

        /// <summary>
        /// Rotates to face a target.
        /// Note: same in EnemyController
        /// </summary>
        private bool FaceTarget(bool shouldTurn, out float difference)
        {
            Vector3 direction = (_target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            difference = Mathf.Abs(lookRotation.eulerAngles.magnitude - transform.rotation.eulerAngles.magnitude);
            bool facesTarget = difference < rotationTolerance;
            if (!facesTarget && shouldTurn)
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            return facesTarget;
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
    }
}