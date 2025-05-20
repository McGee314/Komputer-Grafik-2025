using Godot;
using System;
using System.Collections.Generic;

namespace Godot;

[GlobalClass]
public partial class OndelOndel : Node2D
{
	private primitif _primitif = new primitif();
	private bentukdasar _bentukdasar = new bentukdasar();

	public List<Vector2> CreateOndelOndel(Vector2 position, float scale = 1.0f)
	{
		List<Vector2> ondelOndel = new List<Vector2>();

		// Head (Circle)
		float headRadius = 50 * scale;
		List<Vector2> head = _bentukdasar.Lingkaran((int)position.X, (int)position.Y, (int)headRadius);
		ondelOndel.AddRange(head);

		// Crown (Triangle)
		float crownWidth = 60 * scale;
		float crownHeight = 30 * scale;
		List<Vector2> crown = _bentukdasar.SegitigaSamaKaki(
			position.X - crownWidth / 2, 
			position.Y - headRadius - crownHeight, 
			crownWidth, 
			crownHeight
		);
		ondelOndel.AddRange(crown);

		// Body (Rectangle)
		float bodyWidth = 80 * scale;
		float bodyHeight = 100 * scale;
		List<Vector2> body = _bentukdasar.PersegiPanjang(
			position.X - bodyWidth / 2, 
			position.Y + headRadius, 
			bodyWidth, 
			bodyHeight
		);
		ondelOndel.AddRange(body);

		// Arms (Trapezoids)
		float armWidth = 20 * scale;
		float armHeight = 60 * scale;
		List<Vector2> leftArm = _bentukdasar.TrapesiumSamaKaki(
			position.X - bodyWidth / 2 - armWidth, 
			position.Y + headRadius + bodyHeight / 3, 
			armWidth, 
			armWidth / 2, 
			armHeight
		);
		List<Vector2> rightArm = _bentukdasar.TrapesiumSamaKaki(
			position.X + bodyWidth / 2, 
			position.Y + headRadius + bodyHeight / 3, 
			armWidth, 
			armWidth / 2, 
			armHeight
		);
		ondelOndel.AddRange(leftArm);
		ondelOndel.AddRange(rightArm);

		// Decorative elements (Ellipses)
		float decorRadius1 = 10 * scale;
		float decorRadius2 = 15 * scale;
		List<Vector2> decor1 = _bentukdasar.Ellips(
			(int)(position.X - bodyWidth / 4), 
			(int)(position.Y + headRadius + bodyHeight / 2), 
			decorRadius1, 
			decorRadius2
		);
		List<Vector2> decor2 = _bentukdasar.Ellips(
			(int)(position.X + bodyWidth / 4), 
			(int)(position.Y + headRadius + bodyHeight / 2), 
			decorRadius1, 
			decorRadius2
		);
		ondelOndel.AddRange(decor1);
		ondelOndel.AddRange(decor2);

		return ondelOndel;
	}

	// You can add methods to draw or manipulate the Ondel-ondel
	public override void _Draw()
	{
		// Example drawing method
		List<Vector2> ondelOndelPoints = CreateOndelOndel(new Vector2(300, 300));
		foreach (Vector2 point in ondelOndelPoints)
		{
			DrawPixel(point, Colors.Red);
		}
	}

	private void DrawPixel(Vector2 position, Color color)
	{
		DrawRect(new Rect2(position, new Vector2(1, 1)), color);
	}
}
