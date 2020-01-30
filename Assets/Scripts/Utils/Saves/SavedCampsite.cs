using System;
using System.Collections.Generic;

namespace Utils.Saves
{
    [Serializable]
    public class SavedCampsite
    {
        public string id;
        public bool isUnlocked;
        public List<SavedBlueprint> blueprints;
    }
}