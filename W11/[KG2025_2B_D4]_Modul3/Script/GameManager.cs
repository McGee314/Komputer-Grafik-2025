using Godot;
using System;

// Attached to the root node (e.g., "Playground")
public partial class GameManager : Node3D
{
	// Private fields for state
	private int _currentScore = 0;
	[Export] public int ScoreToWin = 50; // Make win condition configurable
	private bool _isGameOver = false;
	[Export] public Control WinPanel; // Reference to your win screen UI

	// ...

	// Reference to the UI element (set in _Ready)
	private Label _scoreDisplayLabel;

	
	 // --- GameManager Process Method ---
	public override void _Process(double delta)
	{

		{
		// Input for testing score (REMOVE LATER)
		if (Input.IsActionJustPressed("add_score_debug")) { AddScore(10); }

		// Input for testing groups (REMOVE LATER)
		// Define "affect_pushables_debug" in Input Map, assign 'G' key
		if (Input.IsActionJustPressed("affect_pushables_debug"))
		{
			MakePushablesJump();
		}
	}
}
	// --- END GameManager Process Method ---


	private void MakePushablesJump()
	{
		GD.Print("Applying impulse to all 'pushables'...");

		// Get all nodes in the group from the current scene tree
		var nodesInGroup = GetTree().GetNodesInGroup("pushables");

		// Loop through the found nodes
		foreach (Node node in nodesInGroup)
		{
			// IMPORTANT: Check if the node is the correct type before using its methods
			if (node is RigidBody3D pushableRb)
			{
				// Apply an upward impulse
				pushableRb.ApplyCentralImpulse(Vector3.Up * 3.0f); // Adjust force as needed
				GD.Print($"- Applied impulse to {pushableRb.Name}");
			}
			else
			{
				// Good practice to log if something unexpected is in the group
				GD.Print($"- Node {node.Name} is in 'pushables' group but is not a RigidBody3D.");
			}
		}
	}

	// ... (rest of GameManager script) ...
	


	// Called once when the node enters the scene tree.
	public override void _Ready()
	{
		 // Add to existing _Ready method
		AddToGroup("game_manager");
		
		// Rest of your _Ready code...
		_scoreDisplayLabel = GetNode<Label>("CanvasLayer/Label");

		if (_scoreDisplayLabel == null)
		{
			GD.PrintErr("GameManager Error: Score Label node not found at path 'CanvasLayer/Label'. Check scene structure and node names.");
		}
		else
		{
			 // Set initial display
			 UpdateScoreDisplay();
		}
	}

	// Public method other scripts can call to change the score
	public void AddScore(int pointsToAdd)
	{
		if (_isGameOver) return; // Stop if game already ended

		_currentScore += pointsToAdd;
		GD.Print($"Score increased by {pointsToAdd}. New score: {_currentScore}");
		UpdateScoreDisplay();

		// Check for win
		if (_currentScore >= ScoreToWin)
		{
			TriggerWinCondition();
		}
	}

private void TriggerWinCondition()
{
	if (_isGameOver) return; // Ensure it only triggers once

	_isGameOver = true;
	GD.Print($"----- YOU WIN! Final Score: {_currentScore} -----");

	// Load the win scene as an overlay
	var winScene = GD.Load<PackedScene>("res://SceneMenang.tscn");
	if (winScene != null)
	{
		// Instance the scene
		Node winInstance = winScene.Instantiate();
		
		// Add it to the scene tree
		// You'll likely want to add it to a CanvasLayer for proper UI positioning
		if (HasNode("CanvasLayer"))
		{
			GetNode("CanvasLayer").AddChild(winInstance);
		}
		else
		{
			// Add directly to current node if no CanvasLayer
			AddChild(winInstance);
		}
		
		// Optional: Pause the game if needed
		// GetTree().Paused = true;
	}
	else
	{
		GD.PrintErr("Failed to load win scene: res://SceneMenang.tscn");
	}
}

	// Private helper method to update the UI text
	private void UpdateScoreDisplay()
	{
		if (_scoreDisplayLabel != null)
		{
			// Use C# string interpolation to format the text
			_scoreDisplayLabel.Text = $"Score: {_currentScore}";
		}
	}

	// Add this method to your GameManager class
	public void RestartGame()
	{
		// Reset game state
		_currentScore = 0;
		_isGameOver = false;
		
		// Update the score display
		UpdateScoreDisplay();
		
		// Remove the win scene overlay
		if (HasNode("CanvasLayer"))
		{
			foreach (Node child in GetNode("CanvasLayer").GetChildren())
			{
				// Only remove the win scene, not other UI elements
				if (child.Name.ToString().Contains("SceneMenang"))
				{
					child.QueueFree();
				}
			}
		}
		
		// Restore all hidden targets
		var hiddenTargets = GetTree().GetNodesInGroup("hidden_targets");
		foreach (Node target in hiddenTargets)
		{
			if (target is RigidBody3D targetRb)
			{
				// Make visible again
				targetRb.Visible = true;
				
				// Re-enable collision
				targetRb.SetDeferred("monitoring", true);
				targetRb.SetDeferred("monitorable", true);
				
				// Reset physics state
				targetRb.Freeze = true;
				targetRb.LinearVelocity = Vector3.Zero;
				targetRb.AngularVelocity = Vector3.Zero;
				
				// Remove from hidden group
				targetRb.RemoveFromGroup("hidden_targets");
			}
			else if (target is Node3D targetNode)
			{
				// Handle non-RigidBody targets
				// (existing code)
				// Make visible again
				targetNode.Visible = true;
				
				// Re-enable collision
				if (targetNode is CollisionObject3D collisionBody)
				{
					collisionBody.SetDeferred("monitoring", true);
					collisionBody.SetDeferred("monitorable", true);
				}
				
				// Remove from hidden group
				targetNode.RemoveFromGroup("hidden_targets");
			}
		}
		
		// Unpause the game if it was paused
		GetTree().Paused = false;
		
		GD.Print("Game restarted! All targets restored.");
	}
}
