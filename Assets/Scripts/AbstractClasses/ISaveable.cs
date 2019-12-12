using Utils;

namespace Interfaces
{
    public interface ISaveable
    {
        // object is able to convert its current state to JSON
        Save SaveToJson();
        
        // object can set its own state based on data from the JSON file
        void LoadFromJson(Save input);
    }
}