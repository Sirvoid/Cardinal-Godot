using Godot;

namespace CardinalEngine {

	[Tool]
	public partial class Cardinal : Node {
		public static Cardinal Instance;

		public override void _Ready() {
			if (Engine.IsEditorHint()) return;

			Instance = this;

			if (SpaceNode == null) SpaceNode = this;
			LinkAttribute.CompileComponentTypes();
			WClient.Run();
		}

		public override void _Process(double delta) {
			if (Engine.IsEditorHint()) return;
			NetworkHandler.Instance.ReadMainThreadQueue();
		}
	}
}
