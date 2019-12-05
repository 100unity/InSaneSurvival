using System;
using Constants;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Entity.Enemy
{
    public class WanderAI
    {
        /// <summary>
        /// Generates a path for an NPC to wander. The path will not leave the defined wander area
        /// which ensures that the NPC doesn't leave its area and wander to a different plain on the NavMesh
        /// (given that the NPC is placed near a canyon or similar).
        /// </summary>
        /// <param name="wanderArea">The area to generate the path in</param>
        /// <param name="agent">The agent of the NPC to generate the path for</param>
        /// <param name="layermask">The ground</param>
        /// <returns>The next wander path</returns>
        public NavMeshPath GetNextWanderPath(WanderArea wanderArea, NavMeshAgent agent, int layermask)
        {
            NavMeshPath path = new NavMeshPath();
            do
            {
                Vector3 wanderPoint = GetRandomWanderPoint(wanderArea, layermask);
                agent.CalculatePath(wanderPoint, path);
            }
            while (!IsPathInArea(wanderArea, path));
            return path;
        }

        /// <summary>
        /// Checks if a point is in an area.
        /// </summary>
        /// <param name="area">The area the point should be in</param>
        /// <param name="point">The point to be checked</param>
        /// <returns>Whether the point is in the area</returns>
        public bool IsInArea(WanderArea area, Vector3 point)
        {
            float distance = Vector3.Distance(area.GetCenterPosition(), point);
            return distance < area.radius;
        }

        /// <summary>
        /// Generates a random point in the wander area (with a maximum radius x from a center y on the ground).
        /// Since NavMesh.SamplePosition() can get very expensive, it's better to use it with a small distance
        /// and try multiple times (and only use the random points that are near the NavMesh).
        /// </summary>
        /// <param name="wanderArea">The area to draw the point from</param>
        /// <param name="layermask">The ground</param>
        /// <returns>The random point on the ground</returns>
        private Vector3 GetRandomWanderPoint(WanderArea wanderArea, int layermask)
        {
            for (int i = 0; i < Consts.Enemy.MAPPING_ITERATIONS; i++)
            {
                Vector3 randomPoint = GetRandomPoint(wanderArea);
                // try to map random point onto NavMesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, Consts.Enemy.MAX_NAVMESH_MAPPING_DISTANCE, layermask))
                {
                    return hit.position;
                }
            }
            throw new Exception("Could not map a point onto the NavMesh in " + Consts.Enemy.MAPPING_ITERATIONS + " random points.");
        }

        /// <summary>
        /// Draws a random point from a sphere generated using the wander area's center and radius.
        /// </summary>
        /// <param name="wanderArea">The area to generate a sphere from and draw a point</param>
        /// <returns>The random point</returns>
        private Vector3 GetRandomPoint(WanderArea wanderArea)
        {
            Vector3 randomPoint = wanderArea.GetCenterPosition() + UnityEngine.Random.insideUnitSphere * wanderArea.radius;
            return randomPoint;
        }

        /// <summary>
        /// Checks if a path leaves the wander area at any time.
        /// </summary>
        /// <param name="wanderArea">The area the path should be in</param>
        /// <param name="path"></param>
        /// <returns>Whether the whole path is in the wander area</returns>
        private bool IsPathInArea(WanderArea wanderArea, NavMeshPath path)
        {
            Vector3[] corners = path.corners;
            foreach (Vector3 corner in corners)
            {
                if (!IsInArea(wanderArea, corner))
                    return false;
            }
            return true;
        }
    }
}
