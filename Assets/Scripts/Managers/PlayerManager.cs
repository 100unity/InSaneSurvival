﻿using UnityEngine;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;

        public GameObject GetPlayer() => player;

        public bool PlayerInReach(GameObject otherObject, float distanceToPlayer) =>
            Vector3.Distance(player.transform.position, otherObject.transform.position) <=
            distanceToPlayer;
    }
}