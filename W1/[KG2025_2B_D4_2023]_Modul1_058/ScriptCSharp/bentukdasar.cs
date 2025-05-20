namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class bentukdasar: Node2D, IDisposable
{
	private primitif _primitif = new primitif();

	public override void _Ready()
	{
	}
	
	public List<Vector2> Margin(int MarginLeft, int MarginTop, int MarginRight,  int MarginBottom){
		if (_primitif == null)
		{
			GD.PrintErr("Node Primitif belum di-assign!");
			return new List<Vector2>();
		}
		
		List<Vector2> res = new List<Vector2>();

		res.AddRange(_primitif.LineDDA(MarginLeft, MarginTop, MarginRight, MarginTop));
		res.AddRange(_primitif.LineDDA(MarginLeft, MarginBottom, MarginRight, MarginBottom));
		res.AddRange(_primitif.LineDDA(MarginLeft, MarginTop, MarginLeft, MarginBottom));
		res.AddRange(_primitif.LineDDA(MarginRight, MarginTop, MarginRight, MarginBottom));
		
		return res;
	}
	
	
	public List<Vector2> Persegi(float x, float y, float ukuran)
	{
		if (_primitif == null)
		{
			GD.PrintErr("Node Primitif belum di-assign!");
			return new List<Vector2>();
		}

		List<Vector2> garis1 = _primitif.LineBresenham(x, y, x + ukuran, y);
		List<Vector2> garis2 = _primitif.LineBresenham(x + ukuran, y, x + ukuran, y + ukuran);
		List<Vector2> garis3 = _primitif.LineBresenham(x + ukuran, y + ukuran, x, y + ukuran);
		List<Vector2> garis4 = _primitif.LineBresenham(x, y + ukuran, x, y);

		// Gabungkan titik-titik garis (hilangkan duplikat di ujung garis)
		List<Vector2> persegi = new List<Vector2>();
		persegi.AddRange(garis1);
		persegi.AddRange(garis2.GetRange(1, garis2.Count - 1)); // Mulai dari index 1 untuk hindari duplikat
		persegi.AddRange(garis3.GetRange(1, garis3.Count - 1));
		persegi.AddRange(garis4.GetRange(1, garis4.Count - 1));

		// Sekarang 'persegi' berisi titik-titik untuk menggambar persegi.
		// Anda bisa menggunakan titik-titik ini untuk menggambar di canvas atau menggunakan Line2D.

		// foreach (Vector2 point in persegi)
		// {
		//     GD.Print($"x: {point[0]}, y: {point[1]}"); // Contoh: Print koordinat titik
		// }
		return persegi;

	}

	public List<Vector2> PersegiPanjang(float x, float y, float panjang, float lebar)
	{
		if (_primitif == null)
		{
			GD.PrintErr("Node Primitif belum di-assign!");
			return new List<Vector2>();
		}
		// Mirip dengan GambarPersegi, tetapi gunakan 'panjang' dan 'lebar'.
		List<Vector2> garis1 = _primitif.LineBresenham(x, y, x + panjang, y);
		List<Vector2> garis2 = _primitif.LineBresenham(x + panjang, y, x + panjang, y + lebar);
		List<Vector2> garis3 = _primitif.LineBresenham(x + panjang, y + lebar, x, y + lebar);
		List<Vector2> garis4 = _primitif.LineBresenham(x, y + lebar, x, y);

		List<Vector2> persegiPanjang = new List<Vector2>();
		persegiPanjang.AddRange(garis1);
		persegiPanjang.AddRange(garis2.GetRange(1, garis2.Count - 1)); // Mulai dari index 1 untuk hindari duplikat
		persegiPanjang.AddRange(garis3.GetRange(1, garis3.Count - 1));
		persegiPanjang.AddRange(garis4.GetRange(1, garis4.Count - 1));

		// foreach (Vector2 point in persegiPanjang)
		// {
		//     GD.Print($"x: {point[0]}, y: {point[1]}"); // Contoh: Print koordinat titik
		// }

		return persegiPanjang;
	}

	public void Dispose() // Implement Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this); // Untuk mencegah finalizer dipanggil lagi
	}

	protected virtual void Dispose(bool disposing) // Fungsi Dispose() yang sebenarnya
	{
		if (disposing)
		{
			GD.Print($"_primitif is null in _ExitTree(): {_primitif == null}");
			_primitif?.Dispose(); // Dispose _primitif
			_primitif = null; // Set ke null (opsional)
			GD.Print($"_primitif is null in _ExitTree(): {_primitif == null}");
		}
		// bebaskan unmanaged resources di sini jika ada.
	}


}
