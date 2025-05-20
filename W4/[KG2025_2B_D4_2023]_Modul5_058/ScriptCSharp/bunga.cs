namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

public class Bunga
{
	private Vector2 Pusat;
	private float Ukuran;
	private int JumlahKelopak;
	private bentukdasar _bentukDasar = new bentukdasar();

	public Bunga(Vector2 pusat, float ukuran, int jumlahKelopak)
	{
		Pusat = pusat;
		Ukuran = ukuran;
		JumlahKelopak = jumlahKelopak;
	}

	public void SetJumlahKelopak(int jumlah)
	{
		JumlahKelopak = jumlah;
	}

	public void Gambar(Node2D node)
	{
		float rx = Ukuran;
		float ry = Ukuran / 2;

		float[] sudutKelopak = JumlahKelopak == 8 
			? new float[] { 0, 22.5f, 45, 67.5f, 90, 112.5f, 135, 157.5f } 
			: new float[] { 0, 45, 90, 135 };

		foreach (float sudut in sudutKelopak)
		{
			List<Vector2> kelopak = _bentukDasar.Ellips(Pusat.X, Pusat.Y, rx, ry);
			foreach (Vector2 p in kelopak)
			{
				Vector2 rotated = RotatePoint(p, Pusat, sudut);
				node.DrawCircle(rotated, 1, Colors.White);
			}
		}

		List<Vector2> tengahBunga = _bentukDasar.Lingkaran((int)Pusat.X, (int)Pusat.Y, (int)(Ukuran / 2.5));
		foreach (Vector2 p in tengahBunga)
		{
			node.DrawCircle(p, 1, Colors.Yellow);
		}
	}

	private Vector2 RotatePoint(Vector2 point, Vector2 center, float angle)
	{
		float rad = Mathf.DegToRad(angle);
		float cos = Mathf.Cos(rad);
		float sin = Mathf.Sin(rad);
		float dx = point.X - center.X;
		float dy = point.Y - center.Y;

		return new Vector2(
			center.X + (dx * cos - dy * sin),
			center.Y + (dx * sin + dy * cos)
		);
	}
}
