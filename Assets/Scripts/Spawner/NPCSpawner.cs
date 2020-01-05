using Entity.Enemy;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Spawner
{
    public class NPCSpawner : MonoBehaviour
    {
        [Tooltip("The area NPCs should be spawned in")]
        [SerializeField]
        protected Area spawnArea;

        [Tooltip("The maximum NPCs that can be spawned in the area concurrently")]
        [SerializeField]
        protected int maximumConcurrentEnemies;

        [Tooltip("The probability to spawn an NPC for each frame")]
        [SerializeField]
        [Range(0, 0.1f)]
        protected float spawnProbability;

        [Tooltip("The NPC to spawn")]
        [SerializeField]
        protected EnemyController toSpawn;

        [Tooltip("The main camera")]
        [SerializeField]
        protected Camera mainCamera;

        protected Probability _probability;
        protected NavMeshMapper _navMeshMapper;

        protected void Awake()
        {
            _probability = new Probability();
            _navMeshMapper = new NavMeshMapper();
        }

        /// <summary>
        /// Spawns all NPCs on game start.
        /// </summary>
        protected virtual void OnEnable()
        {
            InitNPCs();
        }

        /// <summary>
        /// Spawn all NPCs at game start.
        /// </summary>
        protected void InitNPCs()
        {
            for (int i = 0; i < maximumConcurrentEnemies; i++)
            {
                TrySpawn();
            }
        }

        protected virtual void Update()
        {
            if (transform.childCount < maximumConcurrentEnemies && _probability.GetProbability(spawnProbability))
                TrySpawn();
        }

        /// <summary>
        /// Try spawning an NPC. The NPC might not consist in the scene 
        /// (but will be destroyed before it is made visible if it's in the player's sight).
        /// </summary>
        protected virtual void TrySpawn()
        {
            Vector3 spawnLocation = _navMeshMapper.GetMappedRandomPoint(spawnArea, NavMesh.AllAreas);
            // setting a random rotation doesn't seem to work with a NavMeshAgent, but since NPCs start walking in a random direction immediately it doesn't really matter
            EnemyController spawned = Instantiate(toSpawn, spawnLocation, Quaternion.identity, transform);
            Renderer renderer = spawned.Renderer;
            // could RenderChecker instead, but no advantages for now
            if (!renderer.InFrustum(mainCamera))
                renderer.enabled = true;
            else
                Destroy(spawned.gameObject);
        }

        /// <summary>
        /// Draws disc and wires to display spawn area.
        /// </summary>
        protected void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0, 0, 0, 0.3f);
            UnityEditor.Handles.DrawSolidDisc(spawnArea.GetCenterPosition(), transform.up, spawnArea.radius);
            // draw this as well, so that you keep in mind, the height of the spawner is actually not irrelevant
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(spawnArea.GetCenterPosition(), spawnArea.radius);
        }
    }
}