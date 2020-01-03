using AbstractClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Entity.Enemy {
    
    public class EnemyState : Damageable
    {
        [Tooltip("0 - dead")]
        [SerializeField]
        [Range(0, 100)]
        private int health;

        [Tooltip("The mean of the restored health points the NPC will regenerate with.")]
        [SerializeField]
        private AnimationCurve regenerationCurve;

        [SerializeField]
        private float regenerationProbability;

        [Tooltip("TargetFinderComponent - Finds nearby targets")]
        [SerializeField]
        private TargetFinder targetFinder;

        private Probability _probability;
        private int _maxHealth;

        protected override void Awake()
        {
            base.Awake();
            _probability = new Probability();
            _maxHealth = health;
        }

        protected override void Update()
        {
            base.Update();
            if (!targetFinder.HasTarget() && health < _maxHealth)
            {
                Regenerate();
            }
        }

        public override void Die()
        {
            Destroy(gameObject);
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

        private void Regenerate()
        {
            if (_probability.GetProbability(regenerationProbability))
            {
                System.Random random = new System.Random();
                float x = (float) random.NextDouble();
                Debug.Log("x: " + x);
                float healthPoints = regenerationCurve.Evaluate(x) * 10f;
                Debug.Log(healthPoints);
                ChangeHealth((int)healthPoints);
            }
        }
    }
}
