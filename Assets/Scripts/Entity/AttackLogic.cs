using Entity.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Movable))]
    public class AttackLogic : MonoBehaviour
    {
        [Tooltip("The base damage dealt")]
        [SerializeField]
        private int damage;

        [Tooltip("The maximum distance between attacker and target in order to deal damage")]
        [SerializeField]
        private int attackRange;

        [Tooltip("The time needed for an attack in seconds")]
        [SerializeField]
        private double attackTime;

        [Tooltip("The maximum difference in degrees for the attacker between look direction and target direction in order to perform a successful hit")]
        [SerializeField]
        private double hitRotationTolerance;

        [Tooltip("Stops attacking the target after a (un-)successful hit")]
        [SerializeField]
        private bool resetAfterHit;


        public enum AttackStatus { Hit, Miss, NotFinished, None };
        public AttackStatus Status { get; private set; }

        // component references
        private Movable _movable;

        private float _timer;
        private GameObject _target;
        private float _distanceToTarget;

        // ----temporary as animation replacement------
            private MeshRenderer _gameObjectRenderer;
            private Material _prevMat;
            private Material _attackAnimationMaterial;
        // ---------

        private void Awake()
        {
            // init components
            _movable = GetComponent<Movable>();

            // -----temp replacement for animation-----
                _gameObjectRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
                _prevMat = _gameObjectRenderer.material;
                _attackAnimationMaterial = new Material(Shader.Find("Standard"));
                _attackAnimationMaterial.color = Color.yellow;
            // ----------
        }

        private void OnEnable()
        {
            Status = AttackStatus.None;
        }

        /// <summary>
        /// If it has a target, attacks it.
        /// </summary>
        private void Update()
        {
            if (_target != null)
            {
                Attack();
            }
        }

        /// <summary>
        /// Attacks the target. If the target is not in attacking range, chases it. If the target is near
        /// performs a hit on it. If currently performing a hit, waits for finishing the hit and deals 
        /// damage if hit was successful. Either resets afterwards or continues chasing / attacking target.
        /// </summary>
        private void Attack()
        {
            _distanceToTarget = Vector3.Distance(_target.transform.position, transform.position);
            if (_distanceToTarget < attackRange && Status == AttackStatus.None)
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
                _movable.Move(_target.transform.position);
            }
        }

        /// <summary>
        /// Stops moving, faces the target if not facing yet, if facing, starts a hit.
        /// </summary>
        private void IsInRange()
        {
            _movable.StopMoving();
            // face target
            if (_movable.FaceTarget(_target, true, out _))
            {
                // faces target
                // perform hit
                Status = AttackStatus.NotFinished;

                // -----temp-----
                _prevMat = _gameObjectRenderer.material;
                _gameObjectRenderer.material = _attackAnimationMaterial;
                // ----------
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
                Damageable damageable = _target.GetComponent<Damageable>();
                damageable.Hit(damage);
            }

            // -----temp-----
            //Material newMaterial = new Material(Shader.Find("Standard"));
            //newMaterial.color = Color.gray;
            _gameObjectRenderer.material = _prevMat;
            // ----------

            // end hit
            Status = AttackStatus.None;
            if (resetAfterHit)
            {
                // reset aggro independent of (un-)successful hit
                _target = null;
            }
        }

        /// <summary>
        /// Starts the attack. If the target is not in attacking range, chases it. If the target is near
        /// performs a hit on it and either resets or continues attacking.
        /// </summary>
        /// <param name="target">The target to be attacked</param>
        public void StartAttack(GameObject target)
        {
            print("start attack");
            _target = target;
        }

        /// <summary>
        /// Stops the ongoing attack.
        /// </summary>
        public void StopAttack()
        {
            _target = null;
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
                // animation finished
                _timer = 0;
                // target still in range?
                if (_distanceToTarget < attackRange)
                {
                    // check rotation as well
                    float difference;
                    _movable.FaceTarget(_target, false, out difference);
                    if (difference < hitRotationTolerance)
                        return AttackStatus.Hit;
                    else
                        return AttackStatus.Miss;
                }
                return AttackStatus.Miss;
            }
            return AttackStatus.NotFinished;
        }
    }
}
