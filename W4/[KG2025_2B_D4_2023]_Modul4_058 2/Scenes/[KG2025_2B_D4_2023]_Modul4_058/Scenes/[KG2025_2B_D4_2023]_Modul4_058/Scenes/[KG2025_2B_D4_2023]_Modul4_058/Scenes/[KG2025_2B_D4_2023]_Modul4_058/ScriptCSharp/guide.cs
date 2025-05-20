using Godot;

public partial class SceneController : Control
{
	public override void _Ready()
	{
		// Kosong, sesuai dengan GDScript
	}

	private void OnBtnBackPressed()
	{
		var result = GetTree().ChangeSceneToFile("res://Scenes/Welcome.tscn");
		if (result != Error.Ok)
		{
			GD.Print("Scene Tidak Ada");
		}
	}
}
