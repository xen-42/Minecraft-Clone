using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Snow: Block
    {
        public Snow()
        {
            Top = new Vector2(1, 2);
            Bottom = new Vector2(1, 0);
            Left = new Vector2(0, 2);
            Right = new Vector2(0, 2);
            Front = new Vector2(0, 2);
            Back = new Vector2(0, 2);
            Left = new Vector2(0, 2);
            TagsList = new List<Tags>();
        }
    }
}
