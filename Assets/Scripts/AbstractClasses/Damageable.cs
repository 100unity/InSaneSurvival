using Constants;
using UnityEngine;

namespace AbstractClasses
{
    public abstract class Damageable : MonoBehaviour
    {
        [Tooltip("Animator for playing the hit animation")]
        [SerializeField]
        private Animator animator;

        private static readonly int HitTrigger = Animator.StringToHash(Consts.Animation.HIT_TRIGGER);
        private static readonly int DieTrigger = Animator.StringToHash(Consts.Animation.DIE_TRIGGER);

        /// <summary>
        /// Marks the player as hit after being hit.
        /// </summary>
        /// <param name="damage">The damage dealt to player</param>
        public virtual void Hit(int damage)
        {
            animator.SetTrigger(HitTrigger);
        }

        public virtual void Die()
        {
            animator.SetTrigger(DieTrigger);
        }
    }
}