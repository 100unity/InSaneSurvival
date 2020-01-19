using System.Collections.Generic;
using Buildings;

namespace Utils.Saves
{
    [System.Serializable]
    public class SavedCampsite
    {
        public Campsite campsite;
        public bool isUnlocked;
        public List<SavedBlueprint> blueprints;

        public SavedCampsite(Campsite campsite, bool isUnlocked)
        {
            this.campsite = campsite;
            this.isUnlocked = isUnlocked;
            SetBlueprints(campsite.buildingBlueprints);
        }

        public void SetBlueprints(List<BuildingBlueprint> actualBlueprints)
        {
            blueprints = new List<SavedBlueprint>();
            actualBlueprints.ForEach(bp =>
            {
                blueprints.Add(new SavedBlueprint(bp, bp.Building, bp.IsActive, bp.Building.IsBuilt));
            });
        }

        public void SetState()
        {
            if (isUnlocked && !campsite.IsUnlocked)
            {
                campsite.UnlockBuildingBlueprintsInstantly();
            }
        }
    }
}