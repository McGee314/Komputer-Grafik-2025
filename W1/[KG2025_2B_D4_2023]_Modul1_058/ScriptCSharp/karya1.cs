using Godot;
using System;
using System.Collections.Generic;

public partial class karya1 : Node2D
{
	private primitif _primitif = new primitif();
	
	private const int MarginLeft = 50;
	private const int MarginTop = 50;
	private const int WorldOriginX = 400; // Titik tengah world (sesuai kebutuhan)
	private const int WorldOriginY = 300;

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
		DrawLines();
	}

	private void MarginPixel(int MarginLeft, int MarginTop, int MarginRight, int MarginBottom)
	{
		Godot.Color color = new Godot.Color("#32CD30");
		List<Vector2> margin = new List<Vector2>
		{
			new Vector2(MarginLeft, MarginTop),
			new Vector2(MarginRight, MarginTop),
			new Vector2(MarginRight, MarginBottom),
			new Vector2(MarginLeft, MarginBottom),
			new Vector2(MarginLeft, MarginTop)
		};
		PutPixelAll(margin, color);
	}

	private void DrawLines()
	{
		Godot.Color colorDDA = new Godot.Color("#FF0000"); // Merah untuk DDA
		Godot.Color colorBresenham = new Godot.Color("#0000FF"); // Biru untuk Bresenham

		// Contoh koordinat Kartesian
		float x1 = -100, y1 = -50, x2 = 100, y2 = 50;

		// Konversi ke koordinat World
		(float wx1, float wy1) = ConvertToWorld(x1, y1);
		(float wx2, float wy2) = ConvertToWorld(x2, y2);

		// Menggunakan algoritma DDA
		List<Vector2> ddaPoints = _primitif.LineDDA(wx1, wy1, wx2, wy2);
		PutPixelAll(ddaPoints, colorDDA);

		// Menggunakan algoritma Bresenham
		List<Vector2> bresenhamPoints = _primitif.LineBresenham(wx1, wy1, wx2, wy2);
		PutPixelAll(bresenhamPoints, colorBresenham);
	}

	private (float, float) ConvertToWorld(float x, float y)
	{
		float worldX = WorldOriginX + x;
		float worldY = WorldOriginY - y; // Inversi Y karena koordinat layar
		return (worldX, worldY);
	}

	private void PutPixel(float x, float y, Godot.Color? color = null)
	{
		Godot.Color actualColor = color ?? Godot.Colors.White;
		DrawCircle(new Vector2(x, y), 1, actualColor);
	}

	private void PutPixelAll(List<Vector2> dots, Godot.Color? color = null)
	{
		foreach (Vector2 point in dots)
		{
			PutPixel(point.X, point.Y, color);
		}
	}
}
