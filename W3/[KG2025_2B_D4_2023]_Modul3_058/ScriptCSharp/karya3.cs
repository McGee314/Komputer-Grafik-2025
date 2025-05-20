namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

public partial class karya3 : Node2D
{
	private primitif _primitif = new primitif();
	private bentukdasar _bentukdasar;
	private const int MarginLeft = 50;
	private const int MarginTop = 50;

	private int WorldOriginX;
	private int WorldOriginY;

	public override void _Ready()
	{
		GD.Print("karya3 _Ready() dipanggil");
		_bentukdasar = new bentukdasar();

		if (_bentukdasar == null)
			GD.PrintErr("ERROR: _bentukdasar masih null!");

		WorldOriginX = (int)(GetViewportRect().Size.X / 2);
		WorldOriginY = (int)(GetViewportRect().Size.Y / 2);

		QueueRedraw();
	}

	public override void _Draw()
	{
		DrawShapes();
	}

	private void DrawShapes()
	{
		Godot.Color colorShape = new Godot.Color("#FF5733");

		List<Vector2> persegi1 = _bentukdasar.Persegi(WorldOriginX + 50, WorldOriginY - 100, 50);
		List<Vector2> segitiga = _bentukdasar.SegitigaSamaKaki(WorldOriginX - 100, WorldOriginY - 100, 100, 100);
		List<Vector2> lingkaran = _bentukdasar.Lingkaran(WorldOriginX - 400, WorldOriginY + 100, 50);
		List<Vector2> trapesium = _bentukdasar.TrapesiumSikuSiku(WorldOriginX + 100, WorldOriginY + 300, 100, 40, 100);

		PutPixelDotted(persegi1, colorShape);
		PutPixelDotted(segitiga, colorShape);
		PutPixelDotted(lingkaran, colorShape);
		PutPixelDotted(trapesium, colorShape);
	}

	private void PutPixel(float x, float y, Godot.Color? color = null)
	{
		if (x < 0 || y < 0 || x > GetViewportRect().Size.X || y > GetViewportRect().Size.Y)
		{
			GD.PrintErr($"Warning: Titik di luar layar ({x}, {y})");
			return;
		}
		Godot.Color actualColor = color ?? Godot.Colors.White;
		DrawCircle(new Vector2(x, y), 1, actualColor);
	}

	private void PutPixelDotted(List<Vector2> dots, Godot.Color? color = null)
	{
		for (int i = 0; i < dots.Count; i += 10) // Hanya menggambar titik-titik dengan selang 2 pixel
		{
			PutPixel(dots[i].X, dots[i].Y, color);
		}
	}
}
