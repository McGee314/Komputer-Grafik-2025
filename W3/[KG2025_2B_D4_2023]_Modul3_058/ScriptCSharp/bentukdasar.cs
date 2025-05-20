namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class bentukdasar : Node2D, IDisposable
{
	private primitif _primitif = new primitif();

	public override void _Ready()
	{
	}


public List<Vector2> Ellips(float xCenter, float yCenter, float rx, float ry)
	{
		if (_primitif == null)
		{
			GD.PrintErr("Node Primitif belum di-assign!");
			return new List<Vector2>();
		}

		List<Vector2> points = new List<Vector2>();
		
		float x = 0;
		float y = ry;
		
		// Initial decision parameter for region 1
		float d1 = ry * ry - rx * rx * ry + 0.25f * rx * rx;
		float dx = 2 * ry * ry * x;
		float dy = 2 * rx * rx * y;
		
		// Region 1
		while (dx < dy)
		{
			// Plot points in all four quadrants
			PlotEllipsePoints(points, xCenter, yCenter, x, y);
			
			// Increment x
			x++;
			dx += 2 * ry * ry;
			
			// Check if decision parameter is negative
			if (d1 < 0)
			{
				d1 += dx + ry * ry;
			}
			else
			{
				y--;
				dy -= 2 * rx * rx;
				d1 += dx - dy + ry * ry;
			}
		}
		
		// Decision parameter for region 2
		float d2 = ry * ry * (x + 0.5f) * (x + 0.5f) + rx * rx * (y - 1) * (y - 1) - rx * rx * ry * ry;
		
		// Region 2
		while (y >= 0)
		{
			// Plot points in all four quadrants
			PlotEllipsePoints(points, xCenter, yCenter, x, y);
			
			// Decrement y
			y--;
			dy -= 2 * rx * rx;
			
			// Check if decision parameter is positive
			if (d2 > 0)
			{
				d2 += rx * rx - dy;
			}
			else
			{
				x++;
				dx += 2 * ry * ry;
				d2 += dx - dy + rx * rx;
			}
		}
		
		return points;
	}
	
	private void PlotEllipsePoints(List<Vector2> points, float xCenter, float yCenter, float x, float y)
	{
		// Plot points in all four quadrants
		points.Add(new Vector2(xCenter + x, yCenter + y));
		points.Add(new Vector2(xCenter - x, yCenter + y));
		points.Add(new Vector2(xCenter + x, yCenter - y));
		points.Add(new Vector2(xCenter - x, yCenter - y));
	}
	
	public List<Vector2> Margin(int MarginLeft, int MarginTop, int MarginRight, int MarginBottom)
	{
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

		List<Vector2> persegi = new List<Vector2>();
		persegi.AddRange(garis1);
		persegi.AddRange(garis2.GetRange(1, garis2.Count - 1));
		persegi.AddRange(garis3.GetRange(1, garis3.Count - 1));
		persegi.AddRange(garis4.GetRange(1, garis4.Count - 1));

		return persegi;
	}

	public List<Vector2> PersegiPanjang(float x, float y, float panjang, float lebar)
	{
		if (_primitif == null)
		{
			GD.PrintErr("Node Primitif belum di-assign!");
			return new List<Vector2>();
		}

		List<Vector2> garis1 = _primitif.LineBresenham(x, y, x + panjang, y);
		List<Vector2> garis2 = _primitif.LineBresenham(x + panjang, y, x + panjang, y + lebar);
		List<Vector2> garis3 = _primitif.LineBresenham(x + panjang, y + lebar, x, y + lebar);
		List<Vector2> garis4 = _primitif.LineBresenham(x, y + lebar, x, y);

		List<Vector2> persegiPanjang = new List<Vector2>();
		persegiPanjang.AddRange(garis1);
		persegiPanjang.AddRange(garis2.GetRange(1, garis2.Count - 1));
		persegiPanjang.AddRange(garis3.GetRange(1, garis3.Count - 1));
		persegiPanjang.AddRange(garis4.GetRange(1, garis4.Count - 1));

		return persegiPanjang;
	}

	public List<Vector2> Lingkaran(int xCenter, int yCenter, int radius)
	{
		List<Vector2> points = new List<Vector2>();
		int x = 0;
		int y = radius;
		int p = 1 - radius;

		CirclePlotPoints(xCenter, yCenter, x, y, points);

		while (x < y)
		{
			x++;
			if (p < 0)
				p += 2 * x + 1;
			else
			{
				y--;
				p += 2 * (x - y) + 1;
			}
			CirclePlotPoints(xCenter, yCenter, x, y, points);
		}

		return points;
	}

	public List<Vector2> SegitigaSamaKaki(float x, float y, float alas, float tinggi)
	{
		if (_primitif == null)
		{
			GD.PrintErr("Node Primitif belum di-assign!");
			return new List<Vector2>();
		}

		Vector2 titikBawahKiri = new Vector2(x, y);
		Vector2 titikBawahKanan = new Vector2(x + alas, y);
		Vector2 titikAtas = new Vector2(x + alas / 2, y - tinggi);

		List<Vector2> garis1 =
			_primitif.LineBresenham(titikBawahKiri.X, titikBawahKiri.Y, titikBawahKanan.X, titikBawahKanan.Y);

		List<Vector2> garis2 = _primitif.LineBresenham(titikBawahKiri.X, titikBawahKiri.Y, titikAtas.X, titikAtas.Y);

		List<Vector2> garis3 = _primitif.LineBresenham(titikBawahKanan.X, titikBawahKanan.Y, titikAtas.X, titikAtas.Y);

		List<Vector2> segitiga = new List<Vector2>();
		segitiga.AddRange(garis1);
		segitiga.AddRange(garis2.GetRange(1, garis2.Count - 1)); 
		segitiga.AddRange(garis3.GetRange(1, garis3.Count - 1));

		return segitiga;
	}

	public List<Vector2> TrapesiumSikuSiku(float x, float y, float alasBawah, float alasAtas, float tinggi)
	{
		if (_primitif == null)
		{
			GD.PrintErr("Node Primitif belum di-assign!");
			return new List<Vector2>();
		}
		
		Vector2 titikBawahKiri = new Vector2(x, y);
		Vector2 titikBawahKanan = new Vector2(x + alasBawah, y);
		Vector2 titikAtasKiri = new Vector2(x, y - tinggi);
		Vector2 titikAtasKanan = new Vector2(x + alasAtas, y - tinggi);
		
		List<Vector2> garis1 =
			_primitif.LineBresenham(titikBawahKiri.X, titikBawahKiri.Y, titikBawahKanan.X,
				titikBawahKanan.Y);
		List<Vector2> garis2 =
			_primitif.LineBresenham(titikBawahKiri.X, titikBawahKiri.Y, titikAtasKiri.X,
				titikAtasKiri.Y);
		List<Vector2> garis3 =
			_primitif.LineBresenham(titikAtasKiri.X, titikAtasKiri.Y, titikAtasKanan.X, titikAtasKanan.Y);
		List<Vector2> garis4 =
			_primitif.LineBresenham(titikAtasKanan.X, titikAtasKanan.Y, titikBawahKanan.X,
				titikBawahKanan.Y);
		
		List<Vector2> trapesium = new List<Vector2>();
		trapesium.AddRange(garis1);
		trapesium.AddRange(garis2.GetRange(1, garis2.Count - 1));
		trapesium.AddRange(garis3.GetRange(1, garis3.Count - 1));
		trapesium.AddRange(garis4.GetRange(1, garis4.Count - 1));

		return trapesium;
	}

	public List<Vector2> TrapesiumSamaKaki(float x, float y, float alasBawah, float alasAtas, float tinggi)
	{
		if (_primitif == null)
		{
			GD.PrintErr("Node Primitif belum di-assign!");
			return new List<Vector2>();
		}
		
		float offset = (alasBawah - alasAtas) / 2; 
		Vector2 titikBawahKiri = new Vector2(x, y);
		Vector2 titikBawahKanan = new Vector2(x + alasBawah, y);
		Vector2 titikAtasKiri = new Vector2(x + offset, y - tinggi);
		Vector2 titikAtasKanan = new Vector2(x + alasBawah - offset, y - tinggi);
		
		List<Vector2> garis1 = _primitif.LineBresenham(titikBawahKiri.X, titikBawahKiri.Y, titikBawahKanan.X, titikBawahKanan.Y);
		List<Vector2> garis2 = _primitif.LineBresenham(titikBawahKiri.X, titikBawahKiri.Y, titikAtasKiri.X, titikAtasKiri.Y); 
		List<Vector2> garis3 = _primitif.LineBresenham(titikAtasKiri.X, titikAtasKiri.Y, titikAtasKanan.X, titikAtasKanan.Y); 
		List<Vector2> garis4 = _primitif.LineBresenham(titikAtasKanan.X, titikAtasKanan.Y, titikBawahKanan.X, titikBawahKanan.Y);
		
		List<Vector2> trapesium = new List<Vector2>();
		trapesium.AddRange(garis1);
		trapesium.AddRange(garis2.GetRange(1, garis2.Count - 1));
		trapesium.AddRange(garis3.GetRange(1, garis3.Count - 1));
		trapesium.AddRange(garis4.GetRange(1, garis4.Count - 1));

		return trapesium;
	}

	private void CirclePlotPoints(int xCenter, int yCenter, int x, int y, List<Vector2> points)
	{
		points.Add(new Vector2(xCenter + x, yCenter + y));
		points.Add(new Vector2(xCenter - x, yCenter + y));
		points.Add(new Vector2(xCenter + x, yCenter - y));
		points.Add(new Vector2(xCenter - x, yCenter - y));
		points.Add(new Vector2(xCenter + y, yCenter + x));
		points.Add(new Vector2(xCenter - y, yCenter + x));
		points.Add(new Vector2(xCenter + y, yCenter - x));
		points.Add(new Vector2(xCenter - y, yCenter - x));
	}

public List<Vector2> GarisPola(Vector2 start, Vector2 end, string pola = "solid")
{
	List<Vector2> points = new List<Vector2>();

	if (_primitif == null)
	{
		GD.PrintErr("Node Primitif belum di-assign!");
		return points;
	}

	List<Vector2> garisDasar = _primitif.LineBresenham(start.X, start.Y, end.X, end.Y);

	if (pola == "dotted") // Garis titik-titik
	{
		for (int i = 0; i < garisDasar.Count; i += 2)
		{
			points.Add(garisDasar[i]);
		}
	}
	else if (pola == "dashed") // Garis putus-putus
	{
		for (int i = 0; i < garisDasar.Count; i += 5)
		{
			if (i + 2 < garisDasar.Count)
			{
				points.AddRange(garisDasar.GetRange(i, 2));
			}
		}
	}
	else if (pola == "dash-dot") // Garis titik-garis-titik
	{
		for (int i = 0; i < garisDasar.Count; i += 6)
		{
			if (i + 2 < garisDasar.Count)
			{
				points.AddRange(garisDasar.GetRange(i, 2)); // Garis pendek
			}
			if (i + 4 < garisDasar.Count)
			{
				points.Add(garisDasar[i + 3]); // Titik
			}
		}
	}
	else // Default solid
	{
		points.AddRange(garisDasar);
	}

	return points;
}


	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			GD.Print($"_primitif is null in _ExitTree(): {_primitif == null}");
			_primitif?.Dispose();
			_primitif = null;
			GD.Print($"_primitif is null in _ExitTree(): {_primitif == null}");
		}
	}
}
