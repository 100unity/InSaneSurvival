using AbstractClasses;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Entity.Enemy
{
    public class EnemyController : Movable
    {
        [Tooltip("The area the enemy wanders in.")]
        [SerializeField]
        private Area wanderArea;

        [Tooltip("The frequency at which the NPC walks to a new point in the wander area.")]
        [SerializeField]
        private float wanderTimer;

        [Tooltip("Whether the area should be fixed on enable.")]
        [SerializeField]
        private bool freezeArea;

        [Tooltip("The speed the NPC chases targets with.")]
        [SerializeField]
        private float runningSpeed;

        [Tooltip("Determine whether the NPC attacks and chases the player.")]
        [SerializeField]
        private bool isAggressive;

        [Tooltip("The radius around the NPC in which a player can escape.")]
        [SerializeField]
        private float escapeRadius;

        [Tooltip("Show escape radius in scene view.")]
        [SerializeField]
        private bool showEscapeRadius;

        [Tooltip("TargetFinderComponent - Finds nearby targets")]
        [SerializeField]
        private TargetFinder targetFinder;

        [Tooltip("The renderer that renders the graphics of this object.")]
        [SerializeField]
        private Renderer graphicsRenderer;

        public Renderer Renderer => graphicsRenderer;

        // component references
        private WanderAI _wanderAI;
        private AttackLogic _attackLogic;
        
        private float _timer;
        private bool _isChasing;
        private float _initialSpeed;


        protected override void Awake()
        {
            base.Awake();
            // init components
            _wanderAI = new WanderAI();
            _attackLogic = GetComponent<AttackLogic>();
            if (_attackLogic == null)
                Debug.LogError("NPC is not able to attack without an AttackLogic component.");
        }

        /// <summary>
        /// Freezes wander area if necessary.
        /// </summary>
        private void OnEnable()
        {
            if (freezeArea)
                wanderArea.FreezeArea();
            _timer = wanderTimer;
            // remember initial speed
            _initialSpeed = NavMeshAgent.speed;
        }

        /// <summary>
        /// Moves around randomly in the defined wander area at a defined frequency and checks for an attackable 
        /// target near (which is the player for now). If the player is near, starts chasing (and attacking) it until the aggro is lost.
        /// </summary>
        private void Update()
        {
            if (!_isChasing)
                Wander();

            if (!isAggressive || !targetFinder.HasTarget()) return;

            // Attack first target in list
            Damageable currentTarget = targetFinder.GetFirstTarget();

            if (!_isChasing)
                StartChasing(currentTarget);

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            // stop chasing when player out of escapeRadius and is not performing a hit right now (and is chasing)
            if (distance >= escapeRadius && _attackLogic.Status == AttackLogic.AttackStatus.None && _isChasing)
                StopChasing();
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
        private void StartChasing(Damageable target)
        {
            _isChasing = true;
            NavMeshAgent.speed = runningSpeed;
            _attackLogic.StartAttack(target);
        }

        /// <summary>
        /// Stops chasing the current target.
        /// </summary>
        private void StopChasing()
        {
            targetFinder.RemoveFirstTarget();
            _isChasing = false;
            NavMeshAgent.speed = _initialSpeed;
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
                NavMeshPath wanderPath = _wanderAI.GetNextWanderPath(wanderArea, NavMeshAgent, NavMesh.AllAreas);
                NavMeshAgent.SetPath(wanderPath);
                NavMeshAgent.isStopped = false;
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
