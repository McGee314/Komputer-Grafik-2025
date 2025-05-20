using Godot;
using System.Collections.Generic;

public partial class Karya2 : Node2D
{
	private List<Bunga> bungaList = new List<Bunga>();
	private bool semuaBunga8Kelopak = false;

	public override void _Ready()
	{
		BuatBunga();
	}

	private void BuatBunga()
	{
		bungaList.Clear();
		Vector2 WindowSize = GetViewportRect().Size;
		float width = WindowSize.X;
		float height = WindowSize.Y;

		for (int i = 0; i < 4; i++)
		{
			bungaList.Add(new Bunga(new Vector2(width * (i + 1) / 5, height / 3), 100, 8));
		}

		for (int i = 0; i < 6; i++)
		{
			bungaList.Add(new Bunga(new Vector2(width * (i + 1) / 7, 2 * height / 3), 100, 4));
		}
	}

	public override void _Draw()
	{
		foreach (var bunga in bungaList)
		{
			bunga.Gambar(this);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed)
		{
			if (eventKey.Keycode == Key.Key1)
			{
				semuaBunga8Kelopak = false;
				BuatBunga();
				QueueRedraw();
			}
			else if (eventKey.Keycode == Key.Key2)
			{
				semuaBunga8Kelopak = true;
				foreach (var bunga in bungaList)
				{
					bunga.SetJumlahKelopak(8);
				}
				QueueRedraw();
			}
		}
	}
}
