﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Interactable
{
    private Animator animator;

    [SerializeField] bool isOpen;

    private void Awake()
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
