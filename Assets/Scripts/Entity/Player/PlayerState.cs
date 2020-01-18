using AbstractClasses;
using Entity.Enemy;
using Inventory;
using Remote;
using UnityEngine;

namespace Entity.Player
{
    public class PlayerState : Damageable
    {
        public delegate void PlayerStateChanged(int newValue);

        public delegate void PlayerEvents();
        public delegate void PlayerEventHit(EnemyController attacker);
        public delegate void PlayerIsDead();

        //Player State values
        [Tooltip("100 - full health, 0 - dead")] [SerializeField] [Range(0, 100)]
        private int health;

        [Tooltip("100: no hunger, 0: the player is in desperate need for food")] [SerializeField] [Range(0, 100)]
        private int saturation;

        [Tooltip("100: not thirsty, 0: gazing for a sip of water")] [SerializeField] [Range(0, 100)]
        private int hydration;

        [Tooltip("100: sane, 0: insane")] [Range(0, 100)] [SerializeField]
        private int sanity;

        public int Sanity => sanity;

        public static event PlayerStateChanged OnPlayerHealthUpdate;
        public static event PlayerStateChanged OnPlayerSaturationUpdate;
        public static event PlayerStateChanged OnPlayerHydrationUpdate;
        public static event PlayerStateChanged OnPlayerSanityUpdate;
        public static event PlayerEventHit OnPlayerHit;
        public static event PlayerEvents OnPlayerHealed;
        public static event PlayerIsDead OnPlayerDeath;

        private void Awake()
        {
            // send event on initial values
            /*OnPlayerHealthUpdate?.Invoke(health);
            OnPlayerSaturationUpdate?.Invoke(saturation);
            OnPlayerHydrationUpdate?.Invoke(hydration);
            OnPlayerSanityUpdate?.Invoke(sanity);*/
        }

        private void OnEnable()
        {
            RemoteStatusHandler.OnPlayerHealthRemoteUpdate += ChangePlayerHealth;
            RemoteStatusHandler.OnPlayerHydrationRemoteUpdate += ChangePlayerHydration;
            RemoteStatusHandler.OnPlayerSaturationRemoteUpdate += ChangePlayerSaturation;
            RemoteStatusHandler.OnPlayerSanityRemoteUpdate += ChangePlayerSanity;
        }

        private void OnDisable()
        {
            RemoteStatusHandler.OnPlayerHealthRemoteUpdate -= ChangePlayerHealth;
            RemoteStatusHandler.OnPlayerHydrationRemoteUpdate -= ChangePlayerHydration;
            RemoteStatusHandler.OnPlayerSaturationRemoteUpdate -= ChangePlayerSaturation;
            RemoteStatusHandler.OnPlayerSanityRemoteUpdate -= ChangePlayerSanity;
        }
        
        public int GetHealth() => health;
        public int GetSaturation() => saturation;
        public int GetHydration() => hydration;
        public int GetSanity() => sanity;

        public void SetHealth(int value)
        {
            health = value;
            OnPlayerHealthUpdate?.Invoke(value);
        }

        public void SetSaturation(int value)
        {
            saturation = value;
            OnPlayerSaturationUpdate?.Invoke(value);
        }

        public void SetHydration(int value)
        {
            hydration = value;
            OnPlayerHydrationUpdate?.Invoke(value);
        }

        public void SetSanity(int value)
        {
            sanity = value;
            OnPlayerSanityUpdate?.Invoke(value);
        }

        //Interface
        private void ChangePlayerHealth(int changeBy)
        {
            int updatedValue = health + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue <= 0)
            {
                updatedValue = 0;
                Die();
            }

            health = updatedValue;
            //throws an event with the new health value as a parameter
            OnPlayerHealthUpdate?.Invoke(updatedValue);
        }

        public void ChangePlayerSaturation(int changeBy)
        {
            int updatedValue = saturation + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                ChangePlayerHealth(-1);
            }

            saturation = updatedValue;
            OnPlayerSaturationUpdate?.Invoke(updatedValue);
        }

        public void ChangePlayerHydration(int changeBy)
        {
            int updatedValue = hydration + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                ChangePlayerHealth(-1);
            }

            hydration = updatedValue;
            OnPlayerHydrationUpdate?.Invoke(updatedValue);
        }

        public void ChangePlayerSanity(int changeBy)
        {
            sanity = Mathf.Clamp(sanity + changeBy, 0, 100);
            OnPlayerSanityUpdate?.Invoke(sanity);
        }

        /// <summary>
        /// Does damage to the player.
        /// </summary>
        /// <param name="damage">The damage dealt to player</param>
        /// <param name="attacker">The EnemyController of the enemy</param>
        public override void Hit(int damage, EnemyController attacker)
        {
            base.Hit(damage);
            ChangePlayerHealth(-damage);
            OnPlayerHit?.Invoke(attacker);
        }

        /// <summary>
        /// Heals the player by a specific amount.
        /// Marks the player as healed after being healed.
        /// </summary>
        /// <param name="amount">The amount the player gets healed by</param>
        public void Heal(int amount)
        {
            ChangePlayerHealth(amount);
            OnPlayerHealed?.Invoke();
        }

        public bool Consume(Consumable item)
        {
            if (item.HealthValue > 0)
                Heal(item.HealthValue);
            if (item.SaturationValue > 0)
                ChangePlayerSaturation(item.SaturationValue);
            if (item.HydrationValue > 0)
                ChangePlayerHydration(item.HydrationValue);
            return true;
        }

        public override void Die()
        {
            OnPlayerDeath?.Invoke();
        }
    }
}