using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Interactable
{
    Animator animator;

    [SerializeField] bool isOpen;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetBool("open", isOpen);
        }
    }
}
