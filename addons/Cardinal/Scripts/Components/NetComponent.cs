using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace CardinalEngine {
    public partial class NetComponent : Node {
        public NetEntity Entity { get; private set; }
        public byte ID { get; private set; }

        public NetComponent() { }

        public void SendCommand(byte commandID, params object[] args) {
            NetworkHandler.Instance.SendData(Packet.SendComponentCommand(this, commandID, args));
        }

        internal void SetEntity(NetEntity netEntity, byte componentID) {
            Entity = netEntity;
            ID = componentID;
        }

    }
}
