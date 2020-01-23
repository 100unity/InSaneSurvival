using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Buildings
{
    [System.Serializable]
    public class Campsite : MonoBehaviour
    {
        [Header("Campsite")] [Tooltip("All building blueprints of this campsite")] [SerializeField]
        public List<BuildingBlueprint> buildingBlueprints;

        [Tooltip("The model of the campsite. Used to hide it after unlocking")] [SerializeField]
        private GameObject campsiteModel;

        [Tooltip("The particle system for unlocking this campsite")] [SerializeField]
        private ParticleSystem unlockPS;

        [Tooltip("Delay after which the blueprints appear. Should be synced with the particle system")] [SerializeField]
        private float unlockBuildingTimer;

        /// <summary>
        /// Whether this campsite is already unlocked.
        /// </summary>
        public bool IsUnlocked => isUnlocked;

        public event CampsiteStatus OnUnlock;

        public delegate void CampsiteStatus();

        /// <summary>
        /// Whether this campsite is already unlocked. Used for json.
        /// </summary>
        [SerializeField] [HideInInspector] private bool isUnlocked;

        /// <summary>
        /// Unlocks blueprints and shows the particle system.
        /// </summary>
        public void UnlockBuildingBlueprints()
        {
            isUnlocked = true;
            OnUnlock?.Invoke();
            unlockPS.Play();
            CoroutineManager.Instance.WaitForSeconds(unlockBuildingTimer, () =>
            {
                campsiteModel.SetActive(false);
                foreach (BuildingBlueprint blueprint in buildingBlueprints)
                {
                    if (blueprint.Building.IsBuilt)
                        blueprint.ShowBuilding();
                    else
                        blueprint.ShowBlueprint();
                }
            });
        }

        /// <summary>
        /// Unlocks the blueprints instantly.
        /// </summary>
        public void UnlockBuildingBlueprintsInstantly()
        {
            isUnlocked = true;
            OnUnlock?.Invoke();
            campsiteModel.SetActive(false);
            foreach (BuildingBlueprint blueprint in buildingBlueprints)
            {
                if (blueprint.Building.IsBuilt)
                    blueprint.ShowBuilding();
                else
                    blueprint.ShowBlueprint();
            }
        }
    }
}