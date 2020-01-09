using UnityEngine;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;

        public GameObject GetPlayer() => player;

        public Transform SpawnPoint { get; private set; }

        /// <summary>
        /// Sets the spawn point of the player. WIP.
        /// </summary>
        /// <param name="spawnPoint">The new spawn point</param>
        public void SetSpawnPoint(Transform spawnPoint) => SpawnPoint = spawnPoint;
    }
}