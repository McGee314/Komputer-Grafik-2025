using Godot;
using System;
using System.Collections.Generic;

public partial class karya3: Node2D
{
	private List<AnimatedFlower> _flowers = new List<AnimatedFlower>();
	private float _elapsedTime = 0f;
	private Random _random = new Random();
	
	// Animation configuration
	[Export]
	public int FlowerCount = 5;
	
	[Export]
	public Vector2 FieldSize = new Vector2(800, 600);
	
	[Export]
	public float MinFlowerSize = 20f;
	
	[Export]
	public float MaxFlowerSize = 50f;
	
	// Override methods
	public override void _Ready()
	{
		// Generate random flowers
		for (int i = 0; i < FlowerCount; i++)
		{
			float size = (float)_random.NextDouble() * (MaxFlowerSize - MinFlowerSize) + MinFlowerSize;
			Vector2 position = new Vector2(
				(float)_random.NextDouble() * FieldSize.X,
				(float)_random.NextDouble() * FieldSize.Y
			);
			int petalCount = _random.Next(0, 2) == 0 ? 4 : 8; // Either 4 or 8 petals
			
			// Create animation parameters for this flower
			float swaySpeed = (float)_random.NextDouble() * 2f + 0.5f;
			float swayAmount = (float)_random.NextDouble() * 10f + 5f;
			float growPulseSpeed = (float)_random.NextDouble() * 1.5f + 0.5f;
			float growPulseAmount = (float)_random.NextDouble() * 0.2f + 0.05f;
			float rotationSpeed = (float)_random.NextDouble() * 30f - 15f; // Could be negative for counterclockwise
			
			// Create and add the animated flower
			var flower = new AnimatedFlower(
				position, 
				size, 
				petalCount,
				swaySpeed,
				swayAmount,
				growPulseSpeed,
				growPulseAmount,
				rotationSpeed
			);
			
			_flowers.Add(flower);
		}
	}
	
	public override void _Process(double delta)
	{
		_elapsedTime += (float)delta;
		QueueRedraw(); // Tell Godot to redraw the scene
	}
	
	public override void _Draw()
	{
		// Draw each flower with its current animation state
		foreach (var flower in _flowers)
		{
			flower.Update(_elapsedTime);
			flower.Draw(this);
		}
	}
}

public class AnimatedFlower
{
	// Base flower properties
	private OptimizedBunga _flower;
	private Vector2 _basePosition;
	
	// Animation parameters
	private float _swaySpeed;
	private float _swayAmount;
	private float _growPulseSpeed;
	private float _growPulseAmount;
	private float _rotationSpeed; // Degrees per second
	
	// Current animation state
	private float _currentRotation = 0f;
	private float _currentScale = 1f;
	private Vector2 _currentOffset = Vector2.Zero;
	
	public AnimatedFlower(
		Vector2 position, 
		float size, 
		int petalCount,
		float swaySpeed,
		float swayAmount,
		float growPulseSpeed,
		float growPulseAmount,
		float rotationSpeed)
	{
		_basePosition = position;
		_flower = new OptimizedBunga(position, size, petalCount);
		
		// Set animation parameters
		_swaySpeed = swaySpeed;
		_swayAmount = swayAmount;
		_growPulseSpeed = growPulseSpeed;
		_growPulseAmount = growPulseAmount;
		_rotationSpeed = rotationSpeed;
	}
	
	public void Update(float time)
	{
		// Calculate swaying motion (like wind effect)
		float swayX = Mathf.Sin(time * _swaySpeed) * _swayAmount;
		float swayY = Mathf.Cos(time * _swaySpeed * 0.7f) * (_swayAmount * 0.5f);
		_currentOffset = new Vector2(swayX, swayY);
		
		// Calculate pulsing growth
		_currentScale = 1f + Mathf.Sin(time * _growPulseSpeed) * _growPulseAmount;
		
		// Calculate rotation
		_currentRotation = (time * _rotationSpeed) % 360f;
	}
	
	public void Draw(CanvasItem canvas)
	{
		// Create a transform that combines all our animations
		Transform2D transform = Transform2D.Identity
			.Scaled(new Vector2(_currentScale, _currentScale))
			.Rotated(Mathf.DegToRad(_currentRotation));
		
		// Update the flower's position with animation offset
		Vector2 animatedPosition = _basePosition + _currentOffset;
		_flower.Pusat = animatedPosition;
		
		// Draw the flower with our animation transform
		_flower.Gambar(canvas, transform);
	}
}
//public class OptimizedBunga2
//{
	//public Vector2 Pusat{ get; set;}
	//public float Ukuran{get; private set;}
	//public int Jumlah Kelopak {get; private set;}
	//
