using Godot;
using System;
using MinecraftClone;

[Tool]
public class WorldScript : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	KinematicBody Player = null;
	MeshInstance BlockOutline = null;
	Vector2 chunk_pos = new Vector2();

	int chunk_x = 1;
	int chunk_z = 1;

	ProcWorld pw;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player = (KinematicBody)GetNode("Player");
		BlockOutline = (MeshInstance)GetNode("BlockOutline");
		GD.Print("CREATING WORLD");
		pw = new ProcWorld();
		AddChild(pw);
		Connect("tree_exiting", this, "_onWorldScript_tree_exiting");

		Player.Connect("destroy_block", this, "_on_Player_destroy_block");
		Player.Connect("highlight_block", this, "_on_Player_highlight_block");
		Player.Connect("unhighlight_block", this, "_on_Player_unhighlight_block");

	}
	void _on_WorldScript_tree_exiting()
	{
		GD.Print("Kill map loading thread");
		if(pw != null)
		{
			pw.CallDeferred("kill_thread");
		}
		GD.Print("Finished");
	}

	void _on_Player_destroy_block(Vector3 pos, Vector2 norm)
	{
		pos.x -= norm.x;
		pos.z -= norm.y;
		var cx = (int)Mathf.Floor(pos.x / Chunk_cs.DIMENSION.x);
		var cz = (int)Mathf.Floor(pos.z / Chunk_cs.DIMENSION.z);

		int bx = (int)(Mathf.PosMod(Mathf.Floor(pos.x), Chunk_cs.DIMENSION.x) + 0.5);
		int by = (int)(Mathf.PosMod(Mathf.Floor(pos.y), Chunk_cs.DIMENSION.y) + 0.5);
		int bz = (int)(Mathf.PosMod(Mathf.Floor(pos.z), Chunk_cs.DIMENSION.z) + 0.5);

		pw.change_block(cx,cz,bx,by,bz, "Air");
	}

	void _on_Player_highlight_block(Vector3 pos, Vector2 norm)
	{
		BlockOutline.Visible = true;

		pos -= new Vector3(norm.x, norm.y ,norm.x);

		var bx = Mathf.Floor(pos.x) + 0.5;
		var by = Mathf.Floor(pos.y) + 0.5;
		var bz = Mathf.Floor(pos.z) + 0.5;

		BlockOutline.Translation = new Vector3((float)bx, (float)by, (float)bz);

	}

	void _on_Player_unhighlight_block()
	{
		BlockOutline.Visible = false;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if(Player !=  null && pw != null && pw.mutex != null)
		{
			var playerpos = Player.Translation;
			chunk_x = (int)(Mathf.Floor(Player.Translation.x / Chunk_cs.DIMENSION.x));
			chunk_z = (int)(Mathf.Floor(Player.Translation.z / Chunk_cs.DIMENSION.z));

			var new_chunk_pos = new Vector2(chunk_x, chunk_z);

			if (new_chunk_pos != chunk_pos)
			{
				pw.update_player_pos(chunk_pos);
				chunk_pos = new_chunk_pos;

			}

		}
	}
}
