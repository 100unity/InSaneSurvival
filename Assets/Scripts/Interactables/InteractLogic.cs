using AbstractClasses;
using UnityEngine;

namespace Interactables
{
    public class InteractLogic : MonoBehaviour
    {
        /// <summary>
        /// These are functions for character to interact with all objects in the game (ex: moving, following, pick up items, open crate, etc..)
        /// (Won't conflict with character attacking enemy or moving on the map)
        /// </summary>
        /// 

        [Tooltip("Showing the object player is focusing")]
        private Interactable focus;

        [Tooltip("Showing the object target player is interacting")]
        private Transform target;

        private Movable _movable;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
        }

        private void Update()
        {
            // Check and makes character facing the target object when moving toward it
            if (target != null) _movable.FaceTarget(target.gameObject, true, out _);
        }
        // This will make character going to target object and stop when it is close enough
        private void SetTarget(Interactable newTarget)
        {
            _movable.SetStoppingDistance(newTarget.Radius/2);   
            target = newTarget.transform;
        }

        // Makes character not targeting the object anymore
        private void StopTarget()
        {
            _movable.SetStoppingDistance(0f);
            target = null;
        }

        // Set focus when interact
        public void StartInteract(Interactable newFocus)
        {
            if (newFocus != focus)
            {
                if (focus != null) focus.OnDefocused();
                focus = newFocus;
                SetTarget(newFocus);
            }

            newFocus.OnFocused(transform);

            _movable.Move(target.transform.position); 
        }

        // Remove focus when not interact
        public void RemoveFocus()
        {
            if (focus != null) focus.OnDefocused();
            focus = null;
            StopTarget();
        }
    }
}