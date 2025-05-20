extends Control

func _ready():
	pass

func _on_BtnKarya1_pressed():
	var result = get_tree().change_scene_to_file("res://Scenes/Karya1.tscn")
	if result != OK:
		print("Scene Tidak Ada")
	
	
func _on_BtnKarya2_pressed():
	var result = get_tree().change_scene_to_file("res://Scenes/Karya2.tscn")
	if result != OK:
		print("Scene Tidak Ada")


func _on_BtnAbout_pressed():
	var result = get_tree().change_scene_to_file("res://Scenes/About.tscn")
	if result != OK:
		print("Scene Tidak Ada")


func _on_BtnGuide_pressed():
	var result = get_tree().change_scene_to_file("res://Scenes/Guide.tscn")
	if result != OK:
		print("Scene Tidak Ada")


func _on_BtnExit_pressed():
	get_tree().quit()
