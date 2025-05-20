using Godot;
using System;

public partial class Bullet : Area3D
{
	[Export] public float Speed = 20.0f;
	[Export] public float Lifetime = 4.0f; // Max time bullet lives (seconds)

	private double _timeAlive = 0.0;

	public override void _Ready()
	{
		var mesh = GetNode<MeshInstance3D>("MeshInstance3D");
		if (mesh != null)
		{
			mesh.RotationDegrees = new Vector3(90, 0, 0);
		}
	}

	// Called every physics frame. Use for movement.
	public override void _PhysicsProcess(double delta)
	{
		// Godot convention: -Z is forward for nodes.
		Vector3 forwardDirection = -GlobalTransform.Basis.Z;
		GlobalPosition += forwardDirection * Speed * (float)delta;

		_timeAlive += delta;
		if (_timeAlive >= Lifetime)
		{
			QueueFree(); // Destroy bullet if lifetime expires
		}
	}

	// Signal callback method (needs to be connected in the editor)
	private async void _on_body_entered(Node3D body)
	{
		// Check if we hit a target
		if (body.IsInGroup("targets"))
		{
			// Target hit - find GameManager using groups
			var gameManagers = GetTree().GetNodesInGroup("game_manager");
			if (gameManagers.Count > 0 && gameManagers[0] is GameManager manager)
			{
				manager.AddScore(10);
				GD.Print($"Target hit: {body.Name} - Score +10!");
			}
			
			// If target is a RigidBody3D, unfreeze it to make it fall
			if (body is RigidBody3D targetRb)
			{
				targetRb.Freeze = false;
				
				// Apply a small impulse to make it move more visibly
				targetRb.ApplyCentralImpulse(new Vector3(0, 0, -2.0f));
				
				// Wait for physics to take effect before hiding
				await ToSignal(GetTree().CreateTimer(0.7f), "timeout");
			}
			
			// Hide the target instead of destroying it
			body.Visible = false;
			
			// Disable collision
			if (body is CollisionObject3D collisionBody)
			{
				collisionBody.SetDeferred("monitoring", false);
				collisionBody.SetDeferred("monitorable", false);
			}
			
			// Add to a "hidden_targets" group for easy restoration
			body.AddToGroup("hidden_targets");
		}
		
		// Always destroy the bullet
		QueueFree();
	}

	// Optional: Handle hitting other Areas if needed
	private void _on_area_entered(Area3D area)
	{
		GD.Print($"Bullet hit area: {area.Name}");
		QueueFree(); // Destroy bullet on hitting another Area3D too
	}
}
