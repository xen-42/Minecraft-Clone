extends ChunkGenerator

static func generate_surface(height, x, y, z):
	var type
	if y == height - 1:
		type = "Snow"
	elif y < height - 10 or y == 0:
		type = "Stone"
	elif y < height - 1:
		type = "Dirt"
	else:
		type = "Air"
	return type

static func generate_details(c, rng, ground_height):
	# Now generate trees
	var tree_width = 2
	for n_tree in range(0, rng.randi_range(2, 8)):
		var pos_x = rng.randi_range(tree_width, c.DIMENSION.x - tree_width - 1)
		var pos_z = rng.randi_range(tree_width, c.DIMENSION.z - tree_width - 1)

		# Place a log at this position up to some height
		var tree_height = rng.randi_range(5, 7)
		for i in range(0, tree_height):
			var b = c.BlockData.new()
			b.create("Log")
			var x = pos_x
			var z = pos_z
			var y = ground_height[x][z] + i
			c._set_block_data(x, y, z, b)
		# Place leaves
		for dy in range(0, 5):
			var leaf_width = tree_width
			if dy % 2 == 1:
				leaf_width -= 1
			if dy == 4:
				leaf_width = 0
			for dx in range(-leaf_width, leaf_width+1):
				for dz in range(-leaf_width, leaf_width+1):
					var lx = pos_x + dx
					var ly = ground_height[pos_x][pos_z] + tree_height + dy - 4
					var lz = pos_z + dz
					var l = c.BlockData.new()
					l.create("Pine_Leaf")
					c._set_block_data(lx, ly, lz, l, false)
	return
