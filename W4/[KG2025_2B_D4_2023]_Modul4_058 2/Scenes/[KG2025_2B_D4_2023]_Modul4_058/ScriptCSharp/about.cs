using Godot;

public partial class ButtonBack : Control
{
	public override void _Ready()
	{
		// Initialization code if needed
	}

	private void OnBtnBackPressed()
	{
		Error result = GetTree().ChangeSceneToFile("res://Scenes/Welcome.tscn");
		if (result != Error.Ok)
		{
			GD.Print("Scene Tidak Ada");
		}
	}
}
