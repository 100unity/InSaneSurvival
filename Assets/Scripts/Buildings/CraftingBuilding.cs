using Managers;
using UnityEngine;

namespace Buildings
{
    /// <summary>
    /// A CraftingBuilding offers crafting-functionality to the player, allowing him to craft more items.
    /// It automatically updates the <see cref="CraftingManager._craftingStations"/> when the player is close enough.
    /// </summary>
    public class CraftingBuilding : Building
    {
        [Tooltip("The crafting station that this building provides")] [SerializeField]
        private CraftingManager.CraftingStation craftingStation;

        /// <summary>
        /// Updates the <see cref="CraftingManager._craftingStations"/>.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (!IsBuild) return;

            if (PlayerInReach)
                CraftingManager.Instance.AddCraftingStation(craftingStation);
            else if (CraftingManager.Instance.HasCraftingStation(craftingStation))
                CraftingManager.Instance.RemoveCraftingStation(craftingStation);
        }
    }
}