using Buildings;

namespace Utils.Saves
{
    [System.Serializable]
    public class SavedBlueprint
    {
        public BuildingBlueprint blueprint;
        public Building building;

        public bool blueprintActive;
        public bool buildingActive;

        public SavedBlueprint(BuildingBlueprint blueprint, Building building, bool blueprintActive, bool buildingActive)
        {
            this.blueprint = blueprint;
            this.building = building;
            this.blueprintActive = blueprintActive;
            this.buildingActive = buildingActive;
        }
    }
}