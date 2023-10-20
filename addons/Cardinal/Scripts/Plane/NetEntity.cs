using Godot;
using System.Collections.Generic;

namespace CardinalEngine {

	[Tool]
	public partial class NetEntity : Node3D {
		public int ID { get; set; }
		private Dictionary<byte, NetComponent> _components = new();

		public NetComponent GetComponent(byte componentID) {
			if (_components.ContainsKey(componentID)) {
				return _components[componentID];
			} else {
				return null;
			}
		}

		public T GetComponent<T>() where T : NetComponent {
			foreach (var component in _components.Values) {
				if (component is T typedComponent) {
					return typedComponent;
				}
			}
			return default;
		}

		internal void AddComponent(byte componentID, NetComponent component) {
			if (!_components.ContainsKey(componentID)) {
				_components.Add(componentID, component);
				component.SetEntity(this, componentID);
				AddChild(component);
			}
		}

		internal void RemoveComponent(byte componentID) {
			if (_components.ContainsKey(componentID)) {
				NetComponent component = _components[componentID];
				RemoveChild(component);
				_components.Remove(componentID);
			}
		}
	}
}
