using UnityEngine;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Inventory inventory;

        public GameObject GetPlayer()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player"); //BUG: Replace me with Trigger
            return player;
        }

        public Inventory GetInventory()
        {
            return inventory = player.GetComponentInChildren<Inventory>();
        }
    }
}
