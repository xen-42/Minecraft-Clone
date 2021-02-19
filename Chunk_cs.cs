using Godot;
using MinecraftClone;
using MinecraftClone.Blocks;
using System;
using System.Linq;
using System.Collections.Generic;
using MinecraftClone.Chunk_Generator_cs;

public class Chunk_cs : StaticBody
{


	public static readonly Vector3 DIMENSION = new Vector3(16,384, 16);

	readonly BaseGenerator generator = new ForestGenerator();

	static RandomNumberGenerator rng = new RandomNumberGenerator();

	static readonly Vector2 texture_atlas_size = new Vector2(8, 4);

	static SpatialMaterial mat = (SpatialMaterial)GD.Load("res://assets/TextureAtlasMaterial.tres");

	enum Side
	{
		top,
		bottom,
		left,
		right,
		front,
		back,
		only
	}


	static readonly Vector3[] v = new Vector3[]
	{
		new Vector3(0, 0, 0), //0
		new Vector3(1, 0, 0), //1
		new Vector3(0, 1, 0), //2
		new Vector3(1, 1, 0), //3
		new Vector3(0, 0, 1), //4
		new Vector3(1, 0, 1), //5
		new Vector3(0, 1, 1), //6
		new Vector3(1, 1, 1)  //7
	};

	static readonly Vector2[] face_tri_a = new Vector2[]
	{
		new Vector2(0, 0),
		new Vector2(0, 1),
		new Vector2(1, 1)
	};

	static readonly Vector2[] face_tri_b = new Vector2[]
	{
		new Vector2(0, 0),
		new Vector2(0, 1),
		new Vector2(1, 1)
	};

	static readonly int[] TOP = new int[4] {2, 3, 7, 6};
	static readonly int[] BOTTOM = new int[4] { 0, 4, 5, 1 };
	static readonly int[] LEFT = new int[4] { 6, 4, 0, 2 };
	static readonly int[] RIGHT = new int[4] { 3, 1, 5, 7 };
	static readonly int[] FRONT = new int[4] { 7, 5, 4, 6 };
	static readonly int[] BACK = new int[4] { 2, 0, 1, 3 };
	static readonly int[] CROSS_1 = new int[4] { 3, 1, 4, 6 };
	static readonly int[] CROSS_2 = new int[4] { 7, 5, 0, 2 };
	static readonly int[] CROSS_3 = new int[4] { 6, 4, 1, 3 };
	static readonly int[] CROSS_4 = new int[4] { 2, 0, 5, 7 };

	ProcWorld world = null;

	Vector2 chunk_coord = new Vector2();

	Block[,,] Blocks = new Block[(int)DIMENSION.x, (int)DIMENSION.y, (int)DIMENSION.z];
	public BlockData[,,] _block_data = new BlockData[(int)DIMENSION.x, (int)DIMENSION.y, (int)DIMENSION.z];

	SurfaceTool st = new SurfaceTool();
<<<<<<< HEAD
<<<<<<< HEAD
	SurfaceTool collisionmesh_st = new SurfaceTool();
=======
>>>>>>> parent of 08986a2 (The various classes)
=======
	SurfaceTool collisionmesh = new SurfaceTool();
>>>>>>> parent of ec5d647 (Making the nocollision tag work)

	public void Generate(ProcWorld w, float cx, float cz)
	{
		world = w;

		rng.Seed = (ulong)(cx * 1000 + cz);
		chunk_coord = new Vector2(cx, cz);
		Translation = new Vector3(cx * DIMENSION.x, 0, cz * DIMENSION.z);

		int[,] ground_height = new int[(int)DIMENSION.x, (int)DIMENSION.z];

		for(int x = 0; x < DIMENSION.x; x++)
		{
			for(int y =0; y < DIMENSION.y; y++)
			{
				for(int z =0; z < DIMENSION.z; z++)
				{
					BlockData b = new BlockData();
					float h_noise = (float)((world.height_noise.GetNoise2d(x + cx * DIMENSION.x, z + cz * DIMENSION.z) + 1) / 2.0);
					ground_height[x, z] = (int)(h_noise * (128 - 1) + 1);
					b.create(generator.generate_surface(ground_height[x,z], x, y, z));
					_set_block_data(x,y,z,b);
				}
			}
		}
		generator.generate_details(this, rng, ground_height);

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mat.AlbedoTexture.Flags = 2;
	}

	public void _set_block_data(int x, int y, int z, BlockData b, bool overwrite = true)
	{
		if( x >= 0 && x < DIMENSION.x && y >= 0 && y < DIMENSION.y && z >= 0 && z < DIMENSION.z)
		{
			if( overwrite || _block_data[x, y, z].type == "Air")
			{
				_block_data[x, y, z] = b;
			}

		}

	}

