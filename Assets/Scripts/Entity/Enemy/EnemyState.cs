using AbstractClasses;
using Inventory;
using Managers;
using UnityEngine;
using Utils;

namespace Entity.Enemy {
    
    [RequireComponent(typeof(AttackLogic))]
    [RequireComponent(typeof(EnemyController))]
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
        private AttackLogic _attackLogic;
        private EnemyController _enemyController;

        private void Awake()
        {
            _maxHealth = health;
            _attackLogic = GetComponent<AttackLogic>();
            _enemyController = GetComponent<EnemyController>();
        }

        private void Update()
        {
            if (!targetFinder.HasTarget() && health < _maxHealth)
                Regenerate();
        }
        
        public override void Die()
        {
            base.Die();
            _attackLogic.enabled = false;
            Destroy(gameObject, 5f);
            CoroutineManager.Instance.WaitForSeconds(5, () =>
            {
                foreach (ItemResourceData resourceData in _enemyController.ItemDrops)
                    for (int i = 0; i < resourceData.amount; i++)
                        InventoryManager.Instance.AddItem(resourceData.item);
            });
        }

        /// <summary>
        /// Run away if NPC is not aggressive. Only do hit animation if not running away.
        /// </summary>
        /// <param name="damage">The damage dealt</param>
        /// <param name="_">EnemyController is not used because this is an NPC.</param>
        public override void Hit(int damage, EnemyController _)
        {
            ChangeHealth(-damage);
            if (health <= 0)
                return;
            
            if (!_enemyController.IsAggressive)
                _enemyController.RunAway();
            // don't do hit animation if running away
            else
                base.Hit(damage);
        }

        /// <summary>
        /// Run away if NPC is not aggressive. Only do hit animation if not running away.
        /// </summary>
        /// <param name="damage">The damage dealt</param>
        /// <param name="health">The health after damage was dealt.</param>
        /// <param name="_">EnemyController is not used because this is an NPC.</param>
        public override void Hit(int damage, out int health, EnemyController _)
        {
            Hit(damage, _);
            health = this.health;
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
