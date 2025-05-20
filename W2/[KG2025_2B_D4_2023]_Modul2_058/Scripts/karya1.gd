extends Node2D

@onready var Primitif = primitif.new()

var screen_width 
var screen_height 

const offset_left = 50
const offset_top  = 50
var offset_right 
var offset_bottom  
func _ready():
	screen_width = get_window().size.x
	screen_height = get_window().size.y

	offset_right = screen_width - offset_left
	offset_bottom = screen_height - offset_top
	queue_redraw()
	
func _draw():
	var color = Color("#32CD30")
	var res = PackedVector2Array()
	
	res.append_array(Primitif.lineDDA(offset_left, offset_top, offset_right, offset_top))
	res.append_array(Primitif.lineDDA(offset_left, offset_bottom, offset_right, offset_bottom))  # Kiri Bawah Kanan Bawah
	res.append_array(Primitif.lineDDA(offset_left, offset_top, offset_left, offset_bottom))      # Kiri Atas Kiri Bawah
	res.append_array(Primitif.lineDDA(offset_right, offset_top, offset_right, offset_bottom))   # Kanan Atas Kanan Bawah
	put_pixel_all(res,color)
		

func put_pixel(x, y, color=Color("007fff")):
	draw_primitive(PackedVector2Array([Vector2(x, y)]),
		PackedColorArray([color]),
		PackedVector2Array())

func put_pixel_all(dot: PackedVector2Array, color):
	for i in dot.size():
		put_pixel(dot[i].x, dot[i].y, color)
