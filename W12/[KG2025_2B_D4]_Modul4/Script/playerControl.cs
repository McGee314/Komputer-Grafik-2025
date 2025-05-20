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
	[Export]
	public PackedScene BulletScene { get; set; }

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private void _on_finish_area_body_entered(Node3D body){
		if (body == this)
		{
			GD.Print("PLAYER REACHED THE FINISH LINE!");
			// Get GameManager (make sure path is correct for your scene!)
			// Option 1: Absolute Path (adjust if root node isn't 'Playground')
			GameManager gm = GetNode<GameManager>("/root/Playground");
			//example
			//	GameManager gm = GetNode<GameManager>("/root/Node3D");
			// Option 2: If Player is child of Root (where GameManager is) , work on this tutorial
			// GameManager gm = GetParent<GameManager>();

			gm?.AddScore(100); // Use null-conditional operator ?. for safety
			// Maybe disable the finish area?
			// Node finishArea = GetNode("/root/Playground/FinishArea"); // Adjust path
			// finishArea?.GetNode<CollisionShape3D>("CollisionShape3D")?.SetDeferred("disabled", true);
		}

	}

	public override void _Input(InputEvent @event)
	{
		 if (Input.IsActionJustPressed("fire"))
		 {
			 FireBullet();
		 }
	}

	private void FireBullet()
	{
		if (BulletScene == null)
		{
			GD.PrintErr("Player cannot fire: BulletScene not set!");
			return;
		}

		// Find the spawn point marker
		Marker3D spawnPoint = GetNode<Marker3D>("BulletSpawnPoint"); // Use the correct name!
		if (spawnPoint == null)
		{
			 GD.PrintErr("Player cannot fire: BulletSpawnPoint node not found!");
			 return;
		}

		// Create instance of the bullet scene
		Node bulletInstance = BulletScene.Instantiate(); // Instantiate returns base Node type

		// Add the bullet to the main scene tree (so it's independent of player)
		GetTree().Root.AddChild(bulletInstance);

		// Set the bullet's initial position and orientation
		if (bulletInstance is Node3D bullet3D) // Check if it's a 3D node before setting transform
		{
			 bullet3D.GlobalTransform = spawnPoint.GlobalTransform;
			 GD.Print("Fired bullet!");
		}
		else
		{
			 GD.PrintErr("Instantiated bullet scene root is not a Node3D!");
			 bulletInstance.QueueFree(); // Clean up invalid instance
		}
	}

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
