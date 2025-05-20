namespace Godot;

using Godot;

using System;

public partial class karya2 : Node2D
{
	private Bunga bunga; 
	private Bunga bunga2; //Membuat bunga dengan 4 kelopak
	private Bunga bunga3; //Testing membuat bunga dengan 5 kelopak 
	

	public override void _Ready()
	{
		// Inisialisasi objek bunga pertama dengan pusat di (650, 350), ukuran 100, dan 8 kelopak
		bunga = new Bunga(new Vector2(650, 350), 100, 8);
		
		//inisialisasi objek bunga kedua dengan pusat di (300, 300), ukuran 100 dan kelopak 4
		bunga2 = new Bunga(new Vector2(300, 300), 100, 4);
		
		bunga3 = new Bunga(new Vector2(150, 150), 100, 8);

		// Inisialisasi objek bunga kedua dengan pusat di (300, 300), ukuran 100, dan 4 kelopak
		//bunga2 = new Bunga(new Vector2(300, 300), 100, 4);
	}

	public override void _Draw()
	{
		// Gambar bunga pertama
		bunga.Gambar(this);
		bunga2.Gambar(this);
		bunga3.Gambar(this);

		// Gambar bunga kedua
		//bunga2.Gambar(this);
	}
}
