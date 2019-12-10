using AbstractClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractLogic : MonoBehaviour
{
    /// <summary>
    /// These are functions for character to interact with all objects in the game (ex: moving, following, pick up items, open crate, etc..) using raycast and _NavMeshAgent
    /// (Won't conflict with character attacking enemy or moving on the map)
    /// </summary>
    /// 

    [Tooltip("Showing the object player is focusing")]
    [SerializeField]
    private Interactable focus;

    [Tooltip("Showing the object target player is interacting")]
    [SerializeField]
    private Transform target;

    private Movable _movable;

    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _movable = GetComponent<Movable>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check and makes character facing the target object when moving toward it
        if (target != null) _movable.FaceTarget(target.gameObject, true, out _);
    }
    // This will make character going to target object and stop when it is close enough
    private void SetTarget(Interactable newTarget)
    {
        _navMeshAgent.stoppingDistance = newTarget.Radius * .8f;
        target = newTarget.transform;
    }

    // Makes character not targeting the object anymore
    private void StopTarget()
    {
        _navMeshAgent.stoppingDistance = 0f;
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