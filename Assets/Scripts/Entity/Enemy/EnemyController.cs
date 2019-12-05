using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Entity.Enemy
{
    public class EnemyController : MonoBehaviour, IMovable
    {
        [Tooltip("The area the enemy wanders in.")] [SerializeField]
        private WanderArea wanderArea;

        [Tooltip("The frequency at which the NPC walks to a new point in the wander area.")] [SerializeField]
        private float wanderTimer;

        [Tooltip("Whether the area should be fixed on enable.")] [SerializeField]
        public bool freezeArea;

        [Tooltip("The ground the NPC should walk on.")] [SerializeField]
        private LayerMask moveLayer;

        [Header("Attacking")]
        [Tooltip("Determine whether the NPC attacks and chases the player.")] [SerializeField]
        private bool isAggressive;
        
        [Tooltip("The speed the NPC chases targets with.")] [SerializeField]
        private float runningSpeed;
        
        [Tooltip("The speed the NPC rotates with.")] [SerializeField]
        private float rotationSpeed;
        
        [Tooltip(
            "The maximum difference in degrees for the NPC between look direction and target direction in order to be facing the target.")]
        [SerializeField]
        private float rotationTolerance;
        
        [Tooltip("The radius around the NPC in which a player can escape.")] [SerializeField]
        private float escapeRadius;

        [Tooltip("Show escape radius in scene view.")] [SerializeField]
        private bool showEscapeRadius;

        [Tooltip("TargetFinderComponent - Finds nearby targets")] [SerializeField]
        private TargetFinder targetFinder;
        
        
        
        // component references
        private NavMeshAgent _agent;

        private WanderAI _wanderAI;
        private AttackLogic _attackLogic;

        // hold player object to check distance
        private float _timer;
        private bool _isChasing;
        private float _initialSpeed;

        private void Awake()
        {
            // init components
            _agent = GetComponent<NavMeshAgent>();
            _wanderAI = new WanderAI();
            _attackLogic = GetComponent<AttackLogic>();
            if (_attackLogic == null)
                Debug.LogError("NPC is not able to attack without an AttackLogic component.");
        }

        /// <summary>
        /// Gets the player object and freezes wander area if necessary.
        /// </summary>
        private void OnEnable()
        {
            if (freezeArea)
                wanderArea.FreezeArea();
            _timer = wanderTimer;
            // remember initial speed
            _initialSpeed = _agent.speed;
        }

        /// <summary>
        /// Moves around randomly in the defined wander area at a defined frequency and checks for an attackable 
        /// target near (which is the player for now). If the player is near, starts chasing (and attacking) it until the aggro is lost.
        /// </summary>
        private void Update()
        {
            if (!_isChasing)
                Wander();

            if (!isAggressive || targetFinder.Targets.Count == 0) return;

            // Attack first target in list
            GameObject currentTarget = targetFinder.Targets[0];

            if (!_isChasing)
                StartChasing(currentTarget);

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            // stop chasing when player out of escapeRadius and is not performing a hit right now (and is chasing)
            if (distance >= escapeRadius && _attackLogic.Status == AttackLogic.AttackStatus.None && _isChasing)
                StopChasing();
        }

        // ----- Note: same in PlayerController --> make IMovable abstract class and inherit? ------

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
        /// Sets destination for NavMesh agent and starts moving.
        /// </summary>
        /// <param name="position">The position to move to</param>
        public void Move(Vector3 position)
        {
            _agent.SetDestination(position);
            _agent.isStopped = false;
        }

        /// <summary>
        /// Stops moving.
        /// </summary>
        public void StopMoving()
        {
            _agent.isStopped = true;
        }


        /// <summary>
        /// Wanders in wander area.
        /// </summary>
        private void Wander()
        {
            _timer += Time.deltaTime;
            if (_timer >= wanderTimer)
            {
                MoveToNextPoint();
                _timer = 0;
            }
        }

        /// <summary>
        /// Starts attacking and chasing the target.
        /// </summary>
        /// <param name="target">The target to attack</param>
        private void StartChasing(GameObject target)
        {
            _isChasing = true;
            _agent.speed = runningSpeed;
            _attackLogic.StartAttack(target);
        }

        /// <summary>
        /// Stops chasing and attack the current target.
        /// </summary>
        private void StopChasing()
        {
            targetFinder.Targets.RemoveAt(0);
            _isChasing = false;
            _agent.speed = _initialSpeed;
            _attackLogic.StopAttack();
        }

        /// <summary>
        /// Starts moving to a new random point in the wander area or goes back to wander area if outside.
        /// </summary>
        private void MoveToNextPoint()
        {
            // move back to wander area after chasing, also in this case:
            /* at very high speed and acceleration the NPC can get out of the wander area because 
             * the NavMeshAgent behaves in a physical manner (inertia) */
            if (!_wanderAI.IsInArea(wanderArea, transform.position))
            {
                Move(wanderArea.GetCenterPosition());
            }
            else
            {
                // don't use SetDestination but set calculated path
                NavMeshPath wanderPath = _wanderAI.GetNextWanderPath(wanderArea, _agent, moveLayer.value);
                _agent.SetPath(wanderPath);
                _agent.isStopped = false;
            }
        }

        /// <summary>
        /// Draws wire spheres to display lookRadius, wander area and escapeRadius.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // wanderArea
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(wanderArea.GetCenterPosition(), wanderArea.radius);

            // escapeRadius
            if (showEscapeRadius)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, escapeRadius);
            }
        }

        /* maybe this is useful at another place?
            // restrict walkable area
            Vector3 newLocation = transform.position;
            float walkDistance = Vector3.Distance(newLocation, _centerPosition);
    
            if (walkDistance > patrolRadius)
            {
                Vector3 fromCenterToPosition = newLocation - _centerPosition;
                fromCenterToPosition = fromCenterToPosition * (patrolRadius / walkDistance);
                newLocation = _centerPosition + fromCenterToPosition;
                gameObject.transform.position = newLocation;
            }*/
    }
}