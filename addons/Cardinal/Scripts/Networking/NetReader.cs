using Godot;
using System;
using System.Text;

namespace CardinalEngine {

    internal class NetReader {
        public int Index { get; set; }
        public byte[] Data { get; set; }

        private Func<object>[] _readersByType;

        public NetReader() {
            _readersByType = new Func<object>[12] {
                () => ReadByte(),
                () => ReadSByte(),
                () => ReadShort(),
                () => ReadUShort(),
                () => ReadInt(),
                () => ReadFloat(),
                () => ReadDouble(),
                () => ReadLong(),
                () => ReadULong(),
                () => ReadVector3(),
                () => ReadString(),
                () => ReadGuid()
            };
        }

        public byte ReadByte() {
            return Data[Index++];
        }

        public sbyte ReadSByte() {
            return (sbyte)ReadByte();
        }

        public short ReadShort() {
            return (short)((ReadByte() << 8) | ReadByte());
        }

        public ushort ReadUShort() {
            return (ushort)((ReadByte() << 8) | ReadByte());
        }

        public int ReadInt() {
            int result = 0;
            for (int i = 3; i >= 0; i--) {
                result |= (ReadByte() << (8 * i));
            }
            return result;
        }

        public float ReadFloat() {
            return BitConverter.ToSingle(BitConverter.GetBytes(ReadInt()), 0);
        }

        public double ReadDouble() {
            byte[] data = new byte[8];
            for (int i = 0; i < 8; i++) {
                data[i] = ReadByte();
            }
            return BitConverter.ToDouble(data, 0);
        }

        public long ReadLong() {
            long result = 0;
            for (int i = 7; i >= 0; i--) {
                result |= (long)ReadByte() << (8 * i);
            }
            return result;
        }

        public ulong ReadULong() {
            ulong result = 0;
            for (int i = 7; i >= 0; i--) {
                result |= (ulong)ReadByte() << (8 * i);
            }
            return result;
        }

        public Vector3 ReadVector3() {
            return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
        }

        public string ReadString() {
            ushort length = ReadUShort();
            byte[] stringBytes = new byte[length];

            for (int i = 0; i < length; i++) {
                stringBytes[i] = ReadByte();
            }

            return Encoding.UTF8.GetString(stringBytes);
        }

        public Guid ReadGuid() {
            byte[] bytes = new byte[16];
            Array.Copy(Data, Index, bytes, 0, 16);
            Index += 16;
            return new Guid(bytes);
        }

        public byte[] ReadByteArray(byte length) {
            byte[] bytes = new byte[length];
            Array.Copy(Data, Index, bytes, 0, length);
            Index += length;
            return bytes;
        }

        public object ReadSupported() {
            byte variableIndex = ReadByte();
            if (variableIndex < _readersByType.Length) {
                return _readersByType[variableIndex]();
            } else {
                throw new InvalidOperationException($"NetReader Error: Unsupported type! (Type Index: " + variableIndex + ")");
            }
        }
    }
}