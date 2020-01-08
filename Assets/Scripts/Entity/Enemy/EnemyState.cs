﻿using AbstractClasses;
using UnityEngine;
using Utils;

namespace Entity.Enemy {
    
    public class EnemyState : Damageable
    {
        [Tooltip("0 - dead")]
        [SerializeField]
        [Range(0, 100)]
        private int health;

        [Tooltip("Determines how many health points are restored per tick.")]
        [SerializeField]
        private AnimationCurve regenerationCurve;

        [Tooltip("Probability that the NPC regenerates when not in a fight or chasing.")]
        [SerializeField]
        private float regenerationProbability;

        [Tooltip("TargetFinderComponent - Finds nearby targets")]
        [SerializeField]
        private TargetFinder targetFinder;

        private readonly Probability _probability = new Probability();
        private readonly System.Random _random = new System.Random();
        private int _maxHealth;

        private void Awake()
        {
            _maxHealth = health;
        }

        private void Update()
        {
            if (!targetFinder.HasTarget() && health < _maxHealth)
                Regenerate();
        }
        
        public override void Die()
        {
            base.Die();
            Destroy(gameObject, 5f);
        }

        public override void Hit(int damage)
        {
            base.Hit(damage);
            ChangeHealth(-damage);
        }

        private void ChangeHealth(int changeBy)
        {
            int updatedValue = health + changeBy;
            if (updatedValue > _maxHealth) updatedValue = _maxHealth;
            else if (updatedValue <= 0)
            {
                updatedValue = 0;
                Die();
            }
            health = updatedValue;
        }

        /// <summary>
        /// Restores a certain amount of health points with a certain probability.
        /// </summary>
        private void Regenerate()
        {
            if (_probability.GetProbability(regenerationProbability))
            {
                float x = (float) _random.NextDouble();
                float healthPoints = regenerationCurve.Evaluate(x) * 10f;
                ChangeHealth((int)healthPoints);
            }
        }
    }
}
