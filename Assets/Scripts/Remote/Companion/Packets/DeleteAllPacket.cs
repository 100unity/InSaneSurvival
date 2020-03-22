using System;

namespace Remote.Companion.Packets
{
    [Serializable]
    public class DeleteAllPacket
    {
        public string type = "deleteAll";
        public Item data;
    }
}