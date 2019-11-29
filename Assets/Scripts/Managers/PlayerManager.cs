using System.Collections;
using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;
        
        public GameObject GetPlayer()
        {
            if (player == null)
                GameObject.FindGameObjectWithTag("Player"); //BUG: Replace me with Trigger
            return player;
        }

    }
}
