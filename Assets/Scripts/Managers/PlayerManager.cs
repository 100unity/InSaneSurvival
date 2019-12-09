using UnityEngine;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;

        public GameObject GetPlayer()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player"); //BUG: Replace me with Trigger
            return player;
        }     
    }
}
