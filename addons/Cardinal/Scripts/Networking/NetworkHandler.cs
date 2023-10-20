using Godot;
using System;
using System.Collections.Generic;

namespace CardinalEngine {
	internal class NetworkHandler {
        public static NetworkHandler Instance { get; private set; }
        public Action<byte[]> ClientSendData { get; set; }

        private Dictionary<byte, Action> _packets = new();
        private NetReader _netReader = new();
        private Queue<Action> _mainThreadQueue = new();
        private readonly object _queueLock = new();

        public NetworkHandler() {
			Instance = this;

            // Initialize packet handlers
            _packets.Add(0, AddEntity);
			_packets.Add(1, RemoveEntity);
			_packets.Add(2, UpdateEntityPosition);
			_packets.Add(3, SyncComponentField);
			_packets.Add(4, AddComponent);
			_packets.Add(5, RemoveComponent);
			_packets.Add(6, SetNetworkId);
		}

        public void ReadMainThreadQueue() {
            while (true) {
                Action action = null;
                lock (_queueLock) {
					if (_mainThreadQueue.Count > 0) {
						action = _mainThreadQueue.Dequeue();
					} else {
						break;
					}
                }

                action?.Invoke();
            }
        }

		public void OnData(byte[] data) {
            lock (_queueLock) {
                _mainThreadQueue.Enqueue(() => HandlePacket(data));
            }
        }

        public void SendData(byte[] data) {
			ClientSendData?.Invoke(data);
		}

		public void OnConnected() {
			Cardinal.ConnectedToServer = true;
		}

		public void OnDisconnected() {
            Cardinal.ConnectedToServer = false;
        }

        private void HandlePacket(byte[] data) {
            byte opcode = data[0];
            _netReader.Data = data;
            _netReader.Index = 1;
            _packets.GetValueOrDefault(opcode, () => { })();
        }

        //********************//
        //** Packet Handlers **
        //*******************//

        private void AddEntity() {
			int ID = _netReader.ReadInt();
			Vector3 position = _netReader.ReadVector3();
			Cardinal.Instance.AddEntity(ID, position);
		}

		private void RemoveEntity() {
			int ID = _netReader.ReadInt();
			Cardinal.Instance.RemoveEntity(ID);
		}

		private void UpdateEntityPosition() {
			int ID = _netReader.ReadInt();
			Vector3 position = _netReader.ReadVector3();
			Cardinal.Instance.MoveEntity(ID, position);
		}

		private void AddComponent() {
			int entityID = _netReader.ReadInt();
			byte componentID = _netReader.ReadByte();
			int componentTypeID = _netReader.ReadUShort();
			NetEntity entity = Cardinal.Instance.GetEntity(entityID);
			if (entity != null) {
				Type componentType = LinkAttribute.ComponentTypes[componentTypeID];
				var constructor = componentType.GetConstructor(new Type[] { });
				entity.AddComponent(componentID, (NetComponent)constructor.Invoke(null));
			}
		}

        private void RemoveComponent() {
            int entityID = _netReader.ReadInt();
            byte componentID = _netReader.ReadByte();
            NetEntity entity = Cardinal.Instance.GetEntity(entityID);
            if (entity != null) {
                entity.RemoveComponent(componentID);
            }
        }

        private void SyncComponentField() {
			int entityID = _netReader.ReadInt();
			byte componentID = _netReader.ReadByte(); 
			int fieldID = _netReader.ReadByte();
			var fieldValue = _netReader.ReadSupported();

			NetEntity entity = Cardinal.Instance.GetEntity(entityID);
			if (entity != null) {
				NetComponent component = entity.GetComponent(componentID);
				SyncAttribute.SetFieldValueByNetworkId(component, fieldID, fieldValue);
			}
		}

		private void SetNetworkId() {
			Guid NetworkId = new Guid(_netReader.ReadByteArray(_netReader.ReadByte()));
			Cardinal.LocalNetworkID = NetworkId;
		}
	}
}
