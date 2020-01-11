using System.Collections.Generic;
using Buildings;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Can be used to check the status of all campsites
    /// </summary>
    public class CampsiteManager : Singleton<CampsiteManager>
    {
        [Tooltip("All campsites in the scene")] [SerializeField]
        private List<Campsite> campsites;
    }
}