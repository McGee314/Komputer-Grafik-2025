namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

public partial class karya1 : Node2D
{
	private primitif _primitif = new primitif();
	private bentukdasar _bentukdasar;
	private const int MarginLeft = 50;
	private const int MarginTop = 50;

	private int WorldOriginX; // Tidak lagi `const`
	private int WorldOriginY; 

	public override void _Ready()
	{
		GD.Print("karya1 _Ready() dipanggil");
		_bentukdasar = new bentukdasar();

		if (_bentukdasar == null)
			GD.PrintErr("ERROR: _bentukdasar masih null!");

		// Hitung titik tengah layar secara dinamis
		WorldOriginX = (int)(GetViewportRect().Size.X / 2);
		WorldOriginY = (int)(GetViewportRect().Size.Y / 2);

		QueueRedraw();
	}

	private void DrawEllipses()
	{
		// Get screen dimensions
		Vector2 WindowSize = GetViewportRect().Size;
		float centerX = WindowSize.X / 2;
		float centerY = WindowSize.Y / 2;
		
		// Draw a centered ellipse
		var ellipsePoints = _bentukdasar.Ellips(centerX, centerY, 150, 100);
		PutPixelAll(ellipsePoints, new Color(1, 0, 1)); // Magenta
		
		// Draw ellipses in different positions with different dimensions
		// Horizontal ellipse
		var horizontalEllipse = _bentukdasar.Ellips(centerX - 200, centerY, 80, 40);
		PutPixelAll(horizontalEllipse, new Color(1, 0.7f, 0)); // Orange
		
		// Vertical ellipse
		var verticalEllipse = _bentukdasar.Ellips(centerX + 200, centerY, 40, 80);
		PutPixelAll(verticalEllipse, new Color(0, 0.7f, 1)); // Light blue
		
		// Draw smaller ellipses in each quadrant
		float quadrantOffsetX = 150;
		float quadrantOffsetY = 120;
		
		var ellipse1 = _bentukdasar.Ellips(centerX + quadrantOffsetX, centerY - quadrantOffsetY, 60, 30);
		var ellipse2 = _bentukdasar.Ellips(centerX - quadrantOffsetX, centerY - quadrantOffsetY, 30, 60);
		var ellipse3 = _bentukdasar.Ellips(centerX - quadrantOffsetX, centerY + quadrantOffsetY, 45, 45);
		var ellipse4 = _bentukdasar.Ellips(centerX + quadrantOffsetX, centerY + quadrantOffsetY, 70, 40);
		
		PutPixelAll(ellipse1, new Color(0.5f, 1, 0.5f)); // Light green
		PutPixelAll(ellipse2, new Color(1, 0.5f, 0.5f)); // Light red
		PutPixelAll(ellipse3, new Color(0.5f, 0.5f, 1)); // Light blue
		PutPixelAll(ellipse4, new Color(1, 1, 0.5f)); // Light yellow
	}


	public override void _Draw()
	{
		Vector2 WindowSize = GetViewportRect().Size;
		int ScreenWidth = (int)WindowSize[0];
		int ScreenHeight = (int)WindowSize[1];
		int MarginRight = ScreenWidth - MarginLeft;
		int MarginBottom = ScreenHeight - MarginTop;

		//MarginPixel(MarginLeft, MarginTop, MarginRight, MarginBottom);
		DrawShapes();
		DrawAxes();
		DrawEllipses();
	
	}

	private void DrawShapes()
	{
		Godot.Color colorShape = new Godot.Color("#FF5733"); // Warna bentuk

		// Kuadran I (kanan atas)
		List<Vector2> persegi1 = _bentukdasar.Persegi(WorldOriginX + 50, WorldOriginY - 100, 50);

		// Kuadran II (kiri atas)
		List<Vector2> segitiga = _bentukdasar.SegitigaSamaKaki(WorldOriginX - 100, WorldOriginY - 100, 100, 100);
		List<Vector2> segitiga2 = _bentukdasar.SegitigaSamaKaki(WorldOriginX - 200, WorldOriginY - 200, 100, 100);
		// Kuadran III (kiri bawah)
		List<Vector2> lingkaran = _bentukdasar.Lingkaran(WorldOriginX - 400, WorldOriginY + 100, 50);
		// Kuadran IV (kanan bawah)
		List<Vector2> trapesium = _bentukdasar.TrapesiumSikuSiku(WorldOriginX + 100, WorldOriginY + 300, 100,40,100);
		List<Vector2> trapesium2 = _bentukdasar.TrapesiumSikuSiku(WorldOriginX - 100, WorldOriginY + 300, 100,40,100);
		PutPixelAll(persegi1, colorShape);
		PutPixelAll(segitiga, colorShape);
		PutPixelAll(segitiga2, colorShape);
		PutPixelAll(lingkaran, colorShape);
		PutPixelAll(trapesium, colorShape);
		PutPixelAll(trapesium2, colorShape);
	}
	
		//private void MarginPixel(int MarginLeft, int MarginTop, int MarginRight, int MarginBottom){
		//Godot.Color color = new Godot.Color("#32CD30");
		//var margin = _bentukDasar.Margin(MarginLeft, MarginTop, MarginRight, MarginBottom);
		//PutPixelAll(margin, color);
	//}
	
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
		
	private void DrawAxes()
	{
		Godot.Color axisColor = new Godot.Color(1, 1, 1); // Warna putih untuk sumbu

		// Pastikan `_primitif` telah diinisialisasi sebelum digunakan
		if (_primitif == null)
		{
			GD.PrintErr("ERROR: _primitif masih null!");
			return;
		}

		// Garis sumbu X (horizontal, dari kiri ke kanan)
		List<Vector2> axisX = _primitif.LineDDA(0, WorldOriginY, (int)GetViewportRect().Size.X, WorldOriginY);

		// Garis sumbu Y (vertikal, dari atas ke bawah)
		List<Vector2> axisY = _primitif.LineDDA(WorldOriginX, 0, WorldOriginX, (int)GetViewportRect().Size.Y);

		PutPixelAll(axisX, axisColor);
		PutPixelAll(axisY, axisColor);
	}

	private void PutPixelAll(List<Vector2> dots, Godot.Color? color = null)
	{
		foreach (Vector2 point in dots)
		{
			PutPixel(point.X, point.Y, color);
		}
	}

	public override void _Process(double delta)
	{
		QueueRedraw();
	}
}
