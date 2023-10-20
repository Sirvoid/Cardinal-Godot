using System;
using System.Collections.Generic;
using Godot;

namespace CardinalEngine {
    public partial class Cardinal {
        [Export] public Node SpaceNode { get; set; }
        public Dictionary<int, NetEntity> Entities { get; private set; } = new();
        public NetEntity AddEntity(int id, Vector3 position) {
            if (!Entities.ContainsKey(id)) {
                NetEntity entity = new NetEntity();
                entity.ID = id;
                SpaceNode.AddChild(entity);
                entity.Position = position;
                Entities.Add(id, entity);
                return entity;
            }

            return null;
        }

        public void RemoveEntity(int id) {
            if (Entities.ContainsKey(id)) {
                SpaceNode.RemoveChild(Entities[id]);
                Entities.Remove(id);
            }
        }

        public void MoveEntity(int id, Vector3 position) {
            if (Entities.ContainsKey(id)) {
                Entities[id].Position = position;
            }
        }

        public NetEntity GetEntity(int id) {
            if (Entities.ContainsKey(id)) {
                return Entities[id];
            } else {
                return null;
            }
        }
    }
}
