namespace Godot;
using Godot;
using System;
using System.Collections.Generic;
public class Bunga
{
	// Change Pusat to have a public setter
	public Vector2 Pusat { get; set; }
	public float Ukuran { get; private set; }
	public int JumlahKelopak { get; private set; }
	private bentukdasar _bentukDasar = new bentukdasar();
	
	// Precompute untuk bentuk kelopak dasar relatif terhadap (0,0)
	private List<Vector2> baseKelopak;
	private List<Vector2> baseTengah;
	public Bunga(Vector2 pusat, float ukuran, int jumlahKelopak)
	{
		Pusat = pusat;
		Ukuran = ukuran;
		JumlahKelopak = jumlahKelopak;
		float rx = Ukuran;
		float ry = Ukuran / 2;
		baseKelopak = _bentukDasar.Ellips(0, 0, rx, ry);
		baseTengah = _bentukDasar.Lingkaran(0, 0, (int)(Ukuran / 2.5));
	}
	public void Gambar(CanvasItem canvas, Transform2D transform = default)
	{
		if (transform == default)
			transform = Transform2D.Identity;
		canvas.DrawSetTransformMatrix(transform);
		// Tentukan sudut-sudut kelopak
		float[] sudutKelopak = JumlahKelopak == 8 
			? new float[] { 0, 22.5f, 45, 67.5f, 90, 112.5f, 135, 157.5f } 
			: new float[] { 0, 45, 90, 135 };
		foreach (float sudut in sudutKelopak)
		{
			// Buat transformasi untuk masing-masing kelopak
			Transform2D angleTransform = Transform2D.Identity.Rotated(Mathf.DegToRad(sudut));
			foreach (Vector2 p in baseKelopak)
			{
				// Terapkan rotasi dan offset pusat
				Vector2 rotated = angleTransform * p + Pusat;
				canvas.DrawCircle(rotated, 1, Colors.White); // Ubah ukuran jika terlalu kecil
			}
		}
		foreach (Vector2 p in baseTengah)
		{
			canvas.DrawCircle(p + Pusat, 1, Colors.Yellow);
		}
	}
}
