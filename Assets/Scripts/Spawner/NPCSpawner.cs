using Constants;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Spawner
{
    public class NPCSpawner : MonoBehaviour
    {
        [Tooltip("The area NPCs should be spawned in")]
        [SerializeField]
        private Area spawnArea;

        [Tooltip("The maximum NPCs that can be spawned in the area concurrently")]
        [SerializeField]
        private int maximumConcurrentEnemies;

        [Tooltip("The probability to spawn an NPC for each frame")]
        [SerializeField]
        [Range(0, 0.1f)]
        private float spawnProbability;

        [Tooltip("The NPC to spawn")]
        [SerializeField]
        private GameObject toSpawn;

        [Tooltip("The ground to spawn NPCs on")]
        [SerializeField]
        private LayerMask spawnLayer;

        [Tooltip("The main camera")]
        [SerializeField]
        private Camera mainCamera;

        private Probability _probability;
        private NavMeshMapper _navMeshMapper;

        private void Awake()
        {
            _probability = new Probability();
            _navMeshMapper = new NavMeshMapper();
        }

        /// <summary>
        /// Spawns all NPCs on game start.
        /// </summary>
        private void OnEnable()
        {
            InitNPCs();
        }

        /// <summary>
        /// Spawn all NPCs at game start.
        /// </summary>
        private void InitNPCs()
        {
            for (int i = 0; i < maximumConcurrentEnemies; i++)
            {
                TrySpawn();
            }
        }
        
        private void Update()
        {
            if (transform.childCount < maximumConcurrentEnemies && _probability.GetProbability(spawnProbability))
                TrySpawn();
        }

        /// <summary>
        /// Try spawning an NPC. The NPC might not consist in the scene 
        /// (but will be destroyed before it is made visible if it's in the player's sight).
        /// </summary>
        private void TrySpawn()
        {
            Vector3 spawnLocation = _navMeshMapper.GetMappedRandomPoint(spawnArea, spawnLayer.value);
            // setting a random rotation doesn't seem to work with a NavMeshAgent, but since NPCs start walking in a random direction immediately it doesn't really matter
            GameObject spawned = Instantiate(toSpawn, spawnLocation, Quaternion.identity, transform);
            MeshRenderer renderer = spawned.GetComponentInChildren<MeshRenderer>();
            // could RenderChecker instead, but no advantages for now
            if (!renderer.InFrustum(mainCamera))
                renderer.enabled = true;
            else
                Destroy(spawned);
        }

        /// <summary>
        /// Draws disc and wires to display spawn area.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0, 0, 0, 0.3f);
            UnityEditor.Handles.DrawSolidDisc(spawnArea.GetCenterPosition(), transform.up, spawnArea.radius);
            // draw this as well, so that you keep in mind, the height of the spawner is actually not irrelevant
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(spawnArea.GetCenterPosition(), spawnArea.radius);
        }
    }
}