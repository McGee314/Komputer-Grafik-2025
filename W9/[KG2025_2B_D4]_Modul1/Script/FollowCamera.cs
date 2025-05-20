using Godot;
using System;

public partial class FollowCamera : Camera3D
{
	// --- Exported Variables (Set in the Inspector) ---

	// We need a way to tell the camera *who* to follow.
	// NodePath allows us to assign the target node easily in the editor.
	[Export]
	public NodePath TargetNodePath { get; set; }

	// Define how far behind and above the target the camera should be
	[Export]
	public Vector3 Offset = new Vector3(0, 3, 5); // X=0 (center), Y=3 (above), Z=5 (behind)

	// Controls how fast the camera catches up. Higher = faster/stiffer.
	[Export]
	public float SmoothSpeed = 5.0f;
	
	// --- Zoom Control Variables ---
	private Vector3 _baseOffset; // Store the initial offset for scaling
	private float _zoomFactor = 1.0f; // Default zoom level
	private const float _minZoom = 0.5f; // Closest zoom (0.5x distance)
	private const float _maxZoom = 2.5f; // Furthest zoom (2.5x distance)
	private const float _zoomStep = 0.1f; // How much to zoom per scroll
	
	// --- Orbit Control Variables ---
	private bool _isOrbiting = false; // Flag to track if we're currently orbiting
	private float _orbitSensitivity = 0.005f; // How fast the camera orbits with mouse movement

	// --- Private Variables ---

	private Node3D _target = null; // Internal variable to hold the actual Player node

	// --- Godot Lifecycle Methods ---

	// _Ready is called once when the node enters the scene. Good place for setup.
	public override void _Ready()
	{
		_baseOffset = Offset; // Store initial offset for zoom calculations
		
		if (TargetNodePath != null)
		{
			// Get the node specified by the path. We expect it to be a Node3D (like our Player).
			_target = GetNode<Node3D>(TargetNodePath);

			// Always good to check if we actually found the node
			if (_target == null)
			{
				GD.PrintErr($"FollowCamera Error: Target node not found at path: {TargetNodePath}");
			}
		}
		else
		{
			GD.PrintErr("FollowCamera Error: TargetNodePath not set in the Inspector!");
		}
	}
	
	// Handle mouse wheel input and mouse button input
	public override void _Input(InputEvent @event)
	{
		// Handle trackpad two-finger scroll and mouse wheel for zoom
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			// Handle zoom with mouse wheel or trackpad scroll gesture
			if ((mouseButtonEvent.ButtonIndex == MouseButton.WheelUp || 
				(mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.IsDoubleClick())) 
				&& mouseButtonEvent.Pressed)
			{
				// Zoom in (decrease distance)
				_zoomFactor = Mathf.Max(_minZoom, _zoomFactor - _zoomStep);
				UpdateZoom();
			}
			else if ((mouseButtonEvent.ButtonIndex == MouseButton.WheelDown || 
				(mouseButtonEvent.ButtonIndex == MouseButton.Right && mouseButtonEvent.IsDoubleClick())) 
				&& mouseButtonEvent.Pressed)
			{
				// Zoom out (increase distance)
				_zoomFactor = Mathf.Min(_maxZoom, _zoomFactor + _zoomStep);
				UpdateZoom();
			}
			// Handle orbit with right mouse button instead of middle (more trackpad friendly)
			else if (mouseButtonEvent.ButtonIndex == MouseButton.Right)
			{
				_isOrbiting = mouseButtonEvent.Pressed;
			}
		}
		// Handle trackpad pinch-to-zoom gestures
		else if (@event is InputEventMagnifyGesture magnifyEvent)
		{
			// Magnify factor > 1 means zoom in, < 1 means zoom out
			if (magnifyEvent.Factor > 1.0)
			{
				// Zoom in (decrease distance)
				_zoomFactor = Mathf.Max(_minZoom, _zoomFactor - _zoomStep);
				UpdateZoom();
			}
			else if (magnifyEvent.Factor < 1.0)
			{
				// Zoom out (increase distance)
				_zoomFactor = Mathf.Min(_maxZoom, _zoomFactor + _zoomStep);
				UpdateZoom();
			}
		}
		// Handle trackpad two-finger vertical pan gesture as an alternative zoom method
		else if (@event is InputEventPanGesture panEvent)
		{
			// Vertical pan (Y-axis) is used for zoom
			if (panEvent.Delta.Y < 0)
			{
				// Pan up = zoom in
				_zoomFactor = Mathf.Max(_minZoom, _zoomFactor - _zoomStep);
				UpdateZoom();
			}
			else if (panEvent.Delta.Y > 0)
			{
				// Pan down = zoom out
				_zoomFactor = Mathf.Min(_maxZoom, _zoomFactor + _zoomStep);
				UpdateZoom();
			}
		}
		// Handle motion events for orbit
		else if (@event is InputEventMouseMotion mouseMotionEvent && _isOrbiting)
		{
			// Same orbit code as before
			float angleDelta = -mouseMotionEvent.Relative.X * _orbitSensitivity;
			
			Vector3 horizontalOffset = new Vector3(Offset.X, 0, Offset.Z);
			horizontalOffset = horizontalOffset.Rotated(Vector3.Up, angleDelta);
			
			Offset = new Vector3(horizontalOffset.X, Offset.Y, horizontalOffset.Z);
			_baseOffset = new Vector3(horizontalOffset.X, _baseOffset.Y, horizontalOffset.Z);
		}
		
		// Add keyboard controls as an alternative
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.Equal || keyEvent.Keycode == Key.Plus)
			{
				// Zoom in with + key
				_zoomFactor = Mathf.Max(_minZoom, _zoomFactor - _zoomStep);
				UpdateZoom();
			}
			else if (keyEvent.Keycode == Key.Minus)
			{
				// Zoom out with - key
				_zoomFactor = Mathf.Min(_maxZoom, _zoomFactor + _zoomStep);
				UpdateZoom();
			}
		}
	}
	
	// Update the camera offset based on zoom factor
	private void UpdateZoom()
	{
		// Apply zoom factor to the base offset
		Offset = _baseOffset * _zoomFactor;
	}

	// _Process is called every frame. Good for camera updates as it's not directly physics-dependent.
	public override void _Process(double delta)
	{
		// Only try to follow if we successfully found the target in _Ready
		if (_target != null)
		{
			// --- Camera Movement Logic ---

			// 1. Calculate where the camera *should ideally* be (target position + offset)
			Vector3 desiredPosition = _target.GlobalPosition + Offset;

			// 2. Smoothly interpolate the camera's current position towards the desired position
			// Lerp(from, to, weight): 'weight' determines how much closer we get each frame.
			// Using SmoothSpeed * delta makes the smoothing consistent regardless of frame rate.
			GlobalPosition = GlobalPosition.Lerp(desiredPosition, SmoothSpeed * (float)delta);

			// 3. Make the camera always point towards the target's position
			// We often look slightly above the target's origin (feet) for a better view.
			// Vector3.Up is a shorthand for Vector3(0, 1, 0).
			LookAt(_target.GlobalPosition + Vector3.Up * 1.0f); // Look 1 meter above target origin
		}
	}
}
