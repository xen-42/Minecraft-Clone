extends KinematicBody

onready var camera = $CameraBase/Camera
onready var camera_base = $CameraBase
onready var raycast = $CameraBase/Camera/RayCast
onready var cube_outline = $CubeOutline
onready var info_label = $CameraBase/Camera/Label

var Chunk = load("res://Chunk.gd")
var selected_block = Chunk.block_types.keys()[0]
var selected_block_index = 0

var camera_x_rotation = 0

const mouse_sensitivity = 0.3
const SPEED = 5
var velocity = Vector3.ZERO
const gravity = 9.8
var jump_vel = 5

var paused = false

signal place_block(pos, norm, type)
signal destroy_block(pos, norm)
signal highlight_block(pos, norm)
signal unhighlight_block()

# Called when the node enters the scene tree for the first time.
func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event):
	# Mouse movement
	if not paused:
		if event is InputEventMouseMotion:
			self.rotate_y(deg2rad(-event.relative.x * mouse_sensitivity))
			
			var x_delta = event.relative.y * mouse_sensitivity
			if camera_x_rotation + x_delta > -90 and camera_x_rotation + x_delta < 90:
				camera.rotate_x(deg2rad(-x_delta))
				camera_x_rotation += x_delta

func _physics_process(delta):	
	var cx = floor(self.translation.x / Chunk.DIMENSION.x)
	var cz = floor(self.translation.z / Chunk.DIMENSION.z)
	var px = self.translation.x - cx * Chunk.DIMENSION.x
	var py = self.translation.y
	var pz = self.translation.z - cz * Chunk.DIMENSION.z
	info_label.text = "Selected block %s, Chunk (%d, %d) pos (%d, %d, %d)" % [selected_block, cx, cz, px, py, pz]
	if not paused:
		# Check the raycast
		if raycast.is_colliding():
			var pos = raycast.get_collision_point()
			var norm = raycast.get_collision_normal()
			emit_signal("highlight_block", pos, norm)
			if Input.is_action_just_pressed("click"):
				print("Click")
				emit_signal("destroy_block", pos, norm)
			elif Input.is_action_just_pressed("right_click"):
				emit_signal("place_block", pos, norm, selected_block)
		else:
			emit_signal("unhighlight_block")
		
		# Scroll to change block
		if Input.is_action_just_released("scroll_up"):
			selected_block_index -= 1
			if selected_block_index < 0:
				selected_block_index += Chunk.block_types.keys().size()
		elif Input.is_action_just_released("scroll_down"):
			selected_block_index += 1
			if selected_block_index >= Chunk.block_types.keys().size():
				selected_block_index -= Chunk.block_types.keys().size()
		selected_block = Chunk.block_types.keys()[selected_block_index]
		
		if Input.is_action_just_pressed("jump") and is_on_floor():
			velocity.y = jump_vel
		else:
			var camera_base_basis = self.get_global_transform().basis
			
			var direction = Vector3()
			
			if Input.is_action_pressed("forward"):
				direction -= camera_base_basis.z #forward is negative in Godot
			if Input.is_action_pressed("backward"):
				direction += camera_base_basis.z
			
			# Strafe
			if Input.is_action_pressed("left"):
				direction -= camera_base_basis.x
			if Input.is_action_pressed("right"):
				direction += camera_base_basis.x
			
			# Process inputs (only in the xz plane
			velocity.x = direction.x * SPEED
			velocity.z = direction.z * SPEED
		velocity.y -= gravity * delta
		velocity = move_and_slide(velocity, Vector3.UP)

func _process(delta):
	if Input.is_action_just_pressed("pause"):
		toggle_pause()

func toggle_pause():
	paused = !paused
	if paused:
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	else:
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
