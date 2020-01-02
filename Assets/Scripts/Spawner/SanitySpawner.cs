using Entity.Enemy;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Spawner
{
    public class SanitySpawner : NPCSpawner
    {
        protected override void TrySpawn()
        {
            
            Vector3 spawnLocation = _navMeshMapper.GetMappedRandomPoint(spawnArea, NavMesh.AllAreas);
            EnemyController spawned = Instantiate(toSpawn, spawnLocation, Quaternion.identity, transform);
            Renderer renderer = spawned.Renderer;
            renderer.enabled = true;
            if (!renderer.InFrustum(mainCamera))
                renderer.enabled = true;
            else
                Destroy(spawned.gameObject);
            spawned.WanderArea.center = spawnArea.center;
            
            //base.TrySpawn();
            //spawned.WanderArea.center = spawnArea.center;
        }
    }
}
