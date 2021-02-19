using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    public enum Tags
    {
        Transparent,
        No_Collision,
        Flat,
        Air
    }
    public class Block
    {
        public Vector2 Top = new Vector2(-1, -1);
        public Vector2 Bottom = new Vector2(-1, -1);
        public Vector2 Left = new Vector2(-1, -1);
        public Vector2 Right = new Vector2(-1, -1);
        public Vector2 Front = new Vector2(-1, -1);
        public Vector2 Back = new Vector2(-1, -1);
        public Vector2 Only = new Vector2(-1, -1);

        public List<Tags> TagsList = new List<Tags>() { Tags.Air, Tags.Transparent };
    }
}
