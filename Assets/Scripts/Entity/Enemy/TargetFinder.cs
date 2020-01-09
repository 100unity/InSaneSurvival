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
        private readonly List<Damageable> _targets = new List<Damageable>();

        private void OnTriggerEnter(Collider other)
        {
            if (!attackableLayers.Contains(other.gameObject.layer)) return;

            if (other.gameObject.TryGetComponent(out Damageable target))
                if (!_targets.Contains(target))
                    _targets.Add(target);
        }

        /// <summary>
        /// Checks if the target finder has found any targets.
        /// </summary>
        /// <returns>True if at least one target was found, otherwise false</returns>
        public bool HasTarget() => _targets.Count > 0;

        /// <summary>
        /// Gets the first target from the target finder.
        /// </summary>
        /// <returns>The first target from the list of targets, or null if the list doesn't contain any targets</returns>
        public Damageable GetFirstTarget() => HasTarget() ? _targets[0] : null;

        /// <summary>
        /// Removes the first target from the target finder.
        /// </summary>
        public void RemoveFirstTarget() => _targets.RemoveAt(0);
    }
}