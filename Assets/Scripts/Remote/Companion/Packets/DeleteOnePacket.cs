using System;

namespace Remote.Companion.Packets
{
    [Serializable]
    public class DeleteOnePacket
    {
        public string type = "deleteOne";
        public Item data;
    }
}