//}

// Optimized version of Bunga class
public class OptimizedBunga
{
	public Vector2 Pusat { get; set; }
	public float Ukuran { get; private set; }
	public int JumlahKelopak { get; private set; }
	
	// Pre-computed petal and center points
	private List<Vector2[]> _petalPoints = new List<Vector2[]>();
	private Vector2[] _centerPoints;
	
	// Colors
	private Color _petalColor = Colors.White;
	private Color _centerColor = Colors.Yellow;
	
	public OptimizedBunga(Vector2 pusat, float ukuran, int jumlahKelopak)
	{
		Pusat = pusat;
		Ukuran = ukuran;
		JumlahKelopak = jumlahKelopak;
		
		// Pre-compute petal shapes
		float[] sudutKelopak = JumlahKelopak == 8 
			? new float[] { 0, 22.5f, 45, 67.5f, 90, 112.5f, 135, 157.5f } 
			: new float[] { 0, 45, 90, 135 };
		
		// Pre-generate the petal shapes
		foreach (float sudut in sudutKelopak)
		{
			// Create a petal based on an ellipse
			Vector2[] petalPoints = GeneratePetalPoints(ukuran, ukuran/2, Mathf.DegToRad(sudut));
			_petalPoints.Add(petalPoints);
		}
		
		// Pre-generate the center circle points
		_centerPoints = GenerateCirclePoints((int)(ukuran / 2.5f));
	}
	
	private Vector2[] GeneratePetalPoints(float rx, float ry, float angle)
	{
		// Use a simplified ellipse with fewer points for better performance
		int numPoints = 20; // Reduced number of points
		Vector2[] points = new Vector2[numPoints];
		
		// Generate points for a simplified ellipse
		for (int i = 0; i < numPoints; i++)
		{
			float t = 2 * Mathf.Pi * i / numPoints;
			float x = rx * Mathf.Cos(t);
			float y = ry * Mathf.Sin(t);
			
			// Apply rotation
			float rotatedX = x * Mathf.Cos(angle) - y * Mathf.Sin(angle);
			float rotatedY = x * Mathf.Sin(angle) + y * Mathf.Cos(angle);
			
			points[i] = new Vector2(rotatedX, rotatedY);
		}
		
		return points;
	}
	
	private Vector2[] GenerateCirclePoints(int radius)
	{
		// Use fewer points for the center circle
		int numPoints = 16; // Reduced number of points
		Vector2[] points = new Vector2[numPoints];
		
		for (int i = 0; i < numPoints; i++)
		{
			float angle = 2 * Mathf.Pi * i / numPoints;
			float x = radius * Mathf.Cos(angle);
			float y = radius * Mathf.Sin(angle);
			points[i] = new Vector2(x, y);
		}
		
		return points;
	}
	
	public void Gambar(CanvasItem canvas, Transform2D transform = default)
	{
		if (transform == default)
			transform = Transform2D.Identity;
		
		canvas.DrawSetTransformMatrix(transform);
		
		// Draw petals more efficiently with larger points
		foreach (Vector2[] petalPoints in _petalPoints)
		{
			foreach (Vector2 p in petalPoints)
			{
				// Use larger points and fewer of them
				canvas.DrawCircle(p + Pusat, 2, _petalColor);
			}
		}
		
		// Draw center more efficiently with larger points
		foreach (Vector2 p in _centerPoints)
		{
			canvas.DrawCircle(p + Pusat, 2, _centerColor);
		}
	}
	
	// Advanced rendering option: draw as polygons instead of individual points
	public void GambarPolygon(CanvasItem canvas, Transform2D transform = default)
	{
		if (transform == default)
			transform = Transform2D.Identity;
		
		canvas.DrawSetTransformMatrix(transform);
		
		// Draw petals as polygons
		foreach (Vector2[] petalPoints in _petalPoints)
		{
			Vector2[] translatedPoints = new Vector2[petalPoints.Length];
			for (int i = 0; i < petalPoints.Length; i++)
			{
				translatedPoints[i] = petalPoints[i] + Pusat;
			}
			
			canvas.DrawColoredPolygon(translatedPoints, _petalColor);
		}
		
		// Draw center as a polygon
		Vector2[] translatedCenterPoints = new Vector2[_centerPoints.Length];
		for (int i = 0; i < _centerPoints.Length; i++)
		{
			translatedCenterPoints[i] = _centerPoints[i] + Pusat;
		}
		
		canvas.DrawColoredPolygon(translatedCenterPoints, _centerColor);
	}
}
