namespace Godot;

using Godot;
using System;

public partial class karya2 : Node2D
{

	private bentukdasar _bentukDasar = new bentukdasar();
	
	private const int MarginLeft = 50;
	private const int MarginTop = 50;

	public override void _Ready()
	{

	}

	public override void _Draw()
	{
		Vector2 WindowSize = GetViewportRect().Size;
		int ScreenWidth = (int)WindowSize[0];
		int ScreenHeight = (int)WindowSize[1];
		int MarginRight = ScreenWidth - MarginLeft;
		int MarginBottom = ScreenHeight - MarginTop;

		MarginPixel(MarginLeft, MarginTop, MarginRight, MarginBottom);
		MyGarisVariasi();
	}


	private void MarginPixel(int MarginLeft, int MarginTop, int MarginRight, int MarginBottom){
		Godot.Color color = new Godot.Color("#32CD30");
		var margin = _bentukDasar.Margin(MarginLeft, MarginTop, MarginRight, MarginBottom);
		PutPixelAll(margin, color);
	}

	private void PutPixel(float x, float y, Godot.Color? color = null)
	{
		// Provide a default color if 'color' is null
		Godot.Color actualColor = color ?? Godot.Colors.White;
		Godot.Vector2[] points = new Godot.Vector2[]{new Godot.Vector2(Mathf.Round(x), Mathf.Round(y))};
		Godot.Vector2[] uvs = new Godot.Vector2[]
		{
			Vector2.Zero, Vector2.Down, Vector2.One, Vector2.Right
		};

		DrawPrimitive(points, new Godot.Color[]{ actualColor }, uvs);
	}

	private void MyGarisVariasi()
	{
		Godot.Color color1 = new Godot.Color("#FF2D00"); // Merah
		Godot.Color color2 = new Godot.Color("#FFBD00"); // Biru
		Godot.Color color3 = new Godot.Color("#00FF00"); // Hijau

		var garis1 = _bentukDasar.GarisPola(new Vector2(100, 200), new Vector2(300, 200), "dotted");
		var garis2 = _bentukDasar.GarisPola(new Vector2(100, 250), new Vector2(300, 250), "dashed");
		var garis3 = _bentukDasar.GarisPola(new Vector2(100, 300), new Vector2(300, 300), "dash-dot");

		var gariskanan1 = _bentukDasar.GarisPola(new Vector2(200, 100), new Vector2(200, 300), "dotted");
		var gariskanan2 = _bentukDasar.GarisPola(new Vector2(250, 100), new Vector2(250, 300), "dashed");
		var gariskanan3 = _bentukDasar.GarisPola(new Vector2(300, 100), new Vector2(300, 300), "dash-dot");
		PutPixelAll(garis1, color1);
		PutPixelAll(garis2, color2);
		PutPixelAll(garis3, color3);
		PutPixelAll(gariskanan1, color1);
		PutPixelAll(gariskanan2, color2);
		PutPixelAll(gariskanan3, color3);
	}

	private void PutPixelAll(System.Collections.Generic.List<Vector2> dot, Godot.Color? color = null)
	{
		foreach (Vector2 point in dot)
		{
			PutPixel(point[0], point[1], color);
		}
	}

	public override void _ExitTree()
	{
		GD.Print($"_bentukDasar is null in _ExitTree(): {_bentukDasar == null}");
		_bentukDasar?.Dispose(); // Pastikan _bentukDasar tidak null sebelum Dispose
		_bentukDasar = null;
		GD.Print($"_bentukDasar is null in _ExitTree(): {_bentukDasar == null}");
		base._ExitTree();
	}
	
}	
