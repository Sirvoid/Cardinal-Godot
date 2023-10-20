#if TOOLS
using Godot;
using System;

[Tool]
public partial class CardinalEngineSetup : EditorPlugin {
	public override void _EnterTree() {
		var cardinalScript = GD.Load<Script>("res://addons/Cardinal/Scripts/Cardinal.cs");
		var cardinalIcon = GD.Load<Texture2D>("res://addons/Cardinal/CardinalIcon.png");
		AddCustomType("Cardinal", "Node", cardinalScript, cardinalIcon);
	}

	public override void _ExitTree() {
		RemoveCustomType("Cardinal");
	}
}

#endif
