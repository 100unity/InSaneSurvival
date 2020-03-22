using System;

namespace Remote.Companion.Packets
{
    [Serializable]
    public class InventoryPacket
    {
        public string type = "inventory";
        public Item[] data;
    }
}