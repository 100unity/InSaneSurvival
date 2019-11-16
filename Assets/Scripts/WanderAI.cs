using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderAI : MonoBehaviour
{
    // maxDistance for mapping random points onto the NavMesh
    private float maxNavMeshMappingDistance;
    // the number of tries to map a random point onto the NavMesh
    private int mappingIterations;

    public WanderAI ()
    {
        // init constants
        maxNavMeshMappingDistance = 1.0f;
        mappingIterations = 30;
    }

    /// <summary>
    /// Draws a random point from a sphere with a certain radius.
    /// </summary>
    /// <param name="center">Center of the sphere</param>
    /// <param name="radius">Radius of the sphere</param>
    /// <returns>The random point</returns>
    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * radius;
        return randomPoint;
    }

    /// <summary>
    /// Generates a random point with a maximum radius x from a center y on the ground.
    /// Since NavMesh.SamplePosition() can get very expensive, it's better to use it with a small distance
    /// and try multiple times (and only use the random points that are near the NavMesh).
    /// </summary>
    /// <param name="center">The center of the area to draw the point from</param>
    /// <param name="range">The maximum distance between the point and center</param>
    /// <param name="layermask">The ground</param>
    /// <returns>The random point on the ground or zero if none was found (within mappingIterations)</returns>
    public Vector3 GetRandomWanderPoint(Vector3 center, float range, int layermask)
    {
        for (int i = 0; i < mappingIterations; i++)
        {
            Vector3 randomPoint = GetRandomPoint(center, range);
            // try to map random point onto NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, maxNavMeshMappingDistance, layermask))
            {
                return hit.position;
            }
        }
        return Vector3.zero;
    }

    /// <summary>
    /// WIP TODO
    /// </summary>
    /// <param name="centerPosition"></param>
    /// <param name="wanderRadius"></param>
    /// <param name="wanderPoint"></param>
    public void GetNearestRunnablePoint(Vector3 centerPosition, float wanderRadius, Vector3 wanderPoint)
    {
        throw new NotImplementedException();
    }
}
