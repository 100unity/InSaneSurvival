using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Const
{
    // maxDistance for mapping random points onto the NavMesh
    public const float ENEMY_MAX_NAVMESH_MAPPING_DISTANCE = 1.0f;

    // just a high number to ensure random point generation for wandering
    public const int ENEMY_NAVMESH_MAPPING_ITERATIONS = 500; 
}

