using Entity.Enemy;
using Entity.Player;
using System.Collections;
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
        public delegate void PlayerStateChanged(int newValue);

        [SerializeField]
        private float spawnDimensionScale;

        // at sanity = 45
        [SerializeField]
        private float despawnDimensionScale;

        [SerializeField]
        private AnimationCurve probabilityScaleCurve;

        [Tooltip("Spawn probability is scaled with a factor 1-x for every active/existing sanity NPC.")]
        [SerializeField]
        private float activeSpawnPenalty;


        private Dictionary<Renderer, GameObject> _spawned;
        // keep track of the sanity internally so that calculating the spawnProbability is easier
        private int _sanity;

        protected override void Awake()
        {
            base.Awake();
            _spawned = new Dictionary<Renderer, GameObject>();
        }

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

        private void DestroyActiveNPCs()
        {
            float despawnProbability = probabilityScaleCurve.Evaluate(1 - _sanity) * despawnDimensionScale;
            if (_probability.GetProbability(despawnProbability))
            {
                Destroy(_spawned.First().Value);
                _spawned.Remove(_spawned.First().Key);
            }
            DestroyOutOfSight();
        }

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

        protected override void TrySpawn()
        {
            Vector3 spawnLocation = _navMeshMapper.GetMappedRandomPoint(spawnArea, NavMesh.AllAreas);
            EnemyController spawned = Instantiate(toSpawn, spawnLocation, Quaternion.identity, transform);
            Renderer renderer = spawned.Renderer;
            renderer.enabled = true;
            _spawned.Add(renderer, spawned.gameObject);
            /*if (!renderer.InFrustum(mainCamera))
                renderer.enabled = true;
            else
                Destroy(spawned.gameObject);*/
            spawned.WanderArea.center = spawnArea.center;
            CalculateSpawnProbability(_sanity);
            //base.TrySpawn();
            //spawned.WanderArea.center = spawnArea.center;
        }

        private void OnSanityUpdate(int sanity)
        {
            _sanity = sanity;
            CalculateSpawnProbability(_sanity);
        }

        private void CalculateSpawnProbability(int sanity)
        {
            float scale = probabilityScaleCurve.Evaluate(sanity / 100f) * spawnDimensionScale;
            foreach (Renderer r in _spawned.Keys)
                scale = scale * (1 - activeSpawnPenalty);
            spawnProbability = (100 - sanity) * scale;
        }
    }
}