	public void Update()
	{
		ArrayMesh mesh = new ArrayMesh();
		MeshInstance mesh_instance = new MeshInstance();

		st.Begin(Mesh.PrimitiveType.Triangles);
<<<<<<< HEAD
<<<<<<< HEAD
		collisionmesh_st.Begin(Mesh.PrimitiveType.Triangles);
=======
>>>>>>> parent of 08986a2 (The various classes)
=======
		collisionmesh.Begin(Mesh.PrimitiveType.Triangles);
>>>>>>> parent of ec5d647 (Making the nocollision tag work)

		//Making use of multidimensional arrays allocated on creation, should speed up this process significantly
		for (int x = 0; x < DIMENSION.x; x++)
		{
			for (int y = 0; y < DIMENSION.y; y++)
			{
				for (int z = 0; z < DIMENSION.z; z++)
				{
					if(_block_data[x,y,z].type != "Air")
					{
						bool[] check = check_transparent_neighbours(x, y, z);
						if(check.Contains(true))
						{
<<<<<<< HEAD
							if(BlockData.block_types[blocktype].TagsList.Contains(Tags.No_Collision))
                            {
								_create_block(check, x, y, z, true);
							}
							else
                            {
								_create_block(check, x, y, z, false);
							}
=======
							_create_block(check, x, y, z);
>>>>>>> parent of 08986a2 (The various classes)
						}
					}
				}
			}
		}
		st.GenerateNormals(false);
		st.SetMaterial(mat);
		st.Commit(mesh);
		collisionmesh.Commit(new ArrayMesh());
		collisionmesh.GenerateNormals(false);
		collisionmesh.
		mesh_instance.Mesh = mesh;
		foreach(Node child in GetChildren())
		{
			if(child.GetClass() == mesh_instance.GetClass())
			{
				child.Free();
			}
		}
		AddChild(mesh_instance);
<<<<<<< HEAD
<<<<<<< HEAD
		AddChild(collisionmesh_ins);
		collisionmesh_ins.CreateTrimeshCollision();
=======
		mesh_instance.CreateTrimeshCollision();
>>>>>>> parent of 08986a2 (The various classes)
=======
>>>>>>> parent of ec5d647 (Making the nocollision tag work)
	}

	bool[] check_transparent_neighbours(int x, int y, int z)
	{
		return new bool[6] { is_block_transparent(x, y + 1, z), is_block_transparent(x, y - 1, z), is_block_transparent(x - 1, y, z), is_block_transparent(x + 1, y, z), is_block_transparent(x, y, z - 1), is_block_transparent(x, y, z + 1) };
	}

	public void _create_block(bool[] check, int x, int y, int z)
	{
		var block = _block_data[x, y, z].type;
		var block_types = BlockData.block_types; 
		if( block_types[block].TagsList.Contains(Tags.Flat))
		{
			create_face(CROSS_1, x, y, z, block_types[block].Only);
			create_face(CROSS_2, x, y, z, block_types[block].Only);
			create_face(CROSS_3, x, y, z, block_types[block].Only);
			create_face(CROSS_4, x, y, z, block_types[block].Only);

		}
		else
		{
			if(check[0])
			{
				create_face(TOP, x, y, z, block_types[block].Top);
			}
			if (check[1])
			{
				create_face(BOTTOM, x, y, z, block_types[block].Bottom);

			}
			if (check[2])
			{
				create_face(LEFT, x, y, z, block_types[block].Left);

			}
			if (check[3])
			{
				create_face(RIGHT, x, y, z, block_types[block].Right);

			}
			if (check[4])
			{
				create_face(BACK, x, y, z, block_types[block].Back);

			}
			if (check[5])
			{
				create_face(FRONT, x, y, z, block_types[block].Front);
			}

		}

	}

	void create_face(int[] i, int x, int y, int z, Vector2 texture_atlas_offset)
	{
		Vector3 offset = new Vector3(x, y, z);

		Vector3 a = v[i[0]] + offset;
		Vector3 b = v[i[1]] + offset;
		Vector3 c = v[i[2]] + offset;
		Vector3 d = v[i[3]] + offset;

		Vector2 uv_offset = new Vector2(
										texture_atlas_offset.x / texture_atlas_size.x,
										texture_atlas_offset.y / texture_atlas_size.y
										);

		// the f means float, there is another type called double it defaults to that has better accuracy at the cost of being larger to store, but vector3 does not use it.
		var uv_a = new Vector2(0f, 0f) + uv_offset;
		var uv_b = new Vector2(0, 1.0f / texture_atlas_size.y) + uv_offset;
		var uv_c = new Vector2(1.0f / texture_atlas_size.x, 1.0f / texture_atlas_size.y) + uv_offset;
		var uv_d = new Vector2(1.0f / texture_atlas_size.x, 0) + uv_offset;
		// Add UVs and tris
		st.AddTriangleFan(new Vector3[] { a, b, c }, new Vector2[] { uv_a, uv_b, uv_c });
		st.AddTriangleFan(new Vector3[] { a, c, d }, new Vector2[] { uv_a, uv_c, uv_d });
<<<<<<< HEAD

		if(!NoCollision)
        {
			collisionmesh.AddTriangleFan(new Vector3[] { a, b, c }, new Vector2[] { uv_a, uv_b, uv_c });
			collisionmesh.AddTriangleFan(new Vector3[] { a, c, d }, new Vector2[] { uv_a, uv_c, uv_d });
		}


=======
>>>>>>> parent of 08986a2 (The various classes)
	}

	bool is_block_transparent(int x, int y, int z)
	{
		if(x < 0 || x >= DIMENSION.x || z< 0 || z >= DIMENSION.z || y< 0 || y >= DIMENSION.y)
		{
			return true;
		}
		else
		{
			return _block_data[x, y, z].Transparent;
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
