namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

public partial class karya3 : Node2D
{
	private Bunga bunga;
	private Tween tween;

	public override void _Ready()
	{
		// Membuat bunga di tengah layar
		Vector2 windowSize = GetViewportRect().Size;
		bunga = new Bunga(windowSize / 2, 100, 8);

		// Membuat Tween untuk animasi
		tween = CreateTween();
		tween.SetLoops(); // Mengulang animasi secara terus-menerus

		// Animasi rotasi bunga
		tween.TweenProperty(this, "rotation", Mathf.Pi * 2, 5.0f).AsRelative();

		// Animasi scaling bunga
		tween.TweenProperty(this, "scale", new Vector2(1.5f, 1.5f), 2.5f);
		tween.TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), 2.5f);
	}

	public override void _Draw()
	{
		bunga.Gambar(this);
	}
}
