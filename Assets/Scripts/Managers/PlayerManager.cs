using UnityEngine;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;
        
        public GameObject GetPlayer() => player;
    }
}
