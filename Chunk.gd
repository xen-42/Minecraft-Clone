extends StaticBody

const DIMENSION = Vector3(20,50,20)

var mat = preload("res://assets/TextureAtlasMaterial.tres")
var rng = RandomNumberGenerator.new()
# Make this load from a file
const texture_atlas_size = Vector2(8, 4)

var generator = load("res://chunk_generators/ForestGenerator.gd")

const v = [
	Vector3(0, 0, 0), #0
	Vector3(1, 0, 0), #1
	Vector3(0, 1, 0), #2
	Vector3(1, 1, 0), #3
	Vector3(0, 0, 1), #4
	Vector3(1, 0, 1), #5
	Vector3(0, 1, 1), #6
	Vector3(1, 1, 1)  #7
]

const face_tri_a = [
	Vector2(0, 0),
	Vector2(0, 1),
	Vector2(1, 1)
]

const face_tri_b = [
	Vector2(0, 0),
	Vector2(1, 1),
	Vector2(1, 0)
]

# Indices from the vertices list
const TOP = [2, 3, 7, 6]
const BOTTOM = [0, 4, 5, 1]
const LEFT = [6, 4, 0, 2]
const RIGHT = [3, 1, 5, 7]
const FRONT = [7, 5, 4, 6]
const BACK = [2, 0, 1, 3]
const CROSS_1 = [3, 1, 4, 6]
const CROSS_2 = [7, 5, 0, 2]
const CROSS_3 = [6, 4, 1, 3]
const CROSS_4 = [2, 0, 5, 7]

var world = null
var chunk_coord = Vector2()

var blocks = []
var _block_data = []

var st = SurfaceTool.new()

func _ready():
	mat.albedo_texture.set_flags(2)

func _set_block_data(x, y, z, b, overwrite = true):
	if x >= 0 and x < DIMENSION.x and y >= 0 and y < DIMENSION.y and z >= 0 and z < DIMENSION.z:
		if overwrite or _block_data[x][y][z].type == "Air":
			_block_data[x][y][z] = b

func generate(w, cx, cz):
	world = w
	
	rng.set_seed(cx*1000+cz) # Arbitrary
	
	chunk_coord = Vector2(cx, cz)
	self.translation = Vector3(cx * DIMENSION.x, 0, cz * DIMENSION.z)
	
	# Generate block data
	var ground_height = []
	
	_block_data.resize(DIMENSION.x)
	ground_height.resize(DIMENSION.x)
	for x in DIMENSION.x:
		_block_data[x] = []
		_block_data[x].resize(DIMENSION.y)
		ground_height[x] = []
		ground_height[x].resize(DIMENSION.z)
		for y in DIMENSION.y:
			_block_data[x][y] = []
			_block_data[x][y].resize(DIMENSION.z)
			for z in DIMENSION.z:
				var b = BlockData.new()
				var h_noise = (world.height_noise.get_noise_2d(x + cx * DIMENSION.x, z + cz * DIMENSION.z) + 1) / 2.0
				ground_height[x][z] = int(h_noise * (DIMENSION.y - 1) + 1)
				b.create(generator.generate_surface(ground_height[x][z], x, y, z))
				_set_block_data(x, y, z, b)
	
	generator.generate_details(self, rng, ground_height)

func update():
	var mesh = Mesh.new()
	var mesh_instance = MeshInstance.new()
	
	st.begin(Mesh.PRIMITIVE_TRIANGLES)
	blocks.resize(DIMENSION.x)
	for x in DIMENSION.x:
		blocks[x] = []
		blocks[x].resize(DIMENSION.y)
		for y in DIMENSION.y:
			blocks[x][y] = []
			blocks[x][y].resize(DIMENSION.z)
			for z in DIMENSION.z:
				# Now we create blocks
				if not _block_data[x][y][z].type == "Air":
					# Check if its visible bc of its neighbours
					var check = check_transparent_neighbours(x, y, z)
					if check.has(true):
						_create_block(check, x, y, z)
	st.generate_normals(false)
	st.set_material(mat)
	st.commit(mesh)
	mesh_instance.set_mesh(mesh)
	
	# If it already has a mesh instance child, remove it
	for child in get_children():
		if child is MeshInstance:
			child.queue_free()
	add_child(mesh_instance)
	mesh_instance.create_trimesh_collision()

func _create_block(check, x, y, z):
	var block = _block_data[x][y][z].type
	if block_types[block]["Tags"].has(Tags.Flat):
		create_face(CROSS_1, x, y, z, block_types[block][Side.only])
		create_face(CROSS_2, x, y, z, block_types[block][Side.only])
		create_face(CROSS_3, x, y, z, block_types[block][Side.only])
		create_face(CROSS_4, x, y, z, block_types[block][Side.only])
	else:
		if check[0]:
			create_face(TOP, x, y, z, block_types[block][Side.top])
		if check[1]:
			create_face(BOTTOM, x, y, z, block_types[block][Side.bottom])
		if check[2]:
			create_face(LEFT, x, y, z, block_types[block][Side.left])
		if check[3]:
			create_face(RIGHT, x, y, z, block_types[block][Side.right])
		if check[4]:
			create_face(BACK, x, y, z, block_types[block][Side.back])
		if check[5]:
			create_face(FRONT, x, y, z, block_types[block][Side.front])

