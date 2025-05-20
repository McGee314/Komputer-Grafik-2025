using Godot;
using System;

public partial class karya2_btn : Button
{
	public void On_BtnBack_Pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Welcome.tscn");
	}
}
