extends Control

func _ready():
	pass


func _on_BtnBack_pressed():
	var back = get_tree().change_scene_to_file("res://Scenes/Welcome.tscn")
	if back != OK:
		print("Scene Tidak Ada")
