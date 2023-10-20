using Godot;
using System;
using CardinalEngine;

[Link(0)]
public partial class TestComponent : NetComponent
{
	[Sync(0)] 
	public Vector3 Rot;

	[Sync(1)]
	public int Health;

	[Sync(2)]
	public string Username;

	Observer observer;

	public override void _Ready() {
		observer = Entity.GetComponent<Observer>();
		MeshInstance3D cube = new MeshInstance3D();
		BoxMesh box = new BoxMesh();
		cube.Mesh = box;
		GetParent().AddChild(cube);
	}

	public override void _Process(double delta) {
		if (observer.NetworkID != Cardinal.LocalNetworkID) return;

		if (Input.IsKeyPressed(Key.W)) {
			SendCommand(0, new Vector3(0, 0.1f, 0));
		} else if(Input.IsKeyPressed(Key.S)) {
            SendCommand(0, new Vector3(0, -0.1f, 0));
		} else if(Input.IsKeyPressed(Key.A)) {
            SendCommand(0, new Vector3(-0.1f, 0, 0));
		} else if (Input.IsKeyPressed(Key.D)) {
            SendCommand(0, new Vector3(0.1f, 0, 0));
		}
	}
}
