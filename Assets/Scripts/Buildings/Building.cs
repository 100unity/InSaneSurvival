using Managers;
using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Renderer buildingRenderer;

        [Tooltip("Defines the needed distance to the player for showing the tooltip")] [SerializeField]
        private float showWithDistanceToPlayer;

        public Renderer BuildingRenderer => buildingRenderer;


        public bool PlayerInReach { get; private set; }

        /// <summary>
        /// Whether the building is build or not
        /// </summary>
        public bool IsBuild { get; set; }


        /// <summary>
        /// Used for calculating the distance
        /// </summary>
        private GameObject _player;

        private void Awake()
        {
            _player = PlayerManager.Instance.GetPlayer();
        }

        private void Update()
        {
            PlayerInReach = Vector3.Distance(_player.transform.position, transform.position) <=
                            showWithDistanceToPlayer;
        }
    }
}