using Entity.Player;
using Managers;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Item/Consumable")]
    public class Consumable : Item
    {
        [SerializeField] private int healthValue;
        [SerializeField] private int saturationValue;
        [SerializeField] private int hydrationValue;

        public int HealthValue => healthValue;

        public int SaturationValue => saturationValue;

        public int HydrationValue => hydrationValue;

        /// <summary>
        /// Gets the PlayerState of the player, and changes its values depending on the item values.
        /// </summary>
        public new bool Use()
        {
            PlayerState playerState = PlayerManager.Instance.GetPlayer().GetComponent<PlayerState>();
            return playerState.Consume(this);
        }

        public override bool Equals(object other)
        {
            return other is Consumable item && item.name == name;
        }
    }
}