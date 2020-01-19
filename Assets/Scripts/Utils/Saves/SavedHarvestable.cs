using Interactables;

namespace Utils.Saves
{
    [System.Serializable]
    public class SavedHarvestable
    {
        public Harvestable harvestable;
        public bool isRespawning;
        public double respawnTimePassed;

        public SavedHarvestable(Harvestable harvestable, bool isRespawning, double respawnTimePassed)
        {
            this.harvestable = harvestable;
            this.isRespawning = isRespawning;
            this.respawnTimePassed = respawnTimePassed;
        }

        public void SetState()
        {
            harvestable.SetFromSave(isRespawning, respawnTimePassed);
        }
    }
}