using System;
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

        [Tooltip("Defines the amount of uses.")] [SerializeField]
        private int maxUses;

        /// <summary>
        /// The damage boost of this equipable
        /// </summary>
        public int DamageBoost => damageBoost;

        /// <summary>
        /// The max amount of uses.
        /// </summary>
        public int MaxUses => maxUses;

        /// <summary>
        /// A list of all abilities this equipable has
        /// </summary>
        private List<EquipableAbility> _abilities;

        /// <summary>
        /// Get all abilities from the MultiSelect
        /// </summary>
        private void Awake() => _abilities = GetSelectedElements();

        /// <summary>
        /// Get all abilities from the MultiSelect (if null) and
        /// checks if this item has the given abilityi
        /// </summary>
        /// <param name="ability">The ability to check</param>
        /// <returns>Whether it has the given ability or not</returns>
        public bool HasAbility(EquipableAbility ability)
        {
            if (_abilities.Count == 0)
                _abilities = equipableAbility.GetSelectedElements();
            return _abilities.Contains(ability) || ability == EquipableAbility.None;
        }

        /// <summary>
        /// Placeholder function for now.
        /// </summary>
        public override bool Use()
        {
            Debug.Log("Equipping the item " + name);
            return true;
        }

        /// <summary>
        /// Gets all Abilities selected in the MultiSelect
        /// </summary>
        /// <returns>A list of all selected abilities</returns>
        private List<EquipableAbility> GetSelectedElements()
        {
            List<EquipableAbility> selectedElements = new List<EquipableAbility>();
            for (int i = 0; i < Enum.GetValues(typeof(EquipableAbility)).Length; i++)
            {
                int layer = 1 << i;
                if (((int) equipableAbility & layer) != 0)
                {
                    selectedElements.Add((EquipableAbility) i);
                }
            }

            return selectedElements;
        }

        #region Inner-Class

        /// <summary>
        /// All abilities a equipable could have
        /// </summary>
        public enum EquipableAbility
        {
            Chop,
            Mine,
            None
        }

        #endregion
    }
}