using System;
using Interfaces;
using UnityEngine;
using Utils;

namespace Enemy
{
    [RequireComponent(typeof(Collider))]
    public class TargetFinder : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyController;
        
        [Tooltip("Every layer that will be considered as possible target")] [SerializeField]
        private LayerMask attackableLayers;

        private void OnTriggerEnter(Collider other)
        {
            if(!attackableLayers.Contains(other.gameObject.layer)) return;
            
            if(other.gameObject.TryGetComponent(typeof(IDamageable), out _))
                if(!enemyController.Targets.Contains(other.gameObject))
                    enemyController.Targets.Add(other.gameObject);
        }
    }
}