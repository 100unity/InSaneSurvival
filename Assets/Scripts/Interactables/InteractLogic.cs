using AbstractClasses;
using UnityEngine;

namespace Interactables
{
    /// <summary>
    /// These are functions for character to interact with all objects in the game (ex: moving, following, pick up items, open crate, etc..)
    /// (Won't conflict with character attacking enemy or moving on the map)
    /// </summary>
    public class InteractLogic : MonoBehaviour
    {
        private Interactable _focus;
        private Transform _target;
        private Movable _movable;
        private float _initialStoppingDistance;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
        }

        private void Update()
        {
            // Check and makes character facing the target object when moving toward it
            if (_target != null)
                _movable.FaceTarget(_target.gameObject, true, out _);
        }

        // this is start because in Awake Movable is not yet ready
        private void Start() => _initialStoppingDistance = _movable.GetStoppingDistance();

        // This will make character going to target object and stop when it is close enough
        private void SetTarget(Interactable newTarget)
        {
            _movable.SetStoppingDistance(newTarget.Radius / 2);
            _target = newTarget.transform;
        }

        // Makes character not targeting the object anymore
        private void StopTarget()
        {
            _movable.SetStoppingDistance(_initialStoppingDistance);
            _target = null;
        }

        // Set focus when interact
        public void StartInteract(Interactable newFocus)
        {
            if (newFocus != _focus)
            {
                if (_focus != null) _focus.OnDefocused();
                _focus = newFocus;
                SetTarget(newFocus);
            }

            newFocus.OnFocused(transform);

            _movable.Move(_target.transform.position);
        }

        // Remove focus when not interact
        public void RemoveFocus()
        {
            if (_focus != null) _focus.OnDefocused();
            _focus = null;
            StopTarget();
        }
    }
}