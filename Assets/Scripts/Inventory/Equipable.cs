using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Equipable", menuName = "Inventory/Item/Equipable")]
    public class Equipable : Item
    {
        public enum EquipableAbility
        {
            Chop,
            Mine
        }

        [EnumMultiSelect] [SerializeField] private EquipableAbility equipableAbility;

        public bool isEquipped;

        /// <summary>
        /// Returns a list of all abilities
        /// </summary>
        public List<EquipableAbility> Abilities { get; private set; }

        private void Awake() => Abilities = GetSelectedElements();


        public bool HasAbility(EquipableAbility ability) => Abilities.Contains(ability);

        /// <summary>
        /// Placeholder function for now.
        /// </summary>
        public new bool Use()
        {
            Debug.Log("Equipping the item " + name);
            isEquipped = true;
            return true;
        }

        public override bool Equals(object other)
        {
            return other is Equipable item && item.name == name;
        }

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
    }
}