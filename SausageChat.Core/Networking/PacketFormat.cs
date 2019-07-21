using System;
using SausageChat.Core;
using System.Collections.Generic;

namespace SausageChat.Core.Networking
{
    public class PacketFormat
    {
        public PacketOption Option { get; set; }
        public Guid Guid { get; set; }
        public string NewName { get; set; }

        public PacketFormat(PacketOption packetOption)
        {
            Option = packetOption;
        }

        // for Json.NET
        #region ShouldSeralize

        public bool ShouldSerializeGuid() => Option == PacketOption.ClientMessage
                                          || Option == PacketOption.NameChange;
        public bool ShouldSerializeOption => false;
        public bool ShouldSerialieNewName => Option == PacketOption.NameChange;

        #endregion
    }
}
