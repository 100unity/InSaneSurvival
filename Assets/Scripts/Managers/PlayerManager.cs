using UnityEngine;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;

        public GameObject GetPlayer() => player;

        public bool PlayerInReach(GameObject otherObject, float maxRange) =>
            Vector3.Distance(player.transform.position, otherObject.transform.position) <=
            maxRange;
    }
}