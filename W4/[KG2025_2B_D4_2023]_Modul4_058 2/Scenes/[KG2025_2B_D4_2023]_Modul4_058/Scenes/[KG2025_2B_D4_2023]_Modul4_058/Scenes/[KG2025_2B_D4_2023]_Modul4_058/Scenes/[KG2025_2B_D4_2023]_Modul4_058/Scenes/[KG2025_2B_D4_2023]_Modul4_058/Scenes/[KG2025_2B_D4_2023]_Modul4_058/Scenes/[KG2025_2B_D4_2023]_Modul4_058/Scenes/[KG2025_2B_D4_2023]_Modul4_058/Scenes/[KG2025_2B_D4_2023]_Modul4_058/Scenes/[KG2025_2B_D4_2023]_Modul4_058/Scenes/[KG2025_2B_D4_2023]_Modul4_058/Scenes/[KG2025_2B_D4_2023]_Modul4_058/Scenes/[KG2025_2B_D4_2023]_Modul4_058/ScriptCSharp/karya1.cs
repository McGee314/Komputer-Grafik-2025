namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

public partial class karya1 : Node2D
{
	private primitif _primitif = new primitif();
	private bentukdasar _bentukdasar;
	private const int MarginLeft = 10;
	private const int MarginTop = 10;

	private int WorldOriginX;
	private int WorldOriginY;

	private Transformasi _transformasi = new Transformasi();
	private float[,] _transformMatrix = new float[3, 3];

	public override void _Ready()
	{
		GD.Print("karya1 _Ready() dipanggil");
		_bentukdasar = new bentukdasar();

		if (_bentukdasar == null)
		GD.PrintErr("ERROR: _bentukdasar masih null!");
		
		WorldOriginX = (int)(GetViewportRect().Size.X / 2);
		WorldOriginY = (int)(GetViewportRect().Size.Y / 2);

		// Inisialisasi matriks transformasi sebagai matriks identitas
		Transformasi.Matrix3x3Identity(_transformMatrix);

		// Contoh penggunaan transformasi
		ApplyTranslation(0, 0); // Translasi
		ApplyScaling(1f, 1f);  // Scaling
		ApplyRotationClockwise(0); // Rotasi searah jarum jam 45 derajat
		ApplyShearing(0f, 0);    // Shearing horizontal

		QueueRedraw();
	}

	public override void _Draw()
	{
		Vector2 WindowSize = GetViewportRect().Size;
		int ScreenWidth = (int)WindowSize[0];
		int ScreenHeight = (int)WindowSize[1];
		int NewMarginLeft = MarginLeft + 40;
		int NewMarginTop = MarginTop + 40;
		int MarginRight = ScreenWidth - (NewMarginLeft);
		int MarginBottom = ScreenHeight - (NewMarginTop);
		MarginPixel(NewMarginLeft, NewMarginTop, MarginRight, MarginBottom);

		DrawAxes();
		DrawShapes();
	}


