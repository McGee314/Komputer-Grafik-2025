extends Node3D


# Called when the node enters the scene tree for the first time.
func _ready():
	await get_tree().create_timer(3).timeout
	get_viewport().get_texture().get_image().save_png("/Users/samudera/Bagja/Magang/screenshot.png")
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
