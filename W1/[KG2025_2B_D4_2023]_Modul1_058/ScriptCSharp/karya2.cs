using Godot;
using System.Collections.Generic;

public partial class DrawArc : Node2D
{
	private void DrawCircleArc(Vector2 center, float radius, float angleFrom, float angleTo, Color color)
	{
		int nbPoints = 32;
		List<Vector2> pointsArc = new List<Vector2>();

		for (int i = 0; i <= nbPoints; i++)
		{
			float anglePoint = Mathf.DegToRad(angleFrom + i * (angleTo - angleFrom) / nbPoints - 90);
			pointsArc.Add(center + new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius);
		}

		for (int i = 0; i < nbPoints; i++)
		{
			DrawLine(pointsArc[i], pointsArc[i + 1], color);
		}
	}

	public override void _Draw()
	{
		Vector2 center = new Vector2(200, 200);
		float radius = 80;
		float angleFrom = 75;
		float angleTo = 195;
		Color color = new Color(1.0f, 0.0f, 0.0f);

		DrawCircleArc(center, radius, angleFrom, angleTo, color);
	}
}
