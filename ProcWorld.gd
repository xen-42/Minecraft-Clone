extends Spatial
var height_noise = OpenSimplexNoise.new()

onready var Chunk = load("res://Chunk.gd")

# Thread variables
var thread
var mutex
var semaphore
var kill_thread = false

var _new_chunk_pos = Vector2()
var _chunk_pos = null
var _loaded_chunks = {}
var _chunks_to_unload = []
var _last_chunk = Vector2()

var _kill_thread = false

const load_radius = 3
var current_load_radius = 0

func _ready():
	thread = Thread.new()
	mutex = Mutex.new()
	semaphore = Semaphore.new()
	
	thread.start(self, "_thread_gen")
	height_noise.period = 100

func lock():
	#print("Locking thread")
	mutex.lock()

func unlock():
	#print("Unlocking thread")
	mutex.unlock()

func _thread_gen(userdata):
	# Center map generation on the player
	while(not _kill_thread):
		# Check if player in new chunk
		var player_pos_updated = false
		lock()
		_kill_thread = kill_thread
		unlock()
		if not _kill_thread:
			lock()
			player_pos_updated = _new_chunk_pos != _chunk_pos
			# Make sure we aren't making a shallow copy
			_chunk_pos = Vector2(_new_chunk_pos.x, _new_chunk_pos.y)
			var current_chunk_pos = Vector2(_new_chunk_pos.x, _new_chunk_pos.y)
			unlock()
			
			if player_pos_updated:
				# If new chunk unload unneeded chunks
				for v in _loaded_chunks.keys():
					if abs(v.x - current_chunk_pos.x) > load_radius or abs(v.y - current_chunk_pos.y) > load_radius:
						_chunks_to_unload.append(Vector2(v.x, v.y))
				# Make sure player chunk is loaded
				_last_chunk = _load_chunk(current_chunk_pos.x, current_chunk_pos.y)
				current_load_radius = 1
			
			# Unload old chunks
			if _chunks_to_unload.size() > 0:
				var v = _chunks_to_unload[0]
				_unload_chunk(v.x, v.y)
				_chunks_to_unload.erase(v)
			else:
				# Load next chunk based on the position of the last one
				var delta_pos = _last_chunk - current_chunk_pos
				# Only have player chunk
				if delta_pos == Vector2():
					# Move down one
					_last_chunk = _load_chunk(_last_chunk.x, _last_chunk.y + 1)
				elif delta_pos.x < delta_pos.y:
					# Either go right or up
					# Prioritize going right
					if delta_pos.y == current_load_radius and -delta_pos.x != current_load_radius:
						# Go right
						_last_chunk = _load_chunk(_last_chunk.x - 1, _last_chunk.y)
					# Either moving in constant x or we just reached bottom right
					elif -delta_pos.x == current_load_radius or -delta_pos.x == delta_pos.y:
						# Go up
						_last_chunk = _load_chunk(_last_chunk.x, _last_chunk.y - 1)
					else:
						# We increment here idk why
						if current_load_radius < load_radius:
							current_load_radius += 1
				else:
					# Either go left or down
					# Prioritize going left
					if -delta_pos.y == current_load_radius and delta_pos.x != current_load_radius:
						# Go left
						_last_chunk = _load_chunk(_last_chunk.x + 1, _last_chunk.y)
					elif delta_pos.x == current_load_radius or delta_pos.x == -delta_pos.y:
						# Go down
						# Stop the last one where we'd go over the limit
						if delta_pos.y < load_radius:
							_last_chunk = _load_chunk(_last_chunk.x, _last_chunk.y + 1)
		else:
			return

func update_player_pos(new_pos):
	lock()
	_new_chunk_pos = new_pos
	unlock()

func change_block(cx, cz, bx, by, bz, t):
	lock()
	var c = _loaded_chunks[Vector2(cx, cz)]
	if c._block_data[bx][by][bz].type != t:
		print("Changed block at %d %d %d in chunk %d, %d" % [bx, by, bz, cx, cz])
		c._block_data[bx][by][bz].create(t)
		_update_chunk(cx, cz)
	unlock()

func _load_chunk(cx, cz):
	var c_pos = Vector2(cx, cz)
	if not _loaded_chunks.has(c_pos):
		var c = Chunk.new()
		c.generate(self, cx, cz)
		c.update()
		call_deferred("add_child", c)
		_loaded_chunks[c_pos] = c
	return c_pos

func _update_chunk(cx, cz):
	var c_pos = Vector2(cx, cz)
	if _loaded_chunks.has(c_pos):
		var c = _loaded_chunks[c_pos]
		c.update()
	return c_pos

func _unload_chunk(cx, cz):
	var c_pos = Vector2(cx, cz)
	if _loaded_chunks.has(c_pos):
		_loaded_chunks[c_pos].call_deferred("queue_free")
		_loaded_chunks.erase(c_pos)
		# Force it to just fucking chill after holy shit
		OS.delay_msec(50)

func kill_thread():
	print("Trying to lock")
	lock()
	print("Killed")
	kill_thread = true
	print("Trying to unlock")
	unlock()
	print("Unlocked")
