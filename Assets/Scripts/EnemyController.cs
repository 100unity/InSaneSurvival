using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;

public class EnemyController : MonoBehaviour
{
    [Tooltip("The area the enemy wanders in.")]
    [SerializeField]
    private WanderArea wanderArea;

    [Tooltip("The frequency at which the NPC walks to a new point in the wander area.")]
    [SerializeField]
    private float wanderTimer;

    [Tooltip("The ground the NPC should walk on.")]
    [SerializeField]
    private LayerMask moveLayer;

    [Tooltip("The speed the NPC rotates with.")]
    [SerializeField]
    private float rotationSpeed;

    [Tooltip("The speed the NPC chases targets with.")]
    [SerializeField]
    private float runningSpeed;

    [Tooltip("Determine whether the NPC attacks and chases the player.")]
    [SerializeField]
    private bool isAggressive;

    [Tooltip("The radius around the NPC in which a player gets the aggro.")]
    [SerializeField]
    private float lookRadius;

    [Tooltip("The frequency at which the NPC walks to a new point in the wander area.")]
    [SerializeField]
    private float escapeRadius;

    [Tooltip("Show escape radius in scene view.")]
    [SerializeField]
    private bool showEscapeRadius;

    private Transform _target;
    private NavMeshAgent _agent;
    private WanderAI _wanderAI;
    private float _timer;
    private bool _isChasing;
    private float _initialSpeed;

    /// <summary>
    /// Gets the player as target and the NavMeshAgent component.
    /// </summary>
    private void OnEnable()
    {
        // init components
        _agent = GetComponent<NavMeshAgent>();
        _wanderAI = new WanderAI();

        // set target to player
        _target = PlayerManager.Instance.GetPlayer().transform;
        if (_target == null)
        {
            Debug.LogError("No player found.");
        }

        wanderArea.FreezeArea();
        _timer = wanderTimer;
        // remember initial speed
        _initialSpeed = _agent.speed;
    }

    /// <summary>
    /// Moves around randomly in the defined wander area at a defined frequency and checks for a 
    /// target near. If a target is near, starts chasing (possibly attacking) it until the aggro is lost.
    /// </summary>
    private void Update()
    {
        if (!_isChasing) //readability
            Wander();

        if (isAggressive)
        {
            // Check for a target near
            float distanceToTarget = Vector3.Distance(_target.position, transform.position);
            if (distanceToTarget <= lookRadius || _isChasing)
            {
                Chase(distanceToTarget);
            }

            // stop chasing
            if (_isChasing && distanceToTarget >= escapeRadius)
            {
                _agent.speed = _initialSpeed;
                _isChasing = false;
            }
        }
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
    /// Chases the target.
    /// </summary>
    /// <param name="distanceToTarget">The distance between NPC and its target</param>
    private void Chase(float distanceToTarget)
    {
        _isChasing = true;
        _agent.speed = runningSpeed;
        _agent.SetDestination(_target.position);

        if (distanceToTarget <= _agent.stoppingDistance)
        {
            FaceTarget();
            // attack
        }
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
            _agent.SetDestination(wanderArea.GetCenterPosition());
        }
        else
        {
            NavMeshPath wanderPath = _wanderAI.GetNextWanderPath(wanderArea, _agent, moveLayer.value);
            _agent.SetPath(wanderPath);
        }
    }

    /// <summary>
    /// Rotates to face a target.
    /// </summary>
    private void FaceTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    /// <summary>
    /// Draws wire spheres to display lookRadius, wander area and escapeRadius.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // lookRadius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

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
