using UnityEngine;

namespace Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        [Tooltip("The radius around the object that stops character going inside it")]
        [SerializeField] private float radius;

        public float Radius => radius;

        public void SetRadius(float value) => radius = value;

        [Tooltip("Is this interactable currently being focused?")]
        [SerializeField] private bool isFocus;

        [Tooltip("Have we already interacted with the object?")]
        [SerializeField] private bool hasInteracted;

        public bool HasInteracted => hasInteracted;

        [SerializeField] private Transform player;    // Reference to the player transform

        private void Update()
        {
            if (isFocus)    // If currently being focused
            {
                float distance = Vector3.Distance(player.position, transform.position);
                // If we haven't already interacted and the player is close enough
                if (!hasInteracted && distance <= radius)
                {
                    // Interact with the object
                    hasInteracted = true;
                    Interact();
                }
            }
        }

        /// <summary>
        /// Called when the object starts being focused
        /// </summary>
        public void OnFocused(Transform playerTransform)
        {
            isFocus = true;
            hasInteracted = false;
            player = playerTransform;
        }

        /// <summary>
        /// Called when the object is no longer focused
        /// </summary>
        public void OnDefocused()
        {
            isFocus = false;
            hasInteracted = false;
            player = null;
        }

        /// <summary>
        /// This method is meant to be overwritten for each interactable objects 
        /// (for example: OPEN if it was a "crate" or CHOP if it was a "tree"...)
        /// </summary>
        public abstract void Interact();

        /// <summary>
        /// Draw a sphere around object
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
