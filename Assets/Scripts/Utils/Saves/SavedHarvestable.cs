using System;

namespace Utils.Saves
{
    [Serializable]
    public class SavedHarvestable
    {
        public string id;
        public bool isRespawning;
        public double respawnTimePassed;
    }
}