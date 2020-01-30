using System;

namespace Utils.Saves
{
    [Serializable]
    public class SavedBlueprint
    {
        public string blueprintId;
        public string buildingId;

        public bool blueprintActive;
        public bool buildingActive;
    }
}