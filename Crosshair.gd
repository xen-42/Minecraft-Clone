extends Sprite

var current_viewport_size = Vector2()
func _ready():
	var x = get_viewport().get_visible_rect().size.x / 2.0
	var y = get_viewport().get_visible_rect().size.y / 2.0
	current_viewport_size = Vector2(x, y)
	
	self.position = Vector2(x, y)
	get_tree().get_root().connect("size_changed", self, "_on_size_changed")

func _on_size_changed():
	var x = get_viewport().get_visible_rect().size.x / 2.0
	var y = get_viewport().get_visible_rect().size.y / 2.0
	
	# Change scale by size ratio
	var size_ratio = Vector2(x / current_viewport_size.x, y / current_viewport_size.y)
	current_viewport_size = Vector2(x, y)
	
	self.scale = Vector2(self.scale.x * size_ratio.x, self.scale.y * size_ratio.y)
	self.position = Vector2(x, y)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
