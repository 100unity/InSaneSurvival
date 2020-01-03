using AbstractClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Enemy {
    
    public class EnemyState : Damageable
    {
        [Tooltip("100 - full health, 0 - dead")]
        [SerializeField]
        [Range(0, 100)]
        private int health;

        public override void Die()
        {
            Destroy(gameObject);
        }

        private void ChangeHealth(int changeBy)
        {
            int updatedValue = health + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue <= 0)
            {
                updatedValue = 0;
                Die();
            }

            health = updatedValue;
        }

        public override void Hit(int damage)
        {
            base.Hit(damage);
            ChangeHealth(-damage);
        }
    }
}
