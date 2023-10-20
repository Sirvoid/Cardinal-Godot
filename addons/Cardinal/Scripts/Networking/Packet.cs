using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CardinalEngine {

    public class Packet {
        private static NetWriter _writer = new NetWriter();

        public static byte[] SendComponentCommand(NetComponent component, byte commandID, params object[] parameters) {
            int packetSize = 7;

            foreach (var param in parameters) {
                if (param == null) continue;
                Type type = param.GetType();
                if (type == typeof(string)) {
                    packetSize += 1 + 2 + Encoding.UTF8.GetByteCount((string)param);
                } else {
                    packetSize += 1 + Marshal.SizeOf(type);
                }
            }

            _writer.NewPacket(packetSize);
            _writer.WriteByte((byte)Opcode.SendCommand);
            _writer.WriteInt(component.Entity.ID);
            _writer.WriteByte(component.ID);
            _writer.WriteByte(commandID);
            foreach (var param in parameters) {
                if (param == null) continue;
                _writer.WriteSupported(param.GetType(), param);
            }
            return _writer.Data;
        }
    }
}