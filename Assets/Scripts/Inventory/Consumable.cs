using Entity.Player;
using Managers;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Item/Consumable")]
    public class Consumable : Item
    {
        [Header("Consumable")] [SerializeField]
        private int healthValue;

        [SerializeField] private int saturationValue;
        [SerializeField] private int hydrationValue;
        [SerializeField] private string soundOnUse;

        public int HealthValue => healthValue;

        public int SaturationValue => saturationValue;

        public int HydrationValue => hydrationValue;
        
        public string SoundOnUse => soundOnUse;

        /// <summary>
        /// Gets the PlayerState of the player, and changes its values depending on the item values.
        /// </summary>
        public override bool Use()
        {
            InventoryManager.Instance.RemoveItem(this);
            PlayerState playerState = PlayerManager.Instance.GetPlayer().GetComponent<PlayerState>();
            return playerState.Consume(this);
        }

        public override bool Equals(object other)
        {
            return other is Consumable item && item.name == name;
        }
    }
}