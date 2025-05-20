using Godot;
using System;

public partial class SceneMenang : Control
{
	private Button _restartButton;
	
	public override void _Ready()
	{
		// Find and connect the restart button
		_restartButton = GetNode<Button>("RestartButton");
		if (_restartButton != null)
		{
			_restartButton.Pressed += OnRestartButtonPressed;
			GD.Print("Restart button connected successfully");
		}
		else
		{
			GD.PrintErr("RestartButton not found! Check the path.");
		}
	}
	
	private void OnRestartButtonPressed()
	{
		GD.Print("Restart button pressed!");
		
		// Find the GameManager using GetTree()
		var gameManager = GetTree().Root.FindChild("GameManager", true, false);
		if (gameManager != null && gameManager is GameManager manager)
		{
			manager.RestartGame();
		}
		else
		{
			// Alternative approach if direct path doesn't work
			var nodes = GetTree().GetNodesInGroup("game_manager");
			if (nodes.Count > 0 && nodes[0] is GameManager managerFromGroup)
			{
				managerFromGroup.RestartGame();
			}
			else
			{
				GD.PrintErr("GameManager not found! Cannot restart game.");
			}
		}
	}
}
