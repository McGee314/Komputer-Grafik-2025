using Godot;
using System;

public partial class welcome: Control
{
	// Called when the node enters the scene tree for the first time.
public override void _Ready()
{
	GD.Print("Scene Welcome telah dimuat!");
}


	private void _on_BtnKarya1_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/karya1.tscn");
	}

	private void _on_BtnKarya2_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/karya2.tscn");
	}
	
	private void _on_BtnKarya3_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/karya3.tscn");
	}
	
	private void _on_BtnAbout_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/About.tscn");
	}

	private void _on_BtnGuide_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Guide.tscn");
	}

	private void _on_BtnExit_pressed()
	{
		GetTree().Quit();
	}
}
