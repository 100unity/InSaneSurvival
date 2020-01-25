using System.Collections;
using Managers;
using UnityEngine;

namespace Interactables
{
    public class ParticleHarvestable : Harvestable
    {
        [Tooltip("The particle system to disable while the harvestable is currently respawning")] [SerializeField]
        private ParticleSystem particles;

        protected override void Awake()
        {
            MainCam = Camera.main;
            OwnCollider = GetComponent<MeshCollider>();

            if (destroyAfterHarvest)
                Parent = transform.parent.gameObject;

            // if item was respawning before save, keep it respawning
            if (isRespawning)
                CoroutineManager.Instance.WaitForSeconds(1.0f / 60.0f, () => StartCoroutine(Respawn()));
        }

        protected override IEnumerator Respawn()
        {
            particles.Stop();
            OwnCollider.enabled = false;

            isRespawning = true;

            while (respawnTimePassed < respawnTime)
            {
                respawnTimePassed += Time.deltaTime;
                yield return null;
            }

            isRespawning = false;
            respawnTimePassed = 0;

            OwnCollider.enabled = true;
            particles.Play();
        }
    }
}