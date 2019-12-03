using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    bool isFocus = false;   // Is this interactable currently being focused?
    Transform player;       // Reference to the player transform

    bool hasInteracted = false; // Have we already interacted with the object?
=======
public abstract class Interactable : MonoBehaviour
{
    [Tooltip("The radius around the object that stops character going inside it")]
    [SerializeField] public float radius = 3f;

    [Tooltip("Is this interactable currently being focused?")]
    bool isFocus = false;

    [Tooltip("Have we already interacted with the object?")]
    bool hasInteracted = false;

    Transform player;    // Reference to the player transform

>>>>>>> Stashed changes

    void Update()
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

<<<<<<< Updated upstream
    // Called when the object starts being focused
=======
    /// <summary>
    /// Called when the object starts being focused
    /// </summary>
>>>>>>> Stashed changes
    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        hasInteracted = false;
        player = playerTransform;
    }

<<<<<<< Updated upstream
    // Called when the object is no longer focused
=======
    /// <summary>
    /// Called when the object is no longer focused
    /// </summary>
>>>>>>> Stashed changes
    public void OnDefocused()
    {
        isFocus = false;
        hasInteracted = false;
        player = null;
    }

<<<<<<< Updated upstream
    // This method is meant to be overwritten
=======
    /// <summary>
    /// This method is meant to be overwritten for each interactable objects (for example: OPEN if it's a crate or CHOP wood if it's a tree...)
    /// </summary>
>>>>>>> Stashed changes
    public virtual void Interact()
    {
        //Do something
    }

<<<<<<< Updated upstream
=======
    /// <summary>
    /// Draw a sphere around object
    /// </summary>
>>>>>>> Stashed changes
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
