extends Node
class_name ChunkGenerator

const Chunk = preload("res://Chunk.gd")

static func generate_surface(height, x, y, z):
	if y == 0:
		return "Stone"
	else:
		return "Air"

static func generate_details(c, rng, ground_height):
	return
