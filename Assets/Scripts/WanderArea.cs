using System;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Defines the area an NPC can wander in.
    /// </summary>
    [Serializable]
    public struct WanderArea
    {
        [Tooltip("The center of the wander area.")]
        public Transform center;

        [Tooltip("The radius of the wander area.")]
        public float radius;


        private Vector3 _centerFrozen;
        
        /// <summary>
        /// Returns the position of a fixed center if FreezeArea was called, or a mobile center.
        /// </summary>
        /// <returns>The center of the wander area</returns>
        public Vector3 GetCenterPosition()
        {
            if (_centerFrozen == Vector3.zero)
                return center.position;
            return _centerFrozen;
        }

        /// <summary>
        /// Freezes the area around the center transform, so that the area can either be
        /// fixed on the map or mobile around a moving transform (i.e. another entity).
        /// </summary>
        public void FreezeArea()
        {
            Vector3 t = center.position;
            _centerFrozen = new Vector3(t.x, t.y, t.z);
        }
    }
}

