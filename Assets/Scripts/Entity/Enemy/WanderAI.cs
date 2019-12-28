using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Entity.Enemy
{
    public class WanderAI
    {
        private NavMeshMapper _navMeshMapper;

        public WanderAI()
        {
            _navMeshMapper = new NavMeshMapper();
        }

        /// <summary>
        /// Generates a path for an NPC to wander. The path will not leave the defined wander area
        /// which ensures that the NPC doesn't leave its area and wander to a different plain on the NavMesh
        /// (given that the NPC is placed near a canyon or similar).
        /// </summary>
        /// <param name="wanderArea">The area to generate the path in</param>
        /// <param name="agent">The agent of the NPC to generate the path for</param>
        /// <param name="layermask">The ground</param>
        /// <returns>The next wander path</returns>
        public NavMeshPath GetNextWanderPath(Area wanderArea, NavMeshAgent agent, int layermask)
        {
            NavMeshPath path = new NavMeshPath();
            do
            {
                Vector3 wanderPoint = _navMeshMapper.GetMappedRandomPoint(wanderArea, layermask);
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
        public bool IsInArea(Area area, Vector3 point)
        {
            float distance = Vector3.Distance(area.GetCenterPosition(), point);
            return distance < area.radius;
        }

        /// <summary>
        /// Checks if a path leaves the wander area at any time.
        /// </summary>
        /// <param name="wanderArea">The area the path should be in</param>
        /// <param name="path"></param>
        /// <returns>Whether the whole path is in the wander area</returns>
        private bool IsPathInArea(Area wanderArea, NavMeshPath path)
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
