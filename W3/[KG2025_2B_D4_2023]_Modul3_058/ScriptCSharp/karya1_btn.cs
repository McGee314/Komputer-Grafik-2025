using Godot;
using System;

public partial class karya1_btn : Button
{
	private void _on_BtnBack_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Welcome.tscn");
	}
}
