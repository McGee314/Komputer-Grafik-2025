extends Button

func _on_BtnBack_pressed():
	get_tree().change_scene_to_file("res://Scenes/Welcome.tscn")
	if get_tree().change_scene_to_file("res://Scenes/Welcome.tscn") != OK:
		print("Scene Tidak Ada")
