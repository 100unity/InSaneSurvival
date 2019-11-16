using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Tooltip("The radius around the NPC in which a player gets the aggro.")] [SerializeField]
    private float lookRadius;

    [Tooltip("The speed the NPC rotates with.")] [SerializeField]
    private float rotationSpeed;

    private Transform _target;
    private NavMeshAgent _agent;

    /// <summary>
    /// Gets the player as target and the NavMeshAgent component.
    /// </summary>
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = PlayerManager.Instance.GetPlayer().transform;
        if (_target == null)
        {
            Debug.LogError("No player found.");
        }
    }

    /// <summary>
    /// Moves around randomly and checks for a target near. If a target is near,
    /// follows it to the position the target left the lookRadius of the NPC.
    /// In the stopping distance of the NavMeshAgent it rotates to face the target.
    /// </summary>
    private void Update()
    {
        // Move randomly


        // Check for a target near
        float distance = Vector3.Distance(_target.position, transform.position);

        if (distance <= lookRadius)
        {
            _agent.SetDestination(_target.position);
    
            if (distance <= _agent.stoppingDistance)
            {
                FaceTarget();
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
