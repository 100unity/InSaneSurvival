using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;

public class EnemyController : MonoBehaviour
{
    [Tooltip("The radius around the NPC in which a player gets the aggro.")] [SerializeField]
    private float lookRadius;

    [Tooltip("The area the enemy wanders in.")] [SerializeField]
    private WanderArea wanderArea;

    [Tooltip("The frequency at which the NPC walks to a new point in the wander area.")] [SerializeField]
    private float wanderTimer;

    [SerializeField]
    private float escapeRadius; // TODO

    [Tooltip("The ground the NPC should walk on.")] [SerializeField]
    private LayerMask moveLayer;

    [Tooltip("The speed the NPC rotates with.")] [SerializeField]
    private float rotationSpeed;

    [Tooltip("Determine whether the NPC attacks and chases the player.")] [SerializeField]
    private bool isAggressive;

    [SerializeField]
    private bool chaseBeyondPatrolRadius; // TODO

    private Transform _target;
    private NavMeshAgent _agent;
    private WanderAI _wanderAI;
    private float _timer;
    private bool isChasing; // TODO

    /// <summary>
    /// Gets the player as target and the NavMeshAgent component.
    /// </summary>
    private void Awake() // change to onEnable TODO
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
    }

    /// <summary>
    /// Moves around randomly in the defined wander area at a defined frequency and checks for a 
    /// target near. If a target is near, follows it to the position the target left the lookRadius of the NPC.
    /// In the stopping distance of the NavMeshAgent it rotates to face the target.
    /// </summary>
    private void Update()
    {
        // move randomly
        _timer += Time.deltaTime;
        if (_timer >= wanderTimer)
        {
            NavMeshPath wanderPath = _wanderAI.GetNextWanderPath(wanderArea, _agent, moveLayer.value);
            _agent.SetPath(wanderPath);
            _timer = 0;
            //TODO very high speed and acceleration make NPC move out of circle
        }
        
        // WIP TODO
        if (isAggressive)
        {
            // Check for a target near
            float distance = Vector3.Distance(_target.position, transform.position);

            if (distance <= lookRadius)
            {
                //isChasing = true;
                _agent.SetDestination(_target.position);

                if (distance <= _agent.stoppingDistance)
                {
                    FaceTarget();
                }
            }
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
    /// Draws wire sphere to display the lookRadius.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // lookRadius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        // wanderArea
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(wanderArea.GetCenterPosition(), wanderArea.radius);
    }

    /* maybe this is useful at another place?
        // don't move beyond patrolRadius
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
