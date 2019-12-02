using UnityEngine;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject _player;
        
        public GameObject GetPlayer()
        {
            if (_player == null)
                _player = GameObject.FindGameObjectWithTag("Player"); //BUG: Replace me with Trigger
            return _player;
        }

    }
}
