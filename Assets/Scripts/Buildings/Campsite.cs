using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Buildings
{
    public class Campsite : MonoBehaviour
    {
        [Header("Campsite")] [SerializeField] private List<BuildingBlueprint> buildingBlueprints;

        [SerializeField] private GameObject campsiteModel;
        [SerializeField] private ParticleSystem unlockPS;
        [SerializeField] private float unlockBuildingTimer;

        public bool IsUnlocked => isUnlocked;

        [SerializeField] [HideInInspector] private bool isUnlocked;

        public void UnlockBuildingBlueprints()
        {
            isUnlocked = true;
            unlockPS.Play();
            CoroutineManager.Instance.WaitForSeconds(unlockBuildingTimer, () =>
            {
                campsiteModel.SetActive(false);
                foreach (BuildingBlueprint blueprint in buildingBlueprints)
                {
                    if (blueprint.Building.IsBuild)
                        blueprint.ShowBuilding();
                    else
                        blueprint.ShowBlueprint();
                }
            });
        }
    }
}