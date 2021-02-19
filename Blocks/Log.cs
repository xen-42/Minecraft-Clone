using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Log: Block
    {
        public Log()
        {
            Top = new Vector2(3, 0);
            Bottom = new Vector2(3, 0);
            Left = new Vector2(2, 0);
            Right = new Vector2(2, 0);
            Front = new Vector2(2, 0);
            Back = new Vector2(2, 0);
            Left = new Vector2(2, 0);
            TagsList = new List<Tags>();
        }
    }
}
