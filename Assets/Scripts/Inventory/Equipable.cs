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


        /// <summary>
        /// Returns a list of all abilities
        /// </summary>
        public List<int> Abilities => GetSelectedElements();

        /// <summary>
        /// Placeholder function for now.
        /// </summary>
        public new bool Use()
        {
            Debug.Log("Equipping the item " + name);
            return false;
        }

        public override bool Equals(object other)
        {
            return other is Equipable item && item.name == name;
        }

        private List<int> GetSelectedElements()
        {
            List<int> selectedElements = new List<int>();
            for (int i = 0; i < Enum.GetValues(typeof(EquipableAbility)).Length; i++)
            {
                int layer = 1 << i;
                if (((int) equipableAbility & layer) != 0)
                {
                    selectedElements.Add(i);
                }
            }

            return selectedElements;
        }
    }
}