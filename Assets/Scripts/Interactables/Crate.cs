using Constants;
using UnityEngine;

namespace Interactables
{
    public class Crate : Interactable
    {
        [SerializeField] private Animator animator;

        [SerializeField] private bool isOpen;
        private static readonly int Open = Animator.StringToHash(Consts.Animation.OPEN_BOOL);

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void Interact()
        {
            if (!isOpen)
            {
                isOpen = true;
                animator.SetBool(Open, isOpen);
            }
        }
    }
}
