using System.Collections.Generic;
using Buildings;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Can be used to check the status of all buildings
    /// </summary>
    public class BuildingManager : Singleton<BuildingManager>
    {
        [SerializeField] private List<Campsite> campsites;
    }
}