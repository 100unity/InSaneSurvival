using Entity.Player;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;

        private PlayerController _playerController;
        private PlayerState _playerState;

        protected override void Awake()
        {
            base.Awake();
            _playerController = player.GetComponent<PlayerController>();
            _playerState = player.GetComponent<PlayerState>();
        }

        public GameObject GetPlayer() => player;
        public PlayerController GetPlayerController() => _playerController;
        public PlayerState GetPlayerState() => _playerState;

        public bool PlayerInReach(GameObject otherObject, float maxRange) =>
            Vector3.Distance(player.transform.position, otherObject.transform.position) <=
            maxRange;
    }
}