func create_face(i, x, y, z, texture_atlas_offset):
	# Two triangles
	var offset = Vector3(x, y, z)
	var a = v[i[0]] + offset
	var b = v[i[1]] + offset
	var c = v[i[2]] + offset
	var d = v[i[3]] + offset
	
	var uv_offset = Vector2(
		texture_atlas_offset.x / texture_atlas_size.x,
		texture_atlas_offset.y / texture_atlas_size.y
	)
	
	var uv_a = Vector2(0, 0) + uv_offset
	var uv_b = Vector2(0, 1.0/texture_atlas_size.y) + uv_offset
	var uv_c = Vector2(1.0/texture_atlas_size.x, 1.0/texture_atlas_size.y) + uv_offset
	var uv_d = Vector2(1.0/texture_atlas_size.x, 0) + uv_offset
	
	# Add UVs and tris
	st.add_triangle_fan(([a, b, c]), ([uv_a, uv_b, uv_c]))
	st.add_triangle_fan(([a, c, d]), ([uv_a, uv_c, uv_d]))

func check_transparent_neighbours(x, y, z):
	var has_top = is_block_transparent(x, y + 1, z)
	var has_bottom = is_block_transparent(x, y - 1, z)
	var has_left = is_block_transparent(x - 1, y, z)
	var has_right = is_block_transparent(x + 1, y, z)
	var has_front = is_block_transparent(x, y, z - 1)
	var has_back = is_block_transparent(x, y, z + 1)
	
	return [has_top, has_bottom, has_left, has_right, has_front, has_back]

func is_block_transparent(x, y, z):
	if x < 0 or x >= DIMENSION.x or z < 0 or z >= DIMENSION.z or y < 0 or y >= DIMENSION.y:
		# For out of bounds lets still show the face
		return true
	else:
		return _block_data[x][y][z].transparent

enum Side {
	top,
	bottom,
	left,
	right,
	front,
	back,
	only
}

enum Tags {
	Transparent,
	No_Collision,
	Flat
}

const block_types = {
	"Dirt":{
		Side.top:Vector2(1,0),Side.bottom:Vector2(1,0),Side.left:Vector2(1,0),
		Side.right:Vector2(1,0),Side.front:Vector2(1,0),Side.back:Vector2(1,0),
		"Tags":[]
		},
	"Grass":{
		Side.top:Vector2(1,1),Side.bottom:Vector2(1,0),Side.left:Vector2(0,1),
		Side.right:Vector2(0,1),Side.front:Vector2(0,1),Side.back:Vector2(0,1),
		"Tags":[]
		},
	"Stone":{
		Side.top:Vector2(0,0),Side.bottom:Vector2(0,0),Side.left:Vector2(0,0),
		Side.right:Vector2(0,0),Side.front:Vector2(0,0),Side.back:Vector2(0,0),
		"Tags":[]
		},
	"Log":{
		Side.top:Vector2(3,0),Side.bottom:Vector2(3,0),Side.left:Vector2(2,0),
		Side.right:Vector2(2,0),Side.front:Vector2(2,0),Side.back:Vector2(2,0),
		"Tags":[]
		},
	"Leaf":{
		Side.top:Vector2(2,1),Side.bottom:Vector2(2,1),Side.left:Vector2(2,1),
		Side.right:Vector2(2,1),Side.front:Vector2(2,1),Side.back:Vector2(2,1),
		"Tags":[Tags.Transparent]
		},
	"Pine_Leaf":{
		Side.top:Vector2(3,1),Side.bottom:Vector2(3,1),Side.left:Vector2(3,1),
		Side.right:Vector2(3,1),Side.front:Vector2(3,1),Side.back:Vector2(3,1),
		"Tags":[Tags.Transparent]
		},
	"Snow":{
		Side.top:Vector2(1,2),Side.bottom:Vector2(1,0),Side.left:Vector2(0,2),
		Side.right:Vector2(0,2),Side.front:Vector2(0,2),Side.back:Vector2(0,2),
		"Tags":[]
		},
	"Sand":{
		Side.top:Vector2(2,2),Side.bottom:Vector2(2,2),Side.left:Vector2(2,2),
		Side.right:Vector2(2,2),Side.front:Vector2(2,2),Side.back:Vector2(2,2),
		"Tags":[]
		},
	"Cactus":{
		Side.top:Vector2(3,2),Side.bottom:Vector2(3,2),Side.left:Vector2(3,2),
		Side.right:Vector2(3,2),Side.front:Vector2(3,2),Side.back:Vector2(3,2),
		"Tags":[]
		},
	"Tall_Grass":{
		Side.only:Vector2(0,3),
		"Tags":[Tags.Flat, Tags.Transparent, Tags.No_Collision]
		},
	"Flower":{
		Side.only:Vector2(2,3),
		"Tags":[Tags.Flat, Tags.Transparent, Tags.No_Collision]
		},
	"Shrub":{
		Side.only:Vector2(1,3),
		"Tags":[Tags.Flat, Tags.Transparent, Tags.No_Collision]
		},
	"Wood":{
		Side.top:Vector2(4,0),Side.bottom:Vector2(4,0),Side.left:Vector2(4,0),
		Side.right:Vector2(4,0),Side.front:Vector2(4,0),Side.back:Vector2(4,0),
		"Tags":[]
		},
	"Brick":{
		Side.top:Vector2(4,1),Side.bottom:Vector2(4,1),Side.left:Vector2(4,1),
		Side.right:Vector2(4,1),Side.front:Vector2(4,1),Side.back:Vector2(4,1),
		"Tags":[]
		},
	"Air":{"Tags":[Tags.Transparent]}
}

class BlockData:
	var transparent = false
	var type =  "Dirt"
	func create(t):
		type = t
		transparent = block_types[t]["Tags"].has(Tags.Transparent)
