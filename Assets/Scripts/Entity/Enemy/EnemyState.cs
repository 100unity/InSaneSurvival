using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Enemy {

    /// <summary>
    /// This class does not really implement the stats (like health etc) YET.
    /// </summary>
    public class EnemyState : Damageable
    {
        public override void Die()
        {
            Debug.Log("NPC is dead");
        }
    }
}
