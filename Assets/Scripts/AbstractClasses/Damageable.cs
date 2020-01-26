using Constants;
using Entity.Enemy;
using UnityEngine;

namespace AbstractClasses
{
    public abstract class Damageable : MonoBehaviour
    {
        public delegate void PlayerHit(EnemyController attacker);
        public delegate void EventHit(Damageable hit);

        [Tooltip("Animator for playing the hit animation")]
        [SerializeField]
        private Animator animator;

        private static readonly int HitTrigger = Animator.StringToHash(Consts.Animation.HIT_TRIGGER);
        private static readonly int DieTrigger = Animator.StringToHash(Consts.Animation.DIE_TRIGGER);

        public static event EventHit OnHit;
        public static event PlayerHit OnPlayerHit;

        /// <summary>
        /// This entity was hit. Invoke <see cref="OnPlayerHit"/> if this entity is the player.
        /// </summary>
        /// <param name="damage">The damage dealt</param>
        /// <param name="attacker">The EnemyController of the attacker if it's an NPC.</param>
        public virtual void Hit(int damage, EnemyController attacker = null)
        {
            animator.SetTrigger(HitTrigger);
            OnHit?.Invoke(this);

            // yes I know this is kinda shitty
            if (attacker != null)
                OnPlayerHit?.Invoke(attacker);
        }

        /// <summary>
        /// Does damage to the player. Implementation should call base.Hit(damage, attacker)
        /// </summary>
        /// <param name="damage">The damage dealth to the player</param>
        /// <param name="health">The health of the entity after damage is dealt</param>
        /// <param name="attacker">The EnemyController of the attacker if it's an NPC.</param>
        public abstract void Hit(int damage, out int health, EnemyController attacker = null);

        public virtual void Die()
        {
            animator.SetTrigger(DieTrigger);
        }
    }
}