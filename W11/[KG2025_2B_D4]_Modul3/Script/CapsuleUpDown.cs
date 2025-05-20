using Godot;
using System;

public partial class CapsuleUpDown : MeshInstance3D
{
	[Export]
	public float amplitude = 0.5f;

	[Export]
	public float speed = 1.0f;

	private float phase = 0.0f;
	private Vector3 startPos;

	public override void _Ready()
	{
		startPos = Position;
	}

	public override void _Process(double delta)
	{
		phase += (float)delta * speed;
		float yOffset = Mathf.Sin(phase * Mathf.Tau) * amplitude;
		Position = new Vector3(startPos.X, startPos.Y + yOffset, startPos.Z);
	}
}
