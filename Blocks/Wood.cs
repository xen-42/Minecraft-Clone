using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Wood: Block
    {
        public Wood()
        {
            Top = new Vector2(4, 0);
            Bottom = new Vector2(4, 0);
            Left = new Vector2(4, 0);
            Right = new Vector2(4, 0);
            Front = new Vector2(4, 0);
            Back = new Vector2(4, 0);
            Left = new Vector2(4, 0);
            TagsList = new List<Tags>();
        }
    }
}
