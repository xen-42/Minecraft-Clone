using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Grass: Block
    {
        public Grass()
        {
            Top = new Vector2(1, 1);
            Bottom = new Vector2(1, 0);
            Left = new Vector2(0, 1);
            Right = new Vector2(0, 1);
            Front = new Vector2(0, 1);
            Back = new Vector2(0, 1);
            Left = new Vector2(0, 1);
            TagsList = new List<Tags>();
        }
    }
}
