using Constants;
using UnityEngine;
using UnityEngine.AI;

namespace AbstractClasses
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Movable : MonoBehaviour
    {
        [Tooltip("The speed to rotate with")] [SerializeField]
        private int rotationSpeed;

        [Tooltip(
            "The maximum difference in degrees between look direction and target direction in order to be facing the target.")]
        [SerializeField]
        private int rotationTolerance;

        [Tooltip("The animator that should be used for movement animations")] [SerializeField]
        private Animator animator;

        [Tooltip("The ground layer to use for movement calculations")] [SerializeField]
        protected LayerMask groundLayer;

        [Tooltip("The graphics object's transform to align parallel to the ground")] [SerializeField]
        private Transform graphicsTransform;

        protected NavMeshAgent NavMeshAgent;
        private static readonly int MovementSpeed = Animator.StringToHash(Consts.Animation.MOVEMENT_SPEED_FLOAT);

        protected virtual void Awake() => NavMeshAgent = GetComponent<NavMeshAgent>();

        protected virtual void Update()
        {
            UpdateRotation();
            animator.SetFloat(MovementSpeed, NavMeshAgent.velocity.magnitude);
        }

        /// <summary>
        /// Updates the rotation of the entity so that it's always parallel to the ground it's standing on.
        /// </summary>
        private void UpdateRotation()
        {
            Quaternion oldRotation = graphicsTransform.rotation;
            
            if (!Physics.Raycast(graphicsTransform.position, Vector3.down, out RaycastHit hit, 100))
                return;
            // Ground hit, update rotation
            Quaternion newRotation = Quaternion.FromToRotation(graphicsTransform.up, hit.normal) * oldRotation;
            graphicsTransform.rotation = Quaternion.Lerp(oldRotation, newRotation, Time.deltaTime * rotationSpeed);
        }

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
            NavMeshAgent.SetDestination(position);
            NavMeshAgent.isStopped = false;
        }

        /// <summary>
        /// Stops moving.
        /// </summary>
        public void StopMoving()
        {
            NavMeshAgent.isStopped = true;
        }

        public void SetStoppingDistance(float distance)
        {
            NavMeshAgent.stoppingDistance = distance;
        }
    }
}