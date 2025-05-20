namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

public partial class primitif: RefCounted
{
	public List<Vector2> LineDDA(float xa, float ya, float xb, float yb)
	{
		float dx = xb - xa;
		float dy = yb - ya;
		float steps;
		float xIncrement;
		float yIncrement;
		float x = xa;
		float y = ya;

		List<Vector2> res = new List<Vector2>();

		if (Mathf.Abs(dx) > Mathf.Abs(dy))
		{
			steps = Mathf.Abs(dx);
		}
		else
		{
			steps = Mathf.Abs(dy);
		}

		xIncrement = dx / steps;
		yIncrement = dy / steps;

		res.Add(new Vector2(Mathf.Round(x), Mathf.Round(y)));

		for (int k = 0; k < steps; k++)
		{
			x += xIncrement;
			y += yIncrement;
			res.Add(new Vector2(Mathf.Round(x), Mathf.Round(y))); 
		}

		return res;
	}

	public List<Vector2> LineBresenham(float xa, float ya, float xb, float yb)
	{
		List<Vector2> res = new List<Vector2>();
		int x1 = (int)xa;
		int y1 = (int)ya;
		int x2 = (int)xb;
		int y2 = (int)yb;

		int dx = Math.Abs(x2 - x1);
		int dy = Math.Abs(y2 - y1);
		int sx = (x1 < x2) ? 1 : -1;
		int sy = (y1 < y2) ? 1 : -1;
		int err = dx - dy;

		while (true)
		{
			res.Add(new Vector2(x1, y1));
			if (x1 == x2 && y1 == y2) break;
			int e2 = 2 * err;
			if (e2 > -dy) { err -= dy; x1 += sx; }
			if (e2 < dx) { err += dx; y1 += sy; }
		}
		return res;
	}
}
