namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

public partial class Bunga
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

public partial class karya2 : Node2D
{
	private List<Bunga> bungaList = new List<Bunga>();
	private bool semuaBunga8Kelopak = false; // Status apakah semua bunga harus menjadi 8 kelopak

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

		// Membuat 4 bunga tipe 1 (awal: 8 elips)
		for (int i = 0; i < 4; i++)
		{
			bungaList.Add(new Bunga(new Vector2(width * (i + 1) / 5, height / 3), 100, 8));
		}

		// Membuat 6 bunga tipe 2 (awal: 4 elips)
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
