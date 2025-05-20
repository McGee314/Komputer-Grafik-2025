using Godot;
using System;



public partial class RotatingCube : MeshInstance3D
{
	// Called when the node enters the scene tree for the first time.


	[Export] // Add this attribute
	public float rotationSpeed = 50.0f;

	public override void _Ready()
	{
		GD.Print("Hello from the Rotating Cube!"); // Add this line
		

	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
public override void _Process(double delta)
{
	

	// Modify the rotation based on speed and delta time
	RotationDegrees += new Vector3(0, 1, 0) * rotationSpeed * (float)delta;
}


}
