using Godot;
using System;

public partial class playerControl : CharacterBody3D
{
	[Export]
	public float Speed { get; set; } = 5.0f;
	[Export]
	public float JumpVelocity { get; set; } = 4.5f;
	[Export]
	public float PushForce { get; set; } = 5.0f; // Add a push force variable for the box

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Apply gravity
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle jump
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Horizontal movement
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_backward", "move_forward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, -inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		// Apply velocity
		Velocity = velocity;

		// Use MoveAndCollide to detect collisions
		var collision = MoveAndCollide(velocity * (float)delta);

		// Check for collision with a RigidBody3D (the box)
		if (collision != null)
		{
			var collider = collision.GetCollider();
			if (collider is RigidBody3D rigidBody)
			{
				// Apply an impulse to the RigidBody3D in the direction of the player's movement
				Vector3 pushDirection = velocity.Normalized();
				pushDirection.Y = 0; // Prevent vertical pushing (e.g., lifting the box)
				rigidBody.ApplyCentralImpulse(pushDirection * PushForce);
			}
		}

		MoveAndSlide();
	}
}
