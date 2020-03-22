using System;

namespace Remote.Companion.Packets
{
    [Serializable]
    public class AddOnePacket
    {
        public string type = "addOne";
        public Item data;
    }
}