using Entity.Player;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;

        private PlayerController _playerController;

        protected override void Awake()
        {
            base.Awake();
            _playerController = player.GetComponent<PlayerController>();
        }

        public GameObject GetPlayer() => player;
        public PlayerController GetPlayerController() => _playerController;

        public bool PlayerInReach(GameObject otherObject, float maxRange) =>
            Vector3.Distance(player.transform.position, otherObject.transform.position) <=
            maxRange;
    }
}