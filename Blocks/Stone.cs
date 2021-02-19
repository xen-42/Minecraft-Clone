using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Stone: Block
    {
        public Stone()
        {
            Top = new Vector2(0, 0);
            Bottom = new Vector2(0, 0);
            Left = new Vector2(0, 0);
            Right = new Vector2(0, 0);
            Front = new Vector2(0, 0);
            Back = new Vector2(0, 0);
            Left = new Vector2(0, 0);
            TagsList = new List<Tags>();
        }
    }
}
