using Constants;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Utils
{
    /// <summary>
    /// Can map random points onto the NavMesh.
    /// </summary>
    public class NavMeshMapper
    {
        /// <summary>
        /// Returns a random point mapped onto the NavMesh.
        /// Since NavMesh.SamplePosition() can get very expensive, it's better to use it with a small distance
        /// and try multiple times (and only use the random points that are near the NavMesh).
        /// </summary>
        /// <param name="area">The area to draw the point from</param>
        /// <param name="areaMask">The NavMesh area mask to map the point onto</param>
        /// <returns>The random point on the ground</returns>
        public Vector3 GetMappedRandomPoint(Area area, int areaMask)
        {
            for (int i = 0; i < Consts.Utils.MAPPING_ITERATIONS; i++)
            {
                Vector3 randomPoint = GetRandomPoint(area);
                // try to map point onto NavMesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, Consts.Utils.MAX_NAVMESH_MAPPING_DISTANCE, areaMask))
                {
                    return hit.position;
                }
            }
            throw new Exception("Could not map a point onto the NavMesh in " + Consts.Utils.MAPPING_ITERATIONS + " random points.");
        }

        /// <summary>
        /// Draws a random point from a sphere generated using the spawn area's center and radius.
        /// </summary>
        /// <param name="area">The area to generate a sphere and draw a point from</param>
        /// <returns>The random point</returns>
        private Vector3 GetRandomPoint(Area area)
        {
            Vector3 randomPoint = area.GetCenterPosition() + UnityEngine.Random.insideUnitSphere * area.radius;
            return randomPoint;
        }
    }
}
