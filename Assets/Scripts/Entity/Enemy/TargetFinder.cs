using AbstractClasses;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Entity.Enemy
{
    /// <summary>
    /// Finds all attackable objects that enter the trigger-collider and gives them to the enemy controller
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TargetFinder : MonoBehaviour
    {
        [Tooltip("Every layer that will be considered as possible target")] [SerializeField]
        private LayerMask attackableLayers;

        /// <summary>
        /// List of all targets that entered the trigger-radius and have a layer of the <see cref="attackableLayers"/> layer mask
        /// </summary>
        public readonly List<GameObject> Targets = new List<GameObject>();  //TODO: Should probably be replaced by the abstract class "Damageable"

        private void OnTriggerEnter(Collider other)
        {
            if(!attackableLayers.Contains(other.gameObject.layer)) return;
            
            if(other.gameObject.TryGetComponent(typeof(Damageable), out _))
                if(!Targets.Contains(other.gameObject))
                    Targets.Add(other.gameObject);
        }
    }
}