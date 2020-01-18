using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Equipable", menuName = "Inventory/Item/Equipable")]
    public class Equipable : Item
    {
        [Header("Equipable")] [Tooltip("All abilities this item has")] [EnumMultiSelect] [SerializeField]
        private EquipableAbility equipableAbility;

        [Tooltip("The damage that will be added to the normal attack when this is equipped")] [SerializeField]
        private int damageBoost;

        /// <summary>
        /// The damage boost of this equipable
        /// </summary>
        public int DamageBoost => damageBoost;

        /// <summary>
        /// A list of all abilities this equipable has
        /// </summary>
        private List<EquipableAbility> _abilities;

        /// <summary>
        /// Get all abilities from the MultiSelect (if null) and
        /// checks if this item has the given ability
        /// </summary>
        /// <param name="ability">The ability to check</param>
        /// <returns>Whether it has the given ability or not</returns>
        public bool HasAbility(EquipableAbility ability)
        {
            if (_abilities == null)
                _abilities = equipableAbility.GetSelectedElements();
            return _abilities.Contains(ability);
        }

        /// <summary>
        /// Placeholder function for now.
        /// </summary>
        public override bool Use()
        {
            Debug.Log("Equipping the item " + name);
            return true;
        }


        #region Inner-Class

        /// <summary>
        /// All abilities a equipable could have
        /// </summary>
        public enum EquipableAbility
        {
            Chop,
            Mine
        }

        #endregion
    }
}