using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Interactable
{
    Animator animator;

    [SerializeField] bool isOpen;

    private TooltipDisplay tooltipdisplay;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        base.Interact();
        if (!isOpen)
        {
            isOpen = true;
            animator.SetBool("open", isOpen);
            GetComponent<TooltipDisplay>().enabled = false;
        }
    }
}
