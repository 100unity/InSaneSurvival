using UnityEngine;
using AbstractClasses;
using Constants;
using Managers;
using System;
using Entity.Enemy;

namespace Entity
{
    [RequireComponent(typeof(Movable))]
    public class AttackLogic : MonoBehaviour
    {
        public delegate void AttackPerformed(AttackLogic attacker);

        [Tooltip("The base damage dealt")] [SerializeField]
        private int damage;

        [Tooltip("The maximum distance between attacker and target in order to deal damage")] [SerializeField]
        private float attackRange;

        [Tooltip("The time needed for an attack in seconds")] [SerializeField]
        private double attackTime;

        [Tooltip("The maximum difference in degrees for the attacker between look direction and target direction in order to perform a successful hit")]
        [SerializeField]
        private double hitRotationTolerance;

        [Tooltip("Stops attacking the target after a (un-)successful hit")] [SerializeField]
        private bool resetAfterHit;

        [Tooltip("The time you have to be out of combat (not attacking, not being hit) to be considered out of combat.")]
        [SerializeField]
        private float fightStopTime;

        [Tooltip("Animator for playing the attack animation")] [SerializeField]
        private Animator animator;


        public static event AttackPerformed OnAttackPerformed;

        public enum AttackStatus { Hit, Miss, NotFinished, None, TargetReached }

        /// <summary>
        /// Whether this entity is considered to be in combat.
        /// </summary>
        public bool IsFighting { get; private set; }

        public AttackStatus Status { get; private set; }
        public Damageable Target { get; private set; }

        /// <summary>
        /// The EnemyController of the last attacked target if it's an NPC.
        /// </summary>
        public EnemyController lastAttacked { get; private set; }

        // component references
        private Movable _movable;
        private EnemyController _enemyController;

        // the time this entity has stopped fighting (+ was not hit) while being in combat
        private float _attackStopTimer;

        private float _timer;
        private float _distanceToTarget;
        private static readonly int AttackTrigger = Animator.StringToHash(Consts.Animation.ATTACK_TRIGGER);

        private void Awake()
        {
            // init components
            _movable = GetComponent<Movable>();
            // try get this, null if player
            _enemyController = GetComponent<EnemyController>();
        }

        private void OnEnable()
        {
            Status = AttackStatus.None;
            Damageable.OnHit += CheckHit;
        }

        private void OnDisable()
        {
            Damageable.OnHit -= CheckHit;
        }

        /// <summary>
        /// If it has a target, attacks it.
        /// </summary>
        private void Update()
        {
            if (Target != null)
            {
                Attack();
            }
            else
            {
                // if target despawns, the player should not freeze
                _timer = 0;
                _distanceToTarget = 0;
                Status = AttackStatus.None;
            }

            if (IsFighting)
            {
                MightEndFighting();
            }
        }


        /// <summary>
        /// Set the <see cref="IsFighting"/> to false if this entity has been out of
        /// combat (not hit, not attacking) for a certain time.
        /// </summary>
        private void MightEndFighting()
        {
            if (Target == null)
                _attackStopTimer += Time.deltaTime;
            else
                _attackStopTimer = 0;
            if (_attackStopTimer >= fightStopTime)
            {
                IsFighting = false;
                _attackStopTimer = 0;
            }
        }

        /// <summary>
        /// Attacks the target. If the target is not in attacking range, chases it. If the target is near
        /// performs a hit on it. If currently performing a hit, waits for finishing the hit and deals 
        /// damage if hit was successful. Either resets afterwards or continues chasing / attacking target.
        /// </summary>
        private void Attack()
        {
            _distanceToTarget = Vector3.Distance(Target.transform.position, transform.position);
            if (_distanceToTarget < attackRange && Status == AttackStatus.None || Status == AttackStatus.TargetReached)
            {
                IsInRange();
            }
            else if (Status == AttackStatus.NotFinished)
            {
                Status = PerformHit();
                if (Status != AttackStatus.NotFinished)
                {
                    HitFinished();
                }
            }
            else
            {
                // chase target
                _movable.Move(Target.transform.position);
            }
        }

        /// <summary>
        /// Stops moving, faces the target if not facing yet, if facing, starts a hit.
        /// </summary>
        private void IsInRange()
        {
            _movable.StopMoving();
            Status = AttackStatus.TargetReached;
            // face target
            if (_movable.FaceTarget(Target.gameObject, true, out _))
            {
                // faces target
                // perform hit
                Status = AttackStatus.NotFinished;

                animator.SetTrigger(AttackTrigger);
            }
        }

        /// <summary>
        /// Ends the hit. Deals damage to the target, if hit was successful. Resets aggro if necessary.
        /// </summary>
        private void HitFinished()
        {
            // hit animation performed
            if (Status == AttackStatus.Hit)
            {
                // deal damage
                int damageDealt = damage;
                // add damage boost from weapon if is player
                if (_enemyController == null)
                    damageDealt += InventoryManager.Instance.DamageBoostFromEquipable;
                Target.Hit(damageDealt, out int targetHealth, _enemyController);
                if (targetHealth <= 0)
                    StopAttack();
            }

            // end hit
            Status = AttackStatus.None;
            if (resetAfterHit)
            {
                // reset aggro independent of (un-)successful hit
                Target = null;
            }
        }

        /// <summary>
        /// Starts the attack. If the target is not in attacking range, chases it. If the target is near
        /// performs a hit on it and either resets or continues attacking.
        /// </summary>
        /// <param name="target">The target to be attacked</param>
        /// <param name="enemyController">If an the attacked instance is an NPC, pass its EnemyController</param>
        public void StartAttack(Damageable target, EnemyController enemyController = null)
        {
            Target = target;
            lastAttacked = enemyController;
        }

        /// <summary>
        /// Stops the ongoing attack.
        /// </summary>
        public void StopAttack()
        {
            Target = null;
        }

        /// <summary>
        /// Waits (attackTime) for hit to be finished and returns the result.
        /// </summary>
        /// <returns>NotFinished if hit not finished, Hit if hit performed successfully, Miss if unsuccessful</returns>
        private AttackStatus PerformHit()
        {
            _timer += Time.deltaTime;

            if (_timer > attackTime)
            {
                AttackWasPerformed();
                // target still in range?
                if (_distanceToTarget < attackRange)
                {
                    // check rotation as well
                    _movable.FaceTarget(Target.gameObject, false, out float difference);
                    return difference < hitRotationTolerance ? AttackStatus.Hit : AttackStatus.Miss;
                }
                return AttackStatus.Miss;
            }

            return AttackStatus.NotFinished;
        }

        /// <summary>
        /// Set <see cref="IsFighting"/> to true as soon as an attack is performed.
        /// </summary>
        private void AttackWasPerformed()
        {
            _timer = 0;
            if (!IsFighting)
                IsFighting = true;
            OnAttackPerformed?.Invoke(this);
        }

        /// <summary>
        /// Check if this entity was hit.
        /// </summary>
        /// <param name="hit">The entity that was hit.</param>
        private void CheckHit(Damageable hit)
        {
            if (hit.gameObject == gameObject)
            {
                OnHit();
            }
        }

        /// <summary>
        /// Make this entity stay in / enter combat.
        /// </summary>
        private void OnHit()
        {
            // enter combat
            if (!IsFighting)
                IsFighting = true;

            // also reset the attack stop timer due to still being in combat
            _attackStopTimer = 0;
        }
    }
}