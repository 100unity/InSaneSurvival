using Entity.Enemy;
using Entity.Player;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using System.Linq;
using System.Collections.Generic;
using Boo.Lang;

namespace Spawner
{
    public class SanitySpawnController : NPCSpawner
    {
        [Tooltip("Scales the spawn probability.")]
        [SerializeField]
        private float spawnDimensionScale;

        [Tooltip("Scales the despawn probability.")]
        [SerializeField]
        private float despawnDimensionScale;

        [Tooltip("Scales spawn and despawn probability.")]
        [SerializeField]
        private AnimationCurve probabilityScaleCurve;

        [Tooltip("Spawn probability is scaled with a factor 1-x for every active/existing sanity NPC.")]
        [SerializeField]
        private float activeSpawnPenalty;


        private readonly Dictionary<Renderer, GameObject> _spawned = new Dictionary<Renderer, GameObject>();
        // keep track of the sanity internally so that calculating the spawnProbability is easier
        private int _sanity;

        protected override void OnEnable()
        {
            // no InitNPCs()
            PlayerState.OnPlayerSanityUpdate += OnSanityUpdate;
        }

        private void OnDisable()
        {
            PlayerState.OnPlayerSanityUpdate -= OnSanityUpdate;
        }
        
        protected override void Update()
        {
            base.Update();
            if (_spawned.Count > 0)
                DestroyActiveNPCs();
        }

        /// <summary>
        /// Destroy sanity NPCs that are out of sight or despawn them randomly.
        /// </summary>
        private void DestroyActiveNPCs()
        {
            // calculate despawn probability
            float despawnProbability = probabilityScaleCurve.Evaluate(1 - _sanity) * despawnDimensionScale;
            if (_probability.GetProbability(despawnProbability))
            {
                Destroy(_spawned.First().Value);
                _spawned.Remove(_spawned.First().Key);
            }
            DestroyOutOfSight();
        }

        /// <summary>
        /// Destroy automatically if player can't see NPC anymore.
        /// </summary>
        private void DestroyOutOfSight()
        {
            List toRemove = new List();
            foreach (Renderer renderer in _spawned.Keys)
            {
                if (!renderer.InFrustum(mainCamera))
                {
                    Destroy(_spawned[renderer]);
                    toRemove.Add(renderer);
                }
            }
            foreach (Renderer renderer in toRemove)
                _spawned.Remove(renderer);
            if (toRemove.Count > 0)
                CalculateSpawnProbability(_sanity);
        }

        /// <summary>
        /// Spawn NPCs even when they are in line of sight, recalculate spawn probability.
        /// </summary>
        protected override void TrySpawn()
        {
            Vector3 spawnLocation = _navMeshMapper.GetMappedRandomPoint(spawnArea, NavMesh.AllAreas);
            EnemyController spawned = Instantiate(toSpawn, spawnLocation, Quaternion.identity, transform);
            Renderer renderer = spawned.Renderer;
            renderer.enabled = true;
            _spawned.Add(renderer, spawned.gameObject);
            // assign the spawned NPC a wander area that is moving with the player
            spawned.WanderArea.center = spawnArea.center;
            CalculateSpawnProbability(_sanity);
        }

        private void OnSanityUpdate(int sanity)
        {
            _sanity = sanity;
            CalculateSpawnProbability(_sanity);
        }

        /// <summary>
        /// Calculate spawn probability and applies active spawn penalties.
        /// </summary>
        /// <param name="sanity">Current sanity</param>
        private void CalculateSpawnProbability(int sanity)
        {
            float scale = probabilityScaleCurve.Evaluate(sanity / 100f) * spawnDimensionScale;
            foreach (Renderer r in _spawned.Keys)
                scale = scale * (1 - activeSpawnPenalty);
            spawnProbability = (100 - sanity) * scale;
        }
    }
}