	private void DrawShapes()
	{
		Godot.Color colorShape = new Godot.Color("#FF5733");

		// Titik pusat layar (sumbu kartesian)
		int centerX = WorldOriginX;
		int centerY = WorldOriginY;
		int offset = 80; // Jarak antar bentuk dari titik pusat

		// 1. **Kuadran I (Persegi)**
		
		List<Vector2> persegi1 = _bentukdasar.Persegi(centerX + offset, centerY - offset, 50);
		ApplyTranslation(50, -50); // Geser ke kanan 50px, ke atas 50px (Kuadran I)
		List<Vector2> transformedPersegi = _transformasi.GetTransformPoint(_transformMatrix, persegi1);
		Transformasi.Matrix3x3Identity(_transformMatrix); // Reset matriks

		// 2. **Kuadran II (Segitiga)**
		List<Vector2> segitiga = _bentukdasar.SegitigaSamaKaki(centerX - offset, centerY - offset, 60, 60);
		ApplyScaling(2f,2f);
		ApplyTranslation(30,30); // Geser ke kiri 30px, ke atas 30px (Kuadran II)
		List<Vector2> transformedSegitiga = _transformasi.GetTransformPoint(_transformMatrix, segitiga);
		Transformasi.Matrix3x3Identity(_transformMatrix); // Reset matriks

		// 3. **Kuadran III (Jajargenjang)**
		List<Vector2> jajargenjang = _bentukdasar.TrapesiumSikuSiku(centerX - offset, centerY + offset, 80, 40, 60);
		ApplyTranslation(30,30); //Geser ke kiri 30 px ke atas 30px
		ApplyScaling(2f, 2f); // Scaling 2 kali lipat
		List<Vector2> transformedJajargenjang = _transformasi.GetTransformPoint(_transformMatrix, jajargenjang);
		Transformasi.Matrix3x3Identity(_transformMatrix); // Reset matriks

		// 4. **Kuadran IV (Trapesium)**
		List<Vector2> trapesium = _bentukdasar.TrapesiumSikuSiku(centerX + offset, centerY + offset, 90, 50, 60);
		ApplyRotationClockwise(90); // Geser ke kanan 50px, ke bawah 50px (Kuadran IV)
		ApplyTranslation(180,180);
		List<Vector2> transformedTrapesium = _transformasi.GetTransformPoint(_transformMatrix, trapesium);
		Transformasi.Matrix3x3Identity(_transformMatrix); // Reset matriks

		// **Gambar hasil transformasi**
		PutPixelAll(persegi1, colorShape);
		PutPixelAll(segitiga, colorShape);
		PutPixelAll(jajargenjang, colorShape);
		PutPixelAll(trapesium, colorShape);
		PutPixelAll(transformedPersegi, colorShape);
		PutPixelAll(transformedSegitiga, colorShape);
		PutPixelAll(transformedJajargenjang, colorShape);
		PutPixelAll(transformedTrapesium, colorShape);
	}


		
	private void DrawAxes()
	{
		Godot.Color axisColor = new Godot.Color("#32CD30"); // Warna putih untuk sumbu

		if (_primitif == null)
		{
			GD.PrintErr("ERROR: _primitif masih null!");
			return;
		}

		// Tentukan area batas margin
		int axisXStart = MarginLeft+40;
		int axisXEnd = (int)GetViewportRect().Size.X - axisXStart;
		int axisYStart = MarginTop+40;
		int axisYEnd = (int)GetViewportRect().Size.Y - axisYStart;

		// Hitung titik tengah berdasarkan margin
		int centerX = (axisXStart + axisXEnd) / 2;
		int centerY = (axisYStart + axisYEnd) / 2;

		// Garis sumbu X (horizontal), dalam margin
		List<Vector2> axisX = _primitif.LineDDA(axisXStart, centerY, axisXEnd, centerY);

		// Garis sumbu Y (vertikal), dalam margin
		List<Vector2> axisY = _primitif.LineDDA(centerX, axisYStart, centerX, axisYEnd);

		PutPixelAll(axisX, axisColor);
		PutPixelAll(axisY, axisColor);
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

	private void MarginPixel(int MarginLeft, int MarginTop, int MarginRight, int MarginBottom){
		Godot.Color color = new Godot.Color("#32CD30");
		var margin = _bentukdasar.Margin(MarginLeft, MarginTop, MarginRight, MarginBottom);
		PutPixelAll(margin, color);
	}
	
	private void MyGarisVariasi()
	{
		Godot.Color color1 = new Godot.Color("#FF2D00"); // Merah
		Godot.Color color2 = new Godot.Color("#FFBD00"); // Biru
		Godot.Color color3 = new Godot.Color("#00FF00"); // Hijau

		var garis1 = _bentukdasar.GarisPola(new Vector2(100, 200), new Vector2(300, 200), "dotted");
		var garis2 = _bentukdasar.GarisPola(new Vector2(100, 250), new Vector2(300, 250), "dashed");
		var garis3 = _bentukdasar.GarisPola(new Vector2(100, 300), new Vector2(300, 300), "dash-dot");

		var gariskanan1 = _bentukdasar.GarisPola(new Vector2(200, 100), new Vector2(200, 300), "dotted");
		var gariskanan2 = _bentukdasar.GarisPola(new Vector2(250, 100), new Vector2(250, 300), "dashed");
		var gariskanan3 = _bentukdasar.GarisPola(new Vector2(300, 100), new Vector2(300, 300), "dash-dot");
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


	// Metode-metode transformasi
	private void ApplyTranslation(float x, float y)
	{
		Vector2 coord = new Vector2(0, 0); // Koordinat referensi untuk translasi
		_transformasi.Translation(_transformMatrix, x, y, ref coord);
	}

	private void ApplyScaling(float x, float y)
	{
		Vector2 coord = new Vector2(WorldOriginX, WorldOriginY); // Titik pusat scaling
		_transformasi.Scaling(_transformMatrix, x, y, coord);
	}

	private void ApplyRotationClockwise(float angle)
	{
		Vector2 coord = new Vector2(WorldOriginX, WorldOriginY); // Titik pusat rotasi
		_transformasi.RotationClockwise(_transformMatrix, angle, coord);
	}

	private void ApplyRotationCounterClockwise(float angle)
	{
		Vector2 coord = new Vector2(WorldOriginX, WorldOriginY); // Titik pusat rotasi
		_transformasi.RotationCounterClockwise(_transformMatrix, angle, coord);
	}

	private void ApplyShearing(float x, float y)
	{
		Vector2 coord = new Vector2(WorldOriginX, WorldOriginY); // Titik referensi untuk shearing
		_transformasi.Shearing(_transformMatrix, x, y, coord);
	}

	private void ApplyReflectionToX()
	{
		Vector2 coord = new Vector2(WorldOriginX, WorldOriginY); // Titik referensi untuk refleksi
		_transformasi.ReflectionToX(_transformMatrix, ref coord);
	}

	private void ApplyReflectionToY()
	{
		Vector2 coord = new Vector2(WorldOriginX, WorldOriginY); // Titik referensi untuk refleksi
		_transformasi.ReflectionToY(_transformMatrix, ref coord);
	}

	private void ApplyReflectionToOrigin()
	{
		Vector2 coord = new Vector2(WorldOriginX, WorldOriginY); // Titik referensi untuk refleksi
		_transformasi.ReflectionToOrigin(_transformMatrix, ref coord);
	}
}
