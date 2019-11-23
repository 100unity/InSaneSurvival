﻿using Interfaces;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public bool PerformingHit { get; private set; }

    // component references
    private IMovable _movable;
    
    private float _timer;
    // can only hold IDamageables
    private GameObject _target;

    // ----temporary as animation replacement------
        private MeshRenderer _gameObjectRenderer;
        private Material _prevMat;
        private Material _attackAnimationMaterial;
    // ---------

    private void Awake()
    {
        // init components
        _movable = GetComponent<IMovable>();

        // -----temp replacement for animation-----
            _gameObjectRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            _prevMat = _gameObjectRenderer.material;
            _attackAnimationMaterial = new Material(Shader.Find("Standard"));
            _attackAnimationMaterial.color = Color.yellow;
        // ----------
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
        

        float distanceToTarget = Vector3.Distance(_target.transform.position, transform.position);
        if (distanceToTarget < attackRange && !PerformingHit)
        {
            // is in range
            _movable.StopMoving();
            // face target
            if (_movable.FaceTarget(_target, true))
            {
                // faces target
                // perform hit
                PerformingHit = true;

                // -----temp-----
                    _prevMat = _gameObjectRenderer.material;
                
                    _gameObjectRenderer.material = _attackAnimationMaterial;
                // ----------

            }
        }
        else if (PerformingHit)
        {
            bool? success = PerformHit();
            if (success != null)
            {
                // hit animation performed
                if (success == true)
                {
                    // deal damage
                    IDamageable damageable = (IDamageable)_target.GetComponent<IDamageable>();
                    damageable.Hit(damage);
                }

                // -----temp-----
                    //Material newMaterial = new Material(Shader.Find("Standard"));
                    //newMaterial.color = Color.gray;
                    _gameObjectRenderer.material = _prevMat;
                // ----------
                
                // end hit
                PerformingHit = false;
                if (resetAfterHit)
                {
                    // reset aggro independent of (un-)successful hit
                    _target = null;
                }
            }
        }
        else
        {
            // chase target
            _movable.Move(_target.transform.position);
        }
    }

    /// <summary>
    /// Starts the attack.
    /// </summary>
    /// <param name="target">The target to be attacked</param>
    public void StartAttack(GameObject target)
    {
        _target = target;
    }

    /// <summary>
    /// Stops the ongoing attack
    /// </summary>
    public void StopAttack()
    {
        _target = null;
    }

    /// <summary>
    /// Waits (attackTime) for hit to be finished and returns the result.
    /// </summary>
    /// <returns>Null if hit not finished, true if hit successful performed, false if unsuccessful</returns>
    private bool? PerformHit()
    {
        _timer += Time.deltaTime;

        if (_timer > attackTime)
        {
            // animation finished
            _timer = 0;
            // target still in range?
            float distanceToTarget = Vector3.Distance(_target.transform.position, transform.position);
            if (distanceToTarget < attackRange)
            {
                // check rotation as well
                float difference;
                _movable.FaceTarget(_target, false, out difference);
                return difference < hitRotationTolerance;
            }
            return false;
        }
        return null;
    }
}
