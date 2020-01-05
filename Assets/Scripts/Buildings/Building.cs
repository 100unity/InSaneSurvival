using Managers;
using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Renderer buildingRenderer;

        [Tooltip("Defines the needed distance to the player for showing the tooltip")] [SerializeField]
        private float showWithDistanceToPlayer;

        [Tooltip("The crafting station that this building provides")] [SerializeField]
        private CraftingManager.CraftingStation craftingStation;

        public Renderer BuildingRenderer => buildingRenderer;

        /// <summary>
        /// Whether the player is in reach of this building.
        /// </summary>
        public bool PlayerInReach { get; private set; }

        /// <summary>
        /// Whether the building is build or not
        /// </summary>
        public bool IsBuild { get; set; }

        /// <summary>
        /// Used for resetting <see cref="CraftingManager.CurrentCraftingStation"/>
        /// </summary>
        private bool _playerWasInReach;

        /// <summary>
        /// Used for calculating the distance
        /// </summary>
        private GameObject _player;

        private void Awake() => _player = PlayerManager.Instance.GetPlayer();

        /// <summary>
        /// Updates <see cref="PlayerInReach"/> and sets the <see cref="CraftingManager.CurrentCraftingStation"/>.
        /// </summary>
        private void Update()
        {
            PlayerInReach = Vector3.Distance(_player.transform.position, transform.position) <=
                            showWithDistanceToPlayer;

            if (!IsBuild) return;

            if (PlayerInReach && !_playerWasInReach)
            {
                _playerWasInReach = true;
                CraftingManager.Instance.CurrentCraftingStation = craftingStation;
            }

            // Walked away from this crafting station
            if (!PlayerInReach && _playerWasInReach)
            {
                _playerWasInReach = false;
                CraftingManager.Instance.CurrentCraftingStation = CraftingManager.CraftingStation.None;
            }
        }
    }
}