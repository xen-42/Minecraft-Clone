using Godot;
using System;
using System.Threading;
using System.Collections.Generic;
using Thread = System.Threading.Thread;

public class ProcWorld : Spatial
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";


	public OpenSimplexNoise height_noise = new OpenSimplexNoise();

	Thread terrain_thread;
	object chunk_mutex = new object();
	public object mutex = new object();

	Vector2 _new_chunk_pos = new Vector2();
	Vector2 _chunk_pos = new Vector2();
	Vector2 current_chunk_pos = new Vector2();
	Dictionary<Vector2, Chunk_cs> _loaded_chunks = new Dictionary<Vector2, Chunk_cs>();
	Vector2 _last_chunk = new Vector2();
	Vector2[] LoadedChunks_list = new Vector2[0];

	readonly int load_radius = 5;
	int current_load_radius;

	bool bKillThread = false;
	public override void _Ready()
	{
		height_noise.Period = 100;
		terrain_thread = new Thread(_thread_gen);
		terrain_thread.Start();
	}

	void _thread_gen()
	{
		GD.Print("Thread Running");
		while(!bKillThread)
		{
			var player_pos_updated = false; 
			player_pos_updated =_new_chunk_pos != _chunk_pos;

			_chunk_pos = new Vector2(_new_chunk_pos.x, _new_chunk_pos.y);

			var current_chunk_pos = new Vector2(_new_chunk_pos.x, _new_chunk_pos.y);

			if (player_pos_updated)
			{
				enforce_render_distance(current_chunk_pos);
				_last_chunk = _load_chunk((int)current_chunk_pos.x, (int)current_chunk_pos.y);
				current_load_radius = 1;
			}
			else
			{
				Vector2 delta_pos = _last_chunk - current_chunk_pos;
				if (delta_pos == new Vector2())
				{
					_last_chunk = _load_chunk((int)_last_chunk.x, (int)_last_chunk.y + 1); 
				}
				else if(delta_pos.x < delta_pos.y)
				{
					if (delta_pos.y == current_load_radius && -delta_pos.x != current_load_radius)
					{
						_last_chunk = _load_chunk((int)(_last_chunk.x - 1), (int)_last_chunk.y);
					}
					else if(-delta_pos.x == current_load_radius || -delta_pos.x == delta_pos.y)
					{
						_last_chunk = _load_chunk((int)_last_chunk.x, (int)_last_chunk.y - 1);
					}
					else
					{
						if (current_load_radius < load_radius)
						{
							current_load_radius+=1;
						}
					}

				}
				else
				{
					if (-delta_pos.y == current_load_radius && delta_pos.x != current_load_radius)
					{
						_last_chunk = _load_chunk((int)_last_chunk.x + 1, (int)_last_chunk.y);
					}
					else if(delta_pos.x == current_load_radius || delta_pos.x == -delta_pos.y)
					{
						if (delta_pos.y < load_radius)
						{
							_last_chunk = _load_chunk((int)_last_chunk.x, (int)_last_chunk.y + 1);
						}
					}
				}
			}
		}
	}

	Vector2 _load_chunk(int cx, int cz)
	{
		Vector2 cpos = new Vector2(cx, cz);
		if(!_loaded_chunks.ContainsKey(cpos))
		{
			Chunk_cs c = new Chunk_cs();
			c.Generate(this, cx, cz);
			c.Update();
			AddChild(c);
			lock(chunk_mutex)
			{
				_loaded_chunks[cpos] = c;
			}
		}
		return cpos;
	}

	public void change_block(int cx, int cz, int bx, int by, int bz, string t)
	{
		var c = _loaded_chunks[new Vector2(cx, cz)];
		if(c._block_data[bx,by,bz].type != t)
		{
			GD.Print($"Changed block at {bx} {by} {bz} in chunk {cx}, {cz}");
			c._block_data[bx, by, bz].create(t);
			c.Update();
		}
	}

	Vector2 _update_chunk(int cx, int cz)
	{
		Vector2 cpos = new Vector2(cx, cz);
		_loaded_chunks[cpos].Update();
		return cpos;
	}

	void enforce_render_distance(Vector2 current_chunk_pos)
	{
		List<Vector2> keyList = new List<Vector2>(_loaded_chunks.Keys);
		for (int Chunklocation = 0; Chunklocation < keyList.Count; Chunklocation++)
		{
			var location = keyList[Chunklocation];
			if (Math.Abs(location.x - current_chunk_pos.x) > load_radius || Math.Abs(location.y - current_chunk_pos.y) > load_radius)
			{
				lock (chunk_mutex)
				{
					_loaded_chunks[location].Free();
					_loaded_chunks.Remove(location);
				}
			}
		}
	}

	void _unloadChunk(int cx, int cz)
	{
		Vector2 cpos = new Vector2(cx, cz);
		if(_loaded_chunks.ContainsKey(cpos))
		{
			lock (chunk_mutex)
			{
				_loaded_chunks[cpos].Free();
				_loaded_chunks.Remove(cpos);
			}
		}
	}

	public void update_player_pos(Vector2 new_pos)
	{
		_new_chunk_pos = new_pos;
	}

	void kill_thread()
	{
		bKillThread = true;